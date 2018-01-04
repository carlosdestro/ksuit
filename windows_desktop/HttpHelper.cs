using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace windows_desktop
{
    class HttpHelper
    {

        static RestClient _client = null;

        static RestClient client
        {
            get
            {
                if (null == _client)
                {
                    client = new RestClient("http://127.0.0.1:8080/edsa-ksuit/http/");

                    var _cookieJar = new CookieContainer();

                    client.CookieContainer = _cookieJar;
                }

                return _client;
            }

            set
            {
                _client = value;
            }
        }

        static public RestRequestAsyncHandle ExecuteAsync(string operation, string formName, Action<IRestResponse> callback, Dictionary<string, object> data = null)
        {
            var request = new RestRequest("?op=" + operation + "&table=" + formName, Method.POST);

            request.RequestFormat = RestSharp.DataFormat.Json;

            if (null != data)
                foreach (var key in data.Keys)
                    request.AddParameter(key, null == data[key] || string.IsNullOrEmpty(data[key].ToString()) ? string.Empty : data[key]);

            return client.ExecuteAsync(request, callback);
        }
    }
}
