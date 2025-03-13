namespace VolunteerService.Domain.Interfaces
{
    public interface ICacheService<T> where T : class
    {
        List<T>? Get(string key);
        void Set(string key, List<T> entites);
        void Reset();
    }
}
