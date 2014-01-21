using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySynopsis.BusinessLogic.JsonConverters
{
    public class ListConverter<T> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(List<T>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var listAsString = serializer.Deserialize<string>(reader);
            return listAsString == null ?
                null :
                JsonConvert.DeserializeObject<List<T>>(listAsString);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var listAsString = JsonConvert.SerializeObject(value);
            serializer.Serialize(writer, listAsString);
        }
    }
}
