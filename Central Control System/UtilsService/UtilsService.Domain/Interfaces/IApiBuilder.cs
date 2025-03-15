namespace UtilsService.Domain.Interfaces
{
    public interface IApiBuilder
    {
        public Task<T> GetRequest<T>(string link, string clientName, CancellationToken cancellation);
        public Task<T> PostRequest<T>(string link, object value, string clientName, CancellationToken cancellation);
        public Task<HttpResponseMessage> PostRequestWithoutDeserializing(string link, object value, string clientName, CancellationToken cancellation);
        public Task<T> PutRequest<T>(string link, object value, string clientName, CancellationToken cancellation);
        public Task<HttpResponseMessage> PutRequestWithoutDeserializing(string link, object value, string clientName, CancellationToken cancellation);
        public Task<HttpResponseMessage> DeleteRequest(string link, object value, string clientName, CancellationToken cancellation);
    }
}
