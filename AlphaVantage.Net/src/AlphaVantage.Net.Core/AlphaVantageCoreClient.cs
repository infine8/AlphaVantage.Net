using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using AlphaVantage.Net.Core.Exceptions;
using AlphaVantage.Net.Core.InternalHttpClient;
using AlphaVantage.Net.Core.Validation;
using JetBrains.Annotations;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AlphaVantage.Net.Core
{
    public class AlphaVantageCoreClient
    {
        [CanBeNull]
        private readonly IApiCallValidator _apiCallValidator;

        [CanBeNull]
        private readonly TimeSpan? _timeout;

        [CanBeNull]
        private readonly HttpClient _httpClient;


        public AlphaVantageCoreClient(IApiCallValidator apiCallValidator = null, TimeSpan? timeout = null)
        {
            _apiCallValidator = apiCallValidator;

            _timeout = timeout;
        }

        public AlphaVantageCoreClient(HttpClient httpClient, IApiCallValidator apiCallValidator = null)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiCallValidator = apiCallValidator;
        }

        public virtual async Task<JObject> RequestApiAsync(string apiKey, ApiFunction function, IDictionary<string, string> query = null)
        {
            var responseBody = await SendRequestAsync(apiKey, function, query);

            var jObject = (JObject)JsonConvert.DeserializeObject(responseBody);

            AssertNotBadRequest(jObject);

            return jObject;
        }

        /// <summary>
        /// Sends the same underlying HTTP request as <see cref="RequestApiAsync"/> but returns
        /// the raw response body as a string. Use for endpoints that return non-JSON payloads
        /// (e.g. LISTING_STATUS returns CSV).
        /// </summary>
        public virtual async Task<string> RequestApiRawAsync(string apiKey, ApiFunction function, IDictionary<string, string> query = null)
        {
            return await SendRequestAsync(apiKey, function, query);
        }

        private async Task<string> SendRequestAsync(string apiKey, ApiFunction function, IDictionary<string, string> query)
        {
            AssertValid(function, query);

            var request = ComposeHttpRequest(apiKey, function, query);

            HttpResponseMessage response;
            if (_httpClient != null)
            {
                response = await _httpClient.SendAsync(request);
            }
            else
            {
                using var client = new HttpClientWithRateLimit(new HttpClient(), 20, 10);
                if (_timeout.HasValue) client.SetTimeOut(_timeout.Value);
                response = await client.SendAsync(request);
            }

            return await response.Content.ReadAsStringAsync();
        }

        private HttpRequestMessage ComposeHttpRequest(string apiKey, ApiFunction function, IDictionary<string, string> query)
        {
            var fullQueryDict = new Dictionary<string, string>(query ?? new Dictionary<string, string>())
            {
                { ApiConstants.ApiKeyQueryVar, apiKey },
                { ApiConstants.FunctionQueryVar, function.ToString() }
            };

            var paramString = HttpUtility.ParseQueryString(string.Empty);
            foreach (var kvp in fullQueryDict)
            {
                paramString[kvp.Key] = kvp.Value;
            }

            var urlWithQueryString = $"{ApiConstants.AlfaVantageUrl}?{paramString}";
            var urlWithQuery = new Uri(urlWithQueryString);

            var request = new HttpRequestMessage
            {
                RequestUri = urlWithQuery,
                Method = HttpMethod.Get
            };

            return request;
        }

        private void AssertValid(ApiFunction function, IDictionary<string, string> query = null)
        {
            if(_apiCallValidator == null) return;

            var validationResult = _apiCallValidator.Validate(function, query);

            if(!validationResult.IsValid)
                throw new AlphaVantageException(validationResult.ErrorMsg);
        }

        private void AssertNotBadRequest(JObject jObject)
        {
            if(jObject.ContainsKey(ApiConstants.BadRequestToken))
                throw new AlphaVantageException(jObject[ApiConstants.BadRequestToken].ToString());
        }
    }
}
