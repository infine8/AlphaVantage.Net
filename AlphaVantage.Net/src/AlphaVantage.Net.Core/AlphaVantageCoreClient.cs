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

            var jsonString = await response.Content.ReadAsStringAsync();
            var jObject = (JObject)JsonConvert.DeserializeObject(jsonString);

            AssertNotBadRequest(jObject);

            return jObject;
        }

        private HttpRequestMessage ComposeHttpRequest(string apiKey, ApiFunction function, IDictionary<string, string> query)
        {
            var fullQueryDict = new Dictionary<string, string>(query)
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