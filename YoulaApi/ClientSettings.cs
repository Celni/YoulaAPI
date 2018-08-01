using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using YoulaApi.Models;

namespace YoulaApi
{
    public class ClientSettings
    {
        /// <summary>
        /// Идентификатор пользователя в Youla
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Номер мобильного телефона
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// Идентификатор системы
        /// </summary>
        public string SystemId { get; set; }

        /// <summary>
        /// Координата Lat
        /// </summary>
        public double UserLat { get; set; }

        /// <summary>
        /// Координата Lon
        /// </summary>
        public double UserLon { get; set; }

        public string UserAgent { get; set; }

        /// <summary>
        /// Токен прошлой авторизации (если была)
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Web прокси
        /// </summary>
        public IWebProxy Proxy { get; set; }

        public bool IsUseToken => !string.IsNullOrWhiteSpace(Token);

        public bool IsUseProxy => Proxy != null;

        public ClientSettings(string userId, string phone, string systemId, double userLat, double userLon,
            string userAgent, string token, IWebProxy proxy)
        {
            UserId = userId;
            Phone = phone;
            SystemId = systemId;
            UserLat = userLat;
            UserLon = userLon;
            UserAgent = userAgent;
            Token = token;
            Proxy = proxy;
        }
    }
}
