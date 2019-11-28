using Super.Core.Infrastruct.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Super.Core.Infrastruct.Http
{
    public delegate Task<T> ResponseHandlerDelegate<T>(HttpResponseMessage response) where T : class;

    public class CommonHttpClient<T> : IDisposable where T : class
    {
        readonly HttpClient _client;

        private ResponseHandlerDelegate<T> responseHandler;

        public CommonHttpClient()
        {
            this._client = new HttpClient();
            this.responseHandler = DefaultResponseHandler;
        }

        public CommonHttpClient(ResponseHandlerDelegate<T> responseHandler)
        {
            this._client = new HttpClient();
            this.responseHandler = responseHandler;
        }

        public void Setting(Action<HttpClient> action)
        {
            action(this._client);
        }

        public void SetAuth(string scheme,string token)
        {
            this._client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(scheme, token);
        }

        public async Task<T> GetAsync(string url)
        {
            var response = await this._client.GetAsync(url);
            //response.EnsureSuccessStatusCode();
            var result = await this.responseHandler(response);
            return result;
        }

        public async Task<T> GetAsync(string url, IEnumerable<KeyValuePair<string, object>> parameter)
        {
            url = UrlJoin(url, parameter);
            return await this.GetAsync(url);
        }

        public async Task<T> GetAsync<TData>(string url, TData parameter)
        {
            var lstKV = from p in typeof(TData).GetProperties()
                        select new KeyValuePair<string, object>(p.Name, p.GetValue(parameter));
            return await this.GetAsync(url, lstKV);
        }

        public async Task<T> PostAsync(string url, string json)
        {
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this._client.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
            var result = await this.responseHandler(response);
            return result;
        }

        public async Task<T> PostAsync<TData>(string url, TData data)
        {
            string json = data.ToJson();
            return await this.PostAsync(url, json);
        }

        public async Task<T> PostAsync(string url, Dictionary<string, object> data)
        {
            string json = data.ToJson();
            return await this.PostAsync(url, json);
        }

        public async Task<T> PutAsync(string url, Dictionary<string, object> data)
        {
            string json = data.ToJson();
            var content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = await this._client.PutAsync(url, content);
            //response.EnsureSuccessStatusCode();
            var result = await this.responseHandler(response);
            return result;
        }

        public async Task<T> DeleteAsync(string url)
        {
            var response = await this._client.DeleteAsync(url);
            //response.EnsureSuccessStatusCode();
            var result = await this.responseHandler(response);
            return result;
        }

        public string UrlJoin(string url, IEnumerable<KeyValuePair<string,object>> parameters)
        {
            if (parameters == null || parameters.Count() < 1) return url;

            var lstExpression = parameters.Select(x => $"{x.Key}={x.Value}");

            if (url.EndsWith("?") || url.EndsWith("&"))
            {
                url = url + string.Join("&", lstExpression);
            }
            else
            {
                url = url + "?" + string.Join("&", lstExpression);
            }
            return url;
        }

        public void Dispose()
        {
            this._client.Dispose();
        }

        /// <summary>
        /// 默认的http返回处理
        /// </summary>
        /// <param name="response"></param>
        /// <returns></returns>
        public static async Task<T> DefaultResponseHandler(HttpResponseMessage response)
        {
            //各种返回的判断逻辑，包括token是否失效            
            var strResult = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {               
                var res = strResult.ToObject<T>();
                return res;
            }
            else
            {
                throw new Exception(strResult);
            }
        }
    }
}
