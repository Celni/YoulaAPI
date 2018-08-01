using System;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YoulaApi.Models;

namespace Isec.Protocol.Helpers
{
    internal class KnownTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return System.Attribute.GetCustomAttributes(objectType).Any(v => v is KnownTypeAttribute);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            // Load JObject from stream
            JObject jObject = JObject.Load(reader);

            // Create target object based on JObject
            System.Attribute[] attrs = System.Attribute.GetCustomAttributes(objectType);  // Reflection. 

            // Displaying output. 
            foreach (System.Attribute attr in attrs)
            {
                if (attr is KnownTypeAttribute)
                {
                    KnownTypeAttribute k = (KnownTypeAttribute)attr;
                    var props = k.Type.GetProperties();
                    bool found = true;
                    foreach (var f in jObject)
                    {
                        if (props.Any(z => z.Name.Equals("UnreadCount")) && f.Key.Equals("unread_count"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(Chats));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                        if (props.Any(z => z.Name.Equals("Token")) && f.Key.Equals("token"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(UserData));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                        if (props.Any(z => z.Name.Equals("PopupId")) && f.Key.Equals("popup_id"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(CheckBonusResponse));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                        if (props.Any(z => z.Name.Equals("Type")) && f.Key.Equals("type") && f.Value.ToObject<string>().Equals("product"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(Product));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                        if (props.Any(z => z.Name.Equals("Chats")) && f.Key.Equals("chats"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(Counters));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                        if (props.Any(z => z.Name.Equals("Height")) && f.Key.Equals("height"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(ImageResponse));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                        if (props.Any(z => z.Name.Equals("SlugSeo")) && f.Key.Equals("slug_seo"))
                        {
                            object target = null;

                            target = Activator.CreateInstance(typeof(Categorys));

                            serializer.Populate(jObject.CreateReader(), target);
                            return target;
                        }
                    }
                }
            }
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
