using Newtonsoft.Json;
using System.Net;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Exceptions;
using UtilsService.Domain.Interfaces;

namespace UtilsService.Application.Services
{
    public class ApiBuilder : IApiBuilder
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiBuilder(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetRequest<T>(string link, string clientName, CancellationToken cancellation)
        {
            var response = await GetClient(clientName).GetAsync(link, cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<T> PostRequest<T>(string link, object value, string clientName, CancellationToken cancellation)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName).PostAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<HttpResponseMessage> PostRequestWithoutDeserializing(string link, object value, string clientName, CancellationToken cancellation)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName).PostAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }

        public async Task<T> PutRequest<T>(string link, object value, string clientName, CancellationToken cancellation)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName).PutAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<HttpResponseMessage> PutRequestWithoutDeserializing(string link, object value, string clientName, CancellationToken cancellation)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName).PutAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }

        public async Task<HttpResponseMessage> DeleteRequest(string link, object value, string clientName, CancellationToken cancellation)
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName).DeleteAsync(link, cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }

        private HttpClient GetClient(string clientName) => _httpClientFactory.CreateClient(clientName);

        private async Task CheckResponse(HttpResponseMessage? response)
        {
            if (response == null)
            {
                throw new UtilsServiceException(Constants.EmptyResponseException);
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound)
            {
                string res = await response.Content.ReadAsStringAsync();
                throw new UtilsServiceException(res);
            }
        }
    }
}
