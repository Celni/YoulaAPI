using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Isec.Protocol.Helpers;
using Newtonsoft.Json;
using YoulaApi.Exceptions;
using YoulaApi.Models;

namespace YoulaApi
{
    public class YoulaClient : IDisposable
    {

        public ClientSettings ClientSettings { get; private set; }
        public UserData UserData { get; private set; }

        private HttpClient _client;
        private string _sessionId;

        private string _url = "https://clientapi.mail.ru/fcgi-bin/";
        private string _urlApiYoula = "https://api.youla.io/api/v1/";

        private VerifyResponse _verifyResponse;
        private VerifyUriRequest _verifyUriRequest;

        public YoulaClient(ClientSettings settings)
        {
            ClientSettings = settings ?? throw new ArgumentNullException(nameof(settings));

            if (settings.IsUseProxy)
            {
                var clientHandler = new HttpClientHandler { Proxy = settings.Proxy };
                _client = new HttpClient(clientHandler);
            }
            else
            {
                _client = new HttpClient();
            }

            if (settings.IsUseToken)
            {
                _client.DefaultRequestHeaders.UserAgent.Clear();
                _client.DefaultRequestHeaders.Add("User-Agent", string.IsNullOrWhiteSpace(settings.UserAgent) ? Const.UserAgentYoula : settings.UserAgent);
                _client.DefaultRequestHeaders.Add("X-Auth-Token", settings.Token);
            }
            else
            {
                _client.DefaultRequestHeaders.UserAgent.Clear();
                _client.DefaultRequestHeaders.Add("User-Agent", Const.UserAgentMail);
            }

        }


        /// <summary>
        /// Первый шаг авторизации - отправка номера на сервер
        /// </summary>
        /// <returns></returns>
        public async Task FirstStepAuth()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Phone))
            {
                throw new AuthMailException("Не задан номер телефона");
            }

            var request = new VerifyUriRequest(ClientSettings.Phone);
            request.LocationLat = ClientSettings.UserLat > 0.01 ? request.LocationLat : ClientSettings.UserLat;
            request.LocationLon = ClientSettings.UserLon > 0.01 ? request.LocationLon : ClientSettings.UserLon;
            request.SystemId = string.IsNullOrWhiteSpace(ClientSettings.SystemId)
                ? request.SystemId
                : ClientSettings.SystemId;

            request.ApplicationId = Guid.NewGuid().ToString();
            var response = await _client.GetStringAsync(_url + request.CreateUri());
            _verifyResponse = JsonConvert.DeserializeObject<VerifyResponse>(response);

            if (_verifyResponse.Status != "OK")
            {
                throw new AuthMailException(_verifyResponse.Description);
            }

            _verifyUriRequest = request;
            _sessionId = request.SessionId;
            ClientSettings.SystemId = request.SystemId;
        }

        /// <summary>
        /// Второй шаг авторизации (подтверждение авторизации)
        /// </summary>
        /// <param name="code">Код отправленный в СМС</param>
        /// <returns>Объект данных пользователя</returns>
        public async Task SecondStepAuth(string code)
        {
            if (_verifyResponse == null)
            {
                throw new AuthMailException("Вы не прошли первую стадию авторизации.");
            }

            var request = new AttemptUriRequest(_verifyResponse, code);
            request.ApplicationId = _verifyUriRequest.ApplicationId;
            var response = await _client.GetStringAsync(_url + request.CreateUri());
            AttempResponse attempResponse = JsonConvert.DeserializeObject<AttempResponse>(response);
            if (attempResponse.Status != "OK")
            {
                throw new AuthMailException(attempResponse.Description);
            }

            ClientSettings.Phone = attempResponse.ModifiedPhoneNumber;

            var youlaRequest = JsonConvert.SerializeObject(new AuthRequestYoula
            {
                Uid = ClientSettings.SystemId,
                Phone = ClientSettings.Phone,
                SessionId = _sessionId,
                Token = attempResponse.Token
            });
            _client.DefaultRequestHeaders.Add("User-Agent", string.IsNullOrWhiteSpace(ClientSettings.UserAgent) ? Const.UserAgentYoula : ClientSettings.UserAgent);
            var youlaResponse = await _client.PostAsync($"{_urlApiYoula}auth/validate?{this.CreatePayloadUri()}", new StringContent(youlaRequest, Encoding.UTF8, "application/json"));
            youlaResponse.EnsureSuccessStatusCode();

            var str = await youlaResponse.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DataRoot>(str, new KnownTypeConverter());
            if (data == null || data.Status != 200)
            {
                throw new YoulaException(data?.Status ?? 0, data?.Detail);
            }

            var userData = (UserData)data.Data.First();
            ClientSettings.Token = userData.Token;
            ClientSettings.UserId = userData.Id;

            try
            {
                await this.OAuthAutorize();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }

            _client.DefaultRequestHeaders.Add("User-Agent", string.IsNullOrWhiteSpace(ClientSettings.UserAgent) ? Const.UserAgentYoula : ClientSettings.UserAgent);
            _client.DefaultRequestHeaders.Add("X-Auth-Token", ClientSettings.Token);

            UserData = userData;
        }


        /// <summary>
        /// Загрузка изображений на сервер
        /// </summary>
        /// <param name="files">Список файлов для загрузки на сервер</param>
        /// <returns>Список объектов содержащий информацию о загруженных файлах</returns>
        public async Task<List<ImageResponse>> UploadImages(List<FilesUpload> files)
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            using (var content = new MultipartFormDataContent())
            {
                foreach (var file in files)
                {
                    var imageContent = new ByteArrayContent(file.ImageData);
                    imageContent.Headers.ContentType =
                        MediaTypeHeaderValue.Parse(file.ContentType);
                    imageContent.Headers.ContentLength = file.ImageData.Length;
                    content.Add(imageContent, "images[]", file.FileName);
                }

                var response = await _client.PostAsync($"{_urlApiYoula}images?{this.CreatePayloadUri()}", content);
                response.EnsureSuccessStatusCode();
                var dataRoot = JsonConvert.DeserializeObject<DataRoot>(await response.Content.ReadAsStringAsync(), new KnownTypeConverter());
                if (dataRoot == null || dataRoot.Status != 200)
                {
                    throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
                }

                var imageResponse = dataRoot.Data.ConvertAll(x => (ImageResponse)x);
                return imageResponse;
            }
        }

        /// <summary>
        /// Обновить данный пользователя
        /// </summary>
        /// <param name="User">Новый профиль пользователя</param>
        /// <returns></returns>
        public async Task UpdateUser(UserProfile user)
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var request = JsonConvert.SerializeObject(user);
            var response = await _client.PostAsync($"{_urlApiYoula}user/{ClientSettings.UserId}?{this.CreatePayloadUri()}",
                new StringContent(request, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var dataRoot = JsonConvert.DeserializeObject<DataRoot>(await response.Content.ReadAsStringAsync(),
                new KnownTypeConverter());
            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }

            var userData = (UserData)dataRoot.Data.First();

            UserData = userData;
        }

        /// <summary>
        /// Обновить местоположение пользователя в профиле
        /// </summary>
        /// <param name="city">прим. Москва</param>
        /// <param name="lon">Координата</param>
        /// <param name="lat">Координата</param>
        /// <returns></returns>
        public async Task UpdateUserLocation(string city, double lon, double lat)
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var request = JsonConvert.SerializeObject(new
            {
                settings = new
                {
                    location = new
                    {
                        description = city,
                        longitude = lon,
                        latitude = lat
                    }
                }
            });

            var response = await _client.PostAsync($"{_urlApiYoula}user/{ClientSettings.UserId}?{this.CreatePayloadUri()}",
                new StringContent(request, Encoding.UTF8, "application/json"));
            response.EnsureSuccessStatusCode();

            var dataRoot = JsonConvert.DeserializeObject<DataRoot>(await response.Content.ReadAsStringAsync(),
                new KnownTypeConverter());
            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }

            var userData = (UserData)dataRoot.Data.First();

            ClientSettings.UserLat = lat;
            ClientSettings.UserLon = lon;

            UserData = userData;
        }

        /// <summary>
        /// Получить профиль авторизованного пользователя
        /// </summary>
        /// <returns>Профиль пользователя</returns>
        public async Task UpdateUserData()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var response = await _client.GetStringAsync($"{_urlApiYoula}user/{ClientSettings.UserId}?{this.CreatePayloadUri()}");

            var data = JsonConvert.DeserializeObject<DataRoot>(response, new KnownTypeConverter());

            if (data == null || data.Status != 200)
            {
                throw new YoulaException(data?.Status ?? 0, data?.Detail);
            }


            var userData = (UserData)data.Data.First();

            UserData = userData;
        }

        /// <summary>
        /// Получить бонусы
        /// </summary>
        /// <returns>Профиль авторизованного пользователя</returns>
        public async Task UserPutBonus()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var response = await _client.GetStringAsync($"{_urlApiYoula}bonus/daily?{this.CreatePayloadUri()}");
            var dataRoot = JsonConvert.DeserializeObject<DataRoot>(response, new KnownTypeConverter());
            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }

            var checkBonusResponse = dataRoot?.Data.First() as CheckBonusResponse;

            if (checkBonusResponse == null || string.IsNullOrWhiteSpace(checkBonusResponse.PopupId))
            {
                throw new YoulaException("Не верный ответ от сервера.");
            }

            var request = JsonConvert.SerializeObject(new
            {
                popup_id = checkBonusResponse.PopupId
            });

            var responseBonuse = await _client.PostAsync($"{_urlApiYoula}bonus/daily/apply?{this.CreatePayloadUri()}",
                new StringContent(request, Encoding.UTF8, "application/json"));

            responseBonuse.EnsureSuccessStatusCode();

            dataRoot = JsonConvert.DeserializeObject<DataRoot>(await responseBonuse.Content.ReadAsStringAsync(), new KnownTypeConverter());

            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }

            var result = dataRoot.Data.First() as UserData;

            UserData = result;
        }

        /// <summary>
        /// Получить счётчики
        /// </summary>
        /// <returns>Счётчики чегото включая новые сообщения на сайте</returns>
        public async Task<Counters> UserCounters()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var response = await _client.GetStringAsync($"{_urlApiYoula}counters/{ClientSettings.UserId}?{this.CreatePayloadUri()}");
            var deserial = new
            {
                data = new Counters(),
                status = 0,
                detail = ""
            };

            var dataRoot = JsonConvert.DeserializeAnonymousType(response, deserial);
            if (dataRoot == null || dataRoot.status != 200)
            {
                throw new YoulaException(dataRoot?.status ?? 0, dataRoot?.detail);
            }

            return dataRoot.data;
        }

        /// <summary>
        /// Получить список товаров авторизованного пользователя
        /// </summary>
        /// <returns>Список товаров пользователя</returns>
        public async Task<List<Product>> UserProducts()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var response = await _client.GetStringAsync($"{_urlApiYoula}user/{ClientSettings.UserId}/profile/products?{this.CreatePayloadUri()}");
            var dataRoot = JsonConvert.DeserializeObject<DataRoot>(response, new KnownTypeConverter());
            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }
            var result = dataRoot.Data.ConvertAll(x => (Product)x);
            return result;
        }

        /// <summary>
        /// Поднимает товар в категории за балы
        /// </summary>
        /// <param name="productId">Идентификатор товара пользователя</param>
        /// <returns>Товар который подняли в категории</returns>
        public async Task<Product> UserProductUp(string productId)
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new YoulaException("Не указан идентификатор товара.");
            }

            var response = await _client.PostAsync($"{_urlApiYoula}products/{productId}/up?{this.CreatePayloadUri()}", new StringContent("", Encoding.UTF8, "application/json"));
            var str = await response.Content.ReadAsStringAsync();
            var deserial = new
            {
                data = new Product(),
                status = 0,
                detail = ""
            };

            var dataRoot = JsonConvert.DeserializeAnonymousType(str, deserial);

            if (dataRoot == null || dataRoot.status != 200)
            {
                throw new YoulaException(dataRoot?.status ?? 0, dataRoot?.detail);
            }

            return dataRoot.data;
        }

        /// <summary>
        /// Получить произвольный товар
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Идентификатор товара</returns>
        public async Task<Product> GetProduct(string productId)
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            if (string.IsNullOrWhiteSpace(productId))
            {
                throw new YoulaException("Не указан идентификатор товара.");
            }

            var response = await _client.GetStringAsync($"{_urlApiYoula}product/{productId}?{this.CreatePayloadUri()}");
            var deserial = new
            {
                data = new Product(),
                status = 0,
                detail = ""
            };
            var dataRoot = JsonConvert.DeserializeAnonymousType(response, deserial);
            if (dataRoot == null || dataRoot.status != 200)
            {
                throw new YoulaException(dataRoot?.status ?? 0, dataRoot?.detail);
            }

            return dataRoot.data;
        }

        /// <summary>
        /// Получить дерево категорий
        /// </summary>
        /// <returns>Дерево категорий</returns>
        public async Task<List<Categorys>> GetCategorys()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var response = await _client.GetStringAsync($"{_urlApiYoula}category_tree?{this.CreatePayloadUri()}");
            var dataRoot = JsonConvert.DeserializeObject<DataRoot>(response, new KnownTypeConverter());
            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }

            return dataRoot.Data.ConvertAll(x=> (Categorys)x);
        }
        
        /// <summary>
        /// Получить спосок всех чатов пользователя с сообщениями и товароми
        /// </summary>
        /// <returns>Список чатов</returns>
        public async Task<List<Chats>> UserChats()
        {
            if (string.IsNullOrWhiteSpace(ClientSettings.Token))
            {
                throw new AuthMailException("Пользователь не авторизован.");
            }

            var response =await _client.GetStringAsync($"{_urlApiYoula}chats/recipient/{ClientSettings.UserId}");

            var dataRoot = JsonConvert.DeserializeObject<DataRoot>(response, new KnownTypeConverter());
            if (dataRoot == null || dataRoot.Status != 200)
            {
                throw new YoulaException(dataRoot?.Status ?? 0, dataRoot?.Detail);
            }

            return dataRoot.Data.ConvertAll(x => (Chats)x);
        }

        private async Task OAuthAutorize()
        {
            var oAuthStr = $"client_id=youla.and&client_secret=qNZTF23DwVnw&grant_type=password&username=youla_{ClientSettings.UserId}&password={ClientSettings.Token}";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("User-Agent", "okhttp/3.9.1");

            var oauthContext = new StringContent(oAuthStr);
            oauthContext.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");
            var oAuthResponse = await _client.PostAsync($"{_urlApiYoula}oauth/access_token", oauthContext);
            if (!oAuthResponse.IsSuccessStatusCode)
            {
                oAuthResponse.EnsureSuccessStatusCode();
            }
            var oAuthResponseStr = await oAuthResponse.Content.ReadAsStringAsync();
            var oAuth = JsonConvert.DeserializeObject<OAuth>(oAuthResponseStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", oAuth.AccessToken);
            _client.DefaultRequestHeaders.Add("X-API-CLIENT", "android_app_youla");
            _client.DefaultRequestHeaders.Add("X-API-KEY", "doesnotexist");
            var amContext = JsonConvert.SerializeObject(new List<object>
            {
                new {method="user.getProfile",jsonrpc="2.0",id=4},
                new {method="youlaFavorites.getUserFavorites",jsonrpc="2.0",id=5}
            });
            var amResponse =
                await _client.PostAsync(
                    "https://am.ru/rpc-api-0.0.1/?applicationMethod=user.getProfile,youlaFavorites.getUserFavorites",
                    new StringContent(amContext, Encoding.UTF8, "application/json"));
            if (!amResponse.IsSuccessStatusCode)
            {
                amResponse.EnsureSuccessStatusCode();
            }

            _client.DefaultRequestHeaders.Clear();
        }

        private string CreatePayloadUri()
        {
            var time = DateTimeOffset.Now.ToUnixTimeSeconds();
            var str = new StringBuilder();
            str.Append($"uid={ClientSettings.SystemId}");
            str.Append($"&app_id={Const.AppId}");
            str.Append($"&usr_latitude={ClientSettings.UserLat.ToString().Replace(",", ".")}");
            str.Append($"&usr_longitude={ClientSettings.UserLon.ToString().Replace(",", ".")}");
            str.Append($"&adv_id=2d4a72d0-783c-4f22-a8d3-0261dc2ecefe");
            str.Append($"&timestamp={time}");
            return str.ToString();
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
