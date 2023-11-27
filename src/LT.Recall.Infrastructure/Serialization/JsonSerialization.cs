using LT.Recall.Application.Abstractions;
using Newtonsoft.Json;

namespace LT.Recall.Infrastructure.Serialization
{
    public class RecallJsonSerializer : IJsonSerializer
    {
        private readonly JsonSerializerSettings _settings = new()
        {
        };

        public string Serialize<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj, _settings);
        }

        public T Deserialize<T>(string json) where T : new()    
        {
            return JsonConvert.DeserializeObject<T>(json, _settings) ?? new T();
        }
    }
}
