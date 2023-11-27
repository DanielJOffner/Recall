namespace LT.Recall.Application.Abstractions
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T obj);
        T Deserialize<T>(string json) where T : new();
    }
}
