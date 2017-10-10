using System;
using System.Net;
using System.Collections.Generic;

namespace PusherClient
{
    public class HttpAuthorizer: IAuthorizer
    {
        private Uri _authEndpoint;
        private Dictionary<HttpRequestHeader, String> _headers;
        public HttpAuthorizer(string authEndpoint,Dictionary<HttpRequestHeader,String> headers = null)
        {
            _authEndpoint = new Uri(authEndpoint);
            _headers = headers;
        }

        public string Authorize(string channelName, string socketId)
        {
            string authToken = null;

            using (var webClient = new System.Net.WebClient())
            {
                string data = String.Format("channel_name={0}&socket_id={1}", channelName, socketId);
                webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                if (_headers != null)
                {
                    foreach(KeyValuePair<HttpRequestHeader,String> header in _headers){
                        webClient.Headers[header.Key] = header.Value;
                    }
                }
                authToken = webClient.UploadString(_authEndpoint, "POST", data);
            }
            return authToken;
        }
    }
}
