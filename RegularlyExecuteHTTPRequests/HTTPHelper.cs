using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RegularlyExecuteHTTPRequests
{
    class HTTPHelper
    {
        public static string HttpGet(string url, Dictionary<String, String> param)
        {
            if (param != null) //有参数的情况下，拼接url
            {
                url = url + "?";
                foreach (var item in param)
                {
                    url = url + item.Key + "=" + item.Value + "&";
                }
                url = url.Substring(0, url.Length - 1);
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;//创建请求
            request.Method = "GET"; //请求方法为GET
            HttpWebResponse res; //定义返回的response
            try
            {
                res = (HttpWebResponse)request.GetResponse(); //此处发送了请求并获得响应
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //响应转化为String字符串
            return content;
        }

        public static string HttpPut(string url, Dictionary<String, object> param)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            //request.AllowReadStreamBuffering = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "Put"; //请求方式为post
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.ContentType = "application/json";
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    if (item.Value.GetType() == typeof(Int32))
                    {
                        json.Add(item.Key, Convert.ToInt32(item.Value.ToString()));
                    }
                    else
                    {
                        json.Add(item.Key, item.Value.ToString());
                    }
                }
            }
            string jsonstring = json.ToString();//获得参数的json字符串
            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonstring);
            Stream postStream = request.GetRequestStream();
            postStream.Write(jsonbyte, 0, jsonbyte.Length);
            postStream.Close();
            //发送请求并获取相应回应数据       
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //获得响应字符串
            return content;
        }

        public static string HttpPost(string url, Dictionary<String, object> param)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            //request.AllowReadStreamBuffering = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "POST"; //请求方式为post
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.ContentType = "application/json";
            //request.Headers.Add("Authorization", "eyJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJzdXBlcmFkbWluIiwiaXNzIjoiZnJhbWUiLCJ1aWkiOjEsInVpZCI6ImVkN2NhMGM4OTg4YTQ1ZDViNmEzMWE3ZjRhM2E1ZjY2IiwidW4iOiLotoXnuqfnrqHnkIblkZgiLCJjcGkiOiIyNDdmNWE4MTkxZWU0YjU3YmM4YWU1M2QzMWNmZjU0NSIsInVkaSI6ImVkN2ExZDQ5Zjc3MDQzNTRhODgyNTY1ZWQwYjA2NmY1IiwianRpIjoic3VwZXJhZG1pbi0xNjI4NjkwODkwIiwiZXhwIjoxNjI4NjkwODkwfQ.YCpcFyfOR2-5g6KD2JTTfJujGJkT5QI-7TiiO56urGCWXY6n3PCdvPpcrtlnrBR6n7qU5fuSk1z3IlcYn6HmqiSkOz5vx-MDEvFhbDKeUaCn7LebST3uL0wPfVHUBM-o51D88C7U6EmUQJ7T518iZQMlUEdGM4ElDh7kv-DF_d8");
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    if (item.Value.GetType() == typeof(Int32))
                    {
                        json.Add(item.Key, Convert.ToInt32(item.Value.ToString()));
                    }
                    else
                    {
                        json.Add(item.Key, item.Value.ToString());
                    }
                }
            }
            string jsonstring = json.ToString();//获得参数的json字符串
            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonstring);
            Stream postStream = request.GetRequestStream();
            postStream.Write(jsonbyte, 0, jsonbyte.Length);
            postStream.Close();
            //发送请求并获取相应回应数据       
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //获得响应字符串
            return content;
        }

        public static string HttpPostArray(string url, Dictionary<String, object> param)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            //request.AllowReadStreamBuffering = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "POST"; //请求方式为post
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.ContentType = "application/json";
            //request.Headers.Add("Authorization", "eyJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJzdXBlcmFkbWluIiwiaXNzIjoiZnJhbWUiLCJ1aWkiOjEsInVpZCI6ImVkN2NhMGM4OTg4YTQ1ZDViNmEzMWE3ZjRhM2E1ZjY2IiwidW4iOiLotoXnuqfnrqHnkIblkZgiLCJjcGkiOiIyNDdmNWE4MTkxZWU0YjU3YmM4YWU1M2QzMWNmZjU0NSIsInVkaSI6ImVkN2ExZDQ5Zjc3MDQzNTRhODgyNTY1ZWQwYjA2NmY1IiwianRpIjoic3VwZXJhZG1pbi0xNjI4NjkwODkwIiwiZXhwIjoxNjI4NjkwODkwfQ.YCpcFyfOR2-5g6KD2JTTfJujGJkT5QI-7TiiO56urGCWXY6n3PCdvPpcrtlnrBR6n7qU5fuSk1z3IlcYn6HmqiSkOz5vx-MDEvFhbDKeUaCn7LebST3uL0wPfVHUBM-o51D88C7U6EmUQJ7T518iZQMlUEdGM4ElDh7kv-DF_d8");
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    if (item.Value.GetType() == typeof(Int32))
                    {
                        json.Add(item.Key, Convert.ToInt32(item.Value.ToString()));
                    }
                    else
                    {
                        json.Add(item.Key, item.Value.ToString());
                    }
                }
            }
            JArray arr = new JArray();
            arr.Add(json);


            string jsonstring = json.ToString();//获得参数的json字符串
            jsonstring = arr.ToString();//获得参数的json字符串
            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonstring);
            Stream postStream = request.GetRequestStream();
            postStream.Write(jsonbyte, 0, jsonbyte.Length);
            postStream.Close();
            //发送请求并获取相应回应数据       
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //获得响应字符串
            return content;
        }

        public static string HttpPostAuthorization(string url, Dictionary<String, object> param, string Authorization)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            //request.AllowReadStreamBuffering = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "POST"; //请求方式为post
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", Authorization);
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    Type itemType = item.Value.GetType();
                    if (itemType == typeof(Int32))
                    {
                        json.Add(item.Key, Convert.ToInt32(item.Value.ToString()));
                    }
                    // 如果值为[{},{}]
                    else if (itemType == typeof(Object[]))
                    {
                        if (item.Value is IEnumerable<object> enumerable) // 检查是否为可枚举类型
                        {
                            var jArray = new JArray();
                            foreach (var obj in enumerable)
                            {
                                if (obj is IDictionary<string, object> dict) // 检查是否为字典类型
                                {
                                    var jObject = new JObject();
                                    foreach (var kvp in dict)
                                    {
                                        jObject.Add(kvp.Key, JToken.FromObject(kvp.Value));
                                    }
                                    jArray.Add(jObject);
                                }
                            }
                            json.Add(item.Key, jArray);
                        }
                    }
                    else
                    {
                        json.Add(item.Key, item.Value.ToString());
                    }
                }
            }
            string jsonstring = json.ToString();//获得参数的json字符串
            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonstring);
            Stream postStream = request.GetRequestStream();
            postStream.Write(jsonbyte, 0, jsonbyte.Length);
            postStream.Close();
            //发送请求并获取相应回应数据       
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //获得响应字符串
            return content;
        }

        public static string HttpPostAuthorizationArray(string url, Dictionary<String, object> param, string Authorization)
        {
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest; //创建请求
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            //request.AllowReadStreamBuffering = true;
            request.MaximumResponseHeadersLength = 1024;
            request.Method = "POST"; //请求方式为post
            request.AllowAutoRedirect = true;
            request.MaximumResponseHeadersLength = 1024;
            request.ContentType = "application/json";
            request.Headers.Add("Authorization", Authorization);
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    if (item.Value.GetType() == typeof(Int32))
                    {
                        json.Add(item.Key, Convert.ToInt32(item.Value.ToString()));
                    }
                    else
                    {
                        json.Add(item.Key, item.Value.ToString());
                    }
                }
            }
            JArray arr = new JArray();
            arr.Add(json);

            string jsonstring = json.ToString();//获得参数的json字符串
            jsonstring = arr.ToString();//获得参数的json字符串
            byte[] jsonbyte = Encoding.UTF8.GetBytes(jsonstring);
            Stream postStream = request.GetRequestStream();
            postStream.Write(jsonbyte, 0, jsonbyte.Length);
            postStream.Close();
            //发送请求并获取相应回应数据       
            HttpWebResponse res;
            try
            {
                res = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException ex)
            {
                res = (HttpWebResponse)ex.Response;
            }
            StreamReader sr = new StreamReader(res.GetResponseStream(), Encoding.UTF8);
            string content = sr.ReadToEnd(); //获得响应字符串
            return content;
        }
    }
}