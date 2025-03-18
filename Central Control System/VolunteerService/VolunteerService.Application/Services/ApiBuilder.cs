using Newtonsoft.Json;
using System.Net;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Domain.Interfaces;

namespace VolunteerService.Application.Services
{
    public class ApiBuilder : IApiBuilder
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ApiBuilder(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<T> GetRequest<T>(string link, string clientName, CancellationToken cancellation, string token = "")
        {
            var response = await GetClient(clientName, token).GetAsync(link, cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<T> PostRequest<T>(string link, object value, string clientName, CancellationToken cancellation, string token = "")
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName, token).PostAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<HttpResponseMessage> PostRequestWithoutDeserializing(string link, object value, string clientName, CancellationToken cancellation, string token = "")
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName, token).PostAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }

        public async Task<T> PutRequest<T>(string link, object value, string clientName, CancellationToken cancellation, string token = "")
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName, token).PutAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<T>(res);
        }

        public async Task<HttpResponseMessage> PutRequestWithoutDeserializing(string link, object value, string clientName, CancellationToken cancellation, string token = "")
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName, token).PutAsync(link, new StringContent(json, null, "application/json"), cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }

        public async Task<HttpResponseMessage> DeleteRequest(string link, object value, string clientName, CancellationToken cancellation, string token = "")
        {
            var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings());
            var response = await GetClient(clientName, token).DeleteAsync(link, cancellation);

            await CheckResponse(response);

            string res = await response.Content.ReadAsStringAsync();
            return response;

        }

        private HttpClient GetClient(string clientName, string? token = null)
        {
            var client = _httpClientFactory.CreateClient(clientName);

            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            return client;
        }

        private async Task CheckResponse(HttpResponseMessage? response)
        {
            if (response == null)
            {
                throw new VolunteerServiceException(Constants.EmptyResponseException);
            }

            if (response.StatusCode == HttpStatusCode.InternalServerError || response.StatusCode == HttpStatusCode.Unauthorized || response.StatusCode == HttpStatusCode.NotFound)
            {
                string res = await response.Content.ReadAsStringAsync();
                throw new VolunteerServiceException(string.IsNullOrEmpty(res) ? Constants.DefaultException : res);
            }
        }
    }
}
