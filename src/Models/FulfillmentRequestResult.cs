using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace SaaSFulfillmentClient.Models
{
    public class FulfillmentRequestResult
    {
        private const string RequestIdKey = "x-ms-requestid";

        public FulfillmentRequestResult()
        {
            this.Success = false;
        }

        public Guid RequestId { get; set; }

        public bool Success { get; set; }

        public static async Task<T> ParseAsync<T>(HttpResponseMessage response)
            where T : FulfillmentRequestResult, new()
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            T result;

            if (jsonString != string.Empty)
            {
                result = JsonConvert.DeserializeObject<T>(jsonString);
            }
            else
            {
                result = (T)Convert.ChangeType(new T(), typeof(T));
            }

            result.UpdateFromHeaders(response.Headers);

            result.Success = response.IsSuccessStatusCode;

            return result;
        }

        public static async Task<IEnumerable<T>> ParseMultipleAsync<T>(HttpResponseMessage response)
            where T : FulfillmentRequestResult, new()
        {
            var jsonString = await response.Content.ReadAsStringAsync();

            var results = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);

            foreach (var result in results)
            {
                result.UpdateFromHeaders(response.Headers);
            }

            return results;
        }

        protected virtual void UpdateFromHeaders(HttpHeaders headers)
        {
            if (headers.TryGetValues(RequestIdKey, out var values))
            {
                this.RequestId = Guid.Parse(values.First());
            }
        }
    }
}
