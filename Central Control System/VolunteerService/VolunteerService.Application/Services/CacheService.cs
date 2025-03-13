using System.Collections.Concurrent;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Services
{
    public sealed class CacheService<T> : ICacheService<T> where T : class
    {
        private readonly ConcurrentDictionary<string, List<T>> _cachedEntites;

        public CacheService()
        {
            _cachedEntites = new ConcurrentDictionary<string, List<T>>();
        }

        public List<T>? Get(string key) =>
            _cachedEntites.TryGetValue(key, out var result) ? result : null;

        public void Reset() => _cachedEntites.Clear();

        public void Set(string key, List<T> entites) =>
            _cachedEntites[key] = entites;
    }
}
