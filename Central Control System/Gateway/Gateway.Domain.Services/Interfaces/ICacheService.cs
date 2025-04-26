namespace Gateway.Domain.Services.Interfaces
{
    public interface ICacheService<T> where T : class
    {
        List<T>? Get(string key);
        void Set(string key, List<T> entities);
        void Remove(string key);
        void Reset();
    }
}
