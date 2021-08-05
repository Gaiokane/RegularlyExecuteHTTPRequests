using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Quartz.Impl;
using Quartz;
using Quartz.Logging;
using Quartz.Impl.Matchers;
using Quartz.Impl.Calendar;
using System.Threading;

namespace RegularlyExecuteHTTPRequests
{
    public partial class Form1 : Form
    {
        /*
         * 1.定时执行（执行频率）
         * 2.执行次数
         * 3.支持变量替换等操作
         * 4.自定义请求
         * 5.
         * 6.批量执行（执行间隔时间）
         */

        string[] sqlQuerys;
        static string noMatch = "";

        public Regex rgGetDateTimeAll = new Regex("{{time(d|h|m|s)(\\+|\\-)\\d*:\\d{4}-(0?[1-9]|1[0-2])-((0?[1-9])|((1|2)[0-9])|30|31) (((0|1)[0-9])|(2[0-3])):((0|1|2|3|4|5)[0-9]):((0|1|2|3|4|5)[0-9])}}");//{{time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}取整块 日、小时、分钟、秒
        public Regex rgGetDateTimeDiff = new Regex("(d|h|m|s)(\\+|\\-)\\d*");//{{timed+-7:2020-03-29 20:00:00}}取(d|h|m|s)(+|-)数字
        public Regex rgGetDateTime = new Regex("\\d{4}-(0?[1-9]|1[0-2])-((0?[1-9])|((1|2)[0-9])|30|31) (((0|1)[0-9])|(2[0-3])):((0|1|2|3|4|5)[0-9]):((0|1|2|3|4|5)[0-9])");//{{timed+-7:2020-03-29 20:00:00}}取时间

        public Regex rgGetOnTheHourAll = new Regex("{{onthehour}}");//{{onthehour}}取整块

        public Regex rgGetOnTheHourCustomAll = new Regex("{{onthehour(\\+|\\-)\\d*}}");//{{onthehour(+|-)7}}取整块 小时
        public Regex rgGetOnTheHourCustomDiff = new Regex("(\\+|\\-)\\d*");//{{onthehour+-7}}取(+|-)数字

        //{{onthehour}}
        //{{onthehour+7}}
        private void button4_Click(object sender, EventArgs e)
        {
            noMatch = "";
            int times = 1;
            sqlQuerys = new string[times];
            for (int i = 0; i < times; i++)
            {
                sqlQuerys[i] = richTextBox1.Text.Trim();
            }

            #region 判断是否有匹配{{onthehour}}
            //判断是否有匹配{{onthehour}}
            if (rgGetOnTheHourAll.IsMatch(sqlQuerys[0]))
            {
                getOnTheHour(sqlQuerys);
            }
            else
            {
                noMatch += "没有匹配项{{onthehour}}\n";
            }
            #endregion

            #region 判断是否有匹配{{onthehour(+|-)7}}
            //判断是否有匹配{{onthehour(+|-)7}}
            if (rgGetOnTheHourCustomAll.IsMatch(sqlQuerys[0]))
            {
                GetOnTheHourCustom(sqlQuerys);
            }
            else
            {
                noMatch += "没有匹配项{{onthehour(+|-)7}}\n";
            }
            #endregion

            noMatch += sqlQuerys[0] + "\n";
            richTextBox2.Text += noMatch + "\n";
        }

        #region 将{{onthehour}}替换为当前整点时间
        /// <summary>
        /// 将{{onthehour}}替换为当前整点时间
        /// </summary>
        /// <param name="sourceSQL">原始SQL数组</param>
        /// <returns>替换完的数组</returns>
        public string[] getOnTheHour(string[] sourceSQL)
        {
            //{{onthehour}}

            Match matchOnTheHour;

            while (rgGetOnTheHourAll.Match(sourceSQL[0]).Success == true)
            {
                for (int i = 0; i < sourceSQL.Length; i++)
                {
                    matchOnTheHour = rgGetOnTheHourAll.Match(sourceSQL[i]);//{{onthehour}}取整块

                    string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string nowtimehour = DateTime.Now.Hour.ToString();
                    //MessageBox.Show(nowdate + "\r\n" + nowtimehour);
                    DateTime dt = Convert.ToDateTime(nowdate + " " + nowtimehour + ":00:00");
                    //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss"));

                    sourceSQL[i] = rgGetOnTheHourAll.Replace(sourceSQL[i], dt.ToString("yyyy-MM-dd HH:mm:ss"), 1);
                }
            }

            return sourceSQL;
        }
        #endregion

        #region 将{{onthehour(+|-)7}}替换为当前整点时间并+/-小时
        /// <summary>
        /// 将{{onthehour(+|-)7}}替换为当前整点时间并+/-小时
        /// </summary>
        /// <param name="sourceSQL">原始SQL数组</param>
        /// <returns>替换完的数组</returns>
        public string[] GetOnTheHourCustom(string[] sourceSQL)
        {
            //{{onthehour+-7}}

            Match matchOnTheHourCustomAll;
            Match matchOnTheHourCustomDiff;

            while (rgGetOnTheHourCustomAll.Match(sourceSQL[0]).Success == true)
            {
                for (int i = 0; i < sourceSQL.Length; i++)
                {
                    matchOnTheHourCustomAll = rgGetOnTheHourCustomAll.Match(sourceSQL[i]);//{{onthehour(+|-)7}}取整块 小时
                    matchOnTheHourCustomDiff = rgGetOnTheHourCustomDiff.Match(matchOnTheHourCustomAll.Groups[0].Value);//{{onthehour+-7}}取(+|-)数字

                    string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
                    string nowtimehour = DateTime.Now.Hour.ToString();

                    string str = matchOnTheHourCustomDiff.Groups[0].Value;//取(+|-)数字
                    string symbol = str.Substring(0, 1);//(d|h|m|s)
                    int length = Convert.ToInt32(str.Substring(1, str.Length - 1));//数字

                    DateTime dt = Convert.ToDateTime(nowdate + " " + nowtimehour + ":00:00");

                    if (symbol == "+")//+
                    {
                        //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                        //sourceSQL[i] = sourceSQL[i].Replace(matchOnTheHourCustomAll.Groups[0].Value, dt.AddHours(length).ToString("yyyy-MM-dd HH:mm:ss"));//小时+
                        //用这条，仅替换第一个匹配对象
                        sourceSQL[i] = rgGetOnTheHourCustomAll.Replace(sourceSQL[i], dt.AddHours(length).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                    }
                    if (symbol == "-")//-
                    {
                        //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                        //sourceSQL[i] = sourceSQL[i].Replace(matchOnTheHourCustomAll.Groups[0].Value, dt.AddHours(-length).ToString("yyyy-MM-dd HH:mm:ss"));//小时-
                        //用这条，仅替换第一个匹配对象
                        sourceSQL[i] = rgGetOnTheHourCustomAll.Replace(sourceSQL[i], dt.AddHours(-length).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                    }
                }
            }

            return sourceSQL;
        }
        #endregion

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = "{\"t0\":\"1627660800000000000\",\"t1\":\"1627660800000000000\",\"name\":\"point_audit_1\"}";
            richTextBox1.Text = "{\"t0\":\"{{timed+1:2021-07-08 00:00:00}}\",\"t1\":\"{{timed+1:2021-07-08 00:00:00}}\",\"name\":\"point_audit_1\",\"matcher-sn\":\"sn =~ /^(210)$/\",\"sn\":\"210\"}";
            richTextBox1.Text = "{\"t0\":\"{{timed+1:2020-06-01 00:00:00}}\",\"t1\":\"{{timed+1:2021-07-08 00:00:00}}\",\"name\":\"point_audit_1\"}";
            richTextBox1.Text = "{\"fromTime\":\"{{onthehour-48}}\",\"toTime\":\"{{onthehour-1}}\"}";

            textBox2.Text = "http://192.168.30.73:9002/admin/task";
            textBox2.Text = "http://127.0.0.1/admin/task";
            textBox2.Text = "http://127.0.0.1:9902/api/service";

            textBox1.Text = "1";

            textBox1.Select();
            textBox1.Focus();

            textBox_cron.Text = "0/2 * * * * ?";
            textBox_cron.Text = "0 17 0 * * ?";

            //MessageBox.Show(DateTimeToTstamp(Convert.ToDateTime("2021-07-19 20:09:28")).ToString());
            //MessageBox.Show(TstampToDateTime("1626696568").ToString("yyyy-MM-dd HH:mm:ss"));
            //MessageBox.Show(ConvertJsonString("{\"aaa\":\"123\"}"));

            //RichTextBox增加右键菜单
            RichTextBoxMenu richTextBoxMenu_richTextBox1 = new RichTextBoxMenu(richTextBox1);
            RichTextBoxMenu richTextBoxMenu_richTextBox2 = new RichTextBoxMenu(richTextBox2);

            string nowdate = DateTime.Now.ToString("yyyy-MM-dd");
            string nowtimehour = DateTime.Now.Hour.ToString();
            //MessageBox.Show(nowdate + "\r\n" + nowtimehour);
            DateTime dt = Convert.ToDateTime(nowdate + " " + nowtimehour + ":00:00");
            //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss"));

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string toJson = ConvertJsonString(richTextBox1.Text);

            if (JsonSplit.IsJson(toJson))//传入的json串
            {
                MessageBox.Show("json格式合法");
            }
            else
            {
                MessageBox.Show("json格式不合法");
            }

            MessageBox.Show(toJson);
        }

        private void btn_DELETE_Click(object sender, EventArgs e)
        {
            MessageBox.Show("empty");
        }

        private void btn_PUT_Click(object sender, EventArgs e)
        {
            var obj = new Dictionary<string, object>()
            {
                {"t0", "1627660800000000000"},
                { "t1" , "1627660800000000000"},
                { "name" , "point_audit_1"}
            };


            if (string.IsNullOrEmpty(textBox1.Text.Trim()))
            {
                MessageBox.Show("执行次数不能为空！");
                textBox1.Focus();
            }
            else
            {
                int times = Convert.ToInt32(textBox1.Text.Trim());
                sqlQuerys = new string[times];
                for (int i = 0; i < times; i++)
                {
                    sqlQuerys[i] = richTextBox1.Text.Trim();
                }

                DateTime dt = DateTime.Now;
                string str = "";
                string type = "";
                string symbol = "";
                int length = 0;

                #region 判断是否有匹配{{time(d|h|m|s)(+|-)7:datetime}}
                //判断是否有匹配{{time(d|h|m|s)(+|-)7:datetime}}
                if (rgGetDateTimeAll.IsMatch(sqlQuerys[0]))
                {
                    Match matchDateTimeAll;
                    Match matchDateTimeDiff;
                    Match matchDateTime;
                    matchDateTimeAll = rgGetDateTimeAll.Match(sqlQuerys[0]);//{{time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}取整块 日、小时、分钟、秒
                    matchDateTimeDiff = rgGetDateTimeDiff.Match(matchDateTimeAll.Groups[0].Value);//{{timed+-7:2020-03-29 20:00:00}}取(d|h|m|s)(+|-)数字
                    matchDateTime = rgGetDateTime.Match(matchDateTimeAll.Groups[0].Value);//{{timed+-7:2020-03-29 20:00:00}}取时间
                    dt = Convert.ToDateTime(matchDateTime.Groups[0].Value);
                    str = matchDateTimeDiff.Groups[0].Value;//取(d|h|m|s)(+|-)数字
                    type = str.Substring(0, 1);//(d|h|m|s)
                    symbol = str.Substring(1, 1);//(+|-)
                    length = Convert.ToInt32(str.Substring(2, str.Length - 2));//数字


                    //MessageBox.Show("true");
                    getNewDateTime(sqlQuerys);
                }
                else
                {
                    //MessageBox.Show("没有匹配项{{time(d|h|m|s)(+|-)7:datetime}}");
                    noMatch += "没有匹配项{{time(d|h|m|s)(+|-)7:datetime}}\n";
                }
                #endregion

                #region 判断是否有匹配{{onthehour}}
                //判断是否有匹配{{onthehour}}
                if (rgGetOnTheHourAll.IsMatch(sqlQuerys[0]))
                {
                    getOnTheHour(sqlQuerys);
                }
                else
                {
                    noMatch += "没有匹配项{{onthehour}}\n";
                }
                #endregion

                #region 判断是否有匹配{{onthehour(+|-)7}}
                //判断是否有匹配{{onthehour(+|-)7}}
                if (rgGetOnTheHourCustomAll.IsMatch(sqlQuerys[0]))
                {
                    GetOnTheHourCustom(sqlQuerys);
                }
                else
                {
                    noMatch += "没有匹配项{{onthehour(+|-)7}}\n";
                }
                #endregion

                JavaScriptSerializer s = new JavaScriptSerializer();

                richTextBox2.Text = "";

                for (int i = 0; i < times; i++)
                {
                    Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(ConvertJsonString(sqlQuerys[i]));
                    //Dictionary<string, string> JsonData = (Dictionary<string, string>)JsonConvert.DeserializeObject(ConvertJsonString(richTextBox1.Text));
                    //Dictionary<string, string> JsonData = richTextBox1.Text.Trim().Split(',').ToDictionary(s => s.Split(':')[0].Replace("\"", ""), s => s.Split(':')[1].Replace("\"", ""));
                    //MessageBox.Show(HttpPut("http://192.168.30.73:9002/admin/task", obj));

                    try
                    {
                        //string result = HttpPut("http://192.168.30.73:9002/admin/task", JsonData);
                        string result = HttpPut(textBox2.Text.Trim(), JsonData);
                        if (result != "{\"msg\":\"success\"}")
                        {
                            //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss") + "_" + type + symbol + i + "：" + result);
                            richTextBox2.Text += "\r\n" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "_" + type + symbol + i + "：" + result;
                        }
                    }
                    catch (Exception ex)
                    {
                        //MessageBox.Show(ex.Message);
                        richTextBox2.Text += "\r\n" + ex.Message;
                    }

                    //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss") + "_" + type + symbol + i + "：" + HttpPut("http://192.168.30.73:9002/admin/task", JsonData));
                }
                MessageBox.Show("执行结束");
            }

        }

        public string ConvertJsonString(string str)

        {
            try
            {
                //格式化json字符串
                JsonSerializer serializer = new JsonSerializer();
                TextReader tr = new StringReader(str);
                JsonTextReader jtr = new JsonTextReader(tr);
                object obj = serializer.Deserialize(jtr);
                if (obj != null)
                {
                    StringWriter textWriter = new StringWriter();
                    JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
                    {
                        Formatting = Formatting.Indented,
                        Indentation = 4,
                        IndentChar = ' '
                    };
                    serializer.Serialize(jsonWriter, obj);
                    return textWriter.ToString();
                }
                else
                {
                    return str;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        //url为请求的网址，param参数为需要查询的条件（服务端接收的参数，没有则为null）
        //返回该次请求的响应
        public string HttpGet(string url, Dictionary<String, String> param)
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
                    json.Add(item.Key, item.Value.ToString());
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
            JObject json = new JObject();
            if (param.Count != 0) //将参数添加到json对象中
            {
                foreach (var item in param)
                {
                    json.Add(item.Key, item.Value.ToString());
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

        private void btn_POST_Click(object sender, EventArgs e)
        {
            MessageBox.Show("empty");

            /*WebClient wc = new WebClient();
            string strUrlPara = "{\"t0\":\"1627660800000000000\",\"t1\":\"1627660800000000000\",\"name\":\"point_audit_1\"}";
            strUrlPara = HttpUtility.UrlEncode(strUrlPara);
            byte[] data = new ASCIIEncoding().GetBytes(strUrlPara);
            byte[] responseArray = wc.UploadData("http://192.168.30.73:9002/admin/task", data);
            var response = Encoding.UTF8.GetString(responseArray);
            //return response;
            MessageBox.Show(response);*/
        }

        private void btn_GET_Click(object sender, EventArgs e)
        {
            MessageBox.Show(HttpGet(textBox2.Text.Trim(), null));
            //MessageBox.Show(HTTPHelper.Get("https://www.baidu.com/s?ie=utf-8&mod=1&isbd=1&isid=BB39D62DFB618099&ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd=RestClient&oq=Rest%2526lt%253Blient&rsv_pq=b9fbf0f70005442f&rsv_t=c488k%2BX1FLp26DkB2hECfqAkSnV4FLK9E7FdQKb3VoI%2Bwrw66RSWTKo9%2BSY&rqlang=cn&rsv_enter=0&rsv_dl=tb&rsv_btype=t&bs=RestClient&rsv_sid=undefined&_ss=1&clist=&hsug=&f4s=1&csor=0&_cr1=26925"));
        }

        // 时间戳转为C#格式时间
        private DateTime TstampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);

            return dateTimeStart.Add(toNow);
        }

        // DateTime时间格式转换为Unix时间戳格式
        private long DateTimeToTstamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            long lTime = long.Parse((int)(time - startTime).TotalSeconds + "000000000");
            return lTime;
            //return (int)(time - startTime).TotalSeconds;
        }

        #region 将time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}指定时间并递增
        /// <summary>
        /// 将time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}指定时间并递增
        /// </summary>
        /// <param name="sourceSQL">原始SQL数组</param>
        /// <returns>替换完的数组</returns>
        private string[] getNewDateTime(string[] sourceSQL)
        {
            //{{timed+777:2020-04-04 11:47:07}}

            Match matchDateTimeAll;
            Match matchDateTimeDiff;
            Match matchDateTime;

            while (rgGetDateTimeAll.Match(sourceSQL[0]).Success == true)
            {
                for (int i = 0; i < sourceSQL.Length; i++)
                {
                    matchDateTimeAll = rgGetDateTimeAll.Match(sourceSQL[i]);//{{time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}取整块 日、小时、分钟、秒
                    //matchDateTimeDiff = rgGetDateTimeDiff.Match(sourceSQL[i]);//{{timed+-7:2020-03-29 20:00:00}}取(d|h|m|s)(+|-)数字
                    matchDateTimeDiff = rgGetDateTimeDiff.Match(matchDateTimeAll.Groups[0].Value);//{{timed+-7:2020-03-29 20:00:00}}取(d|h|m|s)(+|-)数字
                    //matchDateTime = rgGetDateTime.Match(sourceSQL[i]);//{{timed+-7:2020-03-29 20:00:00}}取时间
                    matchDateTime = rgGetDateTime.Match(matchDateTimeAll.Groups[0].Value);//{{timed+-7:2020-03-29 20:00:00}}取时间
                    //DateTime dt = new DateTime();
                    DateTime dt = Convert.ToDateTime(matchDateTime.Groups[0].Value);
                    string str = matchDateTimeDiff.Groups[0].Value;//取(d|h|m|s)(+|-)数字
                    string type = str.Substring(0, 1);//(d|h|m|s)
                    string symbol = str.Substring(1, 1);//(+|-)
                    int length = Convert.ToInt32(str.Substring(2, str.Length - 2));//数字

                    //MessageBox.Show(type+"\n"+symbol+"\n"+length.ToString());
                    //MessageBox.Show(dt.AddHours(7).ToString("yyyy-MM-dd HH:mm:ss"));

                    if (type == "d")//日+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddDays(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//日+
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddDays(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddDays(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//日-
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddDays(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                    }
                    if (type == "h")//小时+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddHours(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//小时+
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddHours(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddHours(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//小时-
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddHours(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                    }
                    if (type == "m")//分钟+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddMinutes(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//分钟+
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddMinutes(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddMinutes(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//分钟-
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddMinutes(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                    }
                    if (type == "s")//秒+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddSeconds(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//秒+
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddSeconds(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddSeconds(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//秒-
                            //用这条，仅替换第一个匹配对象
                            sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddSeconds(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString(), 1);
                        }
                    }
                }
            }

            return sourceSQL;
        }
        #endregion

        private async void button2_Click(object sender, EventArgs e)
        {
            //创建调度单元
            Task<IScheduler> tsk = StdSchedulerFactory.GetDefaultScheduler();
            IScheduler scheduler = tsk.Result;
            //2.创建一个具体的作业即job (具体的job需要单独在一个文件中执行)
            IJobDetail job = JobBuilder.Create<SendMessageJob>().WithIdentity("完成").Build();
            //3.创建并配置一个触发器即trigger   1s执行一次
            ITrigger _CronTrigger = TriggerBuilder.Create()
              .WithIdentity("定时确认")
              //.WithCronSchedule("0/2 * * * * ?") //秒 分 时 某一天 月 周 年(可选参数)
              .WithCronSchedule(textBox_cron.Text.Trim()) //秒 分 时 某一天 月 周 年(可选参数)
              .Build()
              as ITrigger;

            //MessageBox.Show(textBox2.Text.Trim());
            //MessageBox.Show(richTextBox1.Text.Trim());

            // 设置初始参数
            job.JobDataMap.Put(SendMessageJob.url, textBox2.Text.Trim());
            job.JobDataMap.Put(SendMessageJob.body, richTextBox1.Text.Trim());

            //4.将job和trigger加入到作业调度池中
            scheduler.ScheduleJob(job, _CronTrigger);
            //5.开启调度
            scheduler.Start();
            //Console.ReadLine();
            richTextBox2.Text += "\r\n定时开启";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Task<IScheduler> tsk = StdSchedulerFactory.GetDefaultScheduler();
            IScheduler scheduler = tsk.Result;
            scheduler.Shutdown();
            richTextBox2.Text += "\r\n定时结束";
        }

        public void printlog()
        {
            richTextBox2.Text += "\r\n执行a";
        }
        public virtual void JobWasExecuted(IJobExecutionContext inContext, JobExecutionException inException)
        {
            JobKey jobKey = inContext.JobDetail.Key;
            // 获取传递过来的参数
            JobDataMap data = inContext.JobDetail.JobDataMap;
            //获取回传的数据库表条目数
            string result = data.GetString(SendMessageJob.backresult);

            try
            {
                richTextBox2.Text += result;
            }
            catch (SchedulerException e)
            {

                Console.Error.WriteLine(e.StackTrace);
            }
        }
    }
    public class SendMessageJob : IJob
    {
        public const string url = "url";
        public const string body = "body";
        public const string backresult = "backresult";

        /// <summary>
        /// 创建要执行的作业
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;
            string Url = data.GetString(url);
            string Body = data.GetString(body);

            await Task.Run(() =>
            {
                //Console.WriteLine("你好啊！");
                //richTextBox2.Text += "你好啊！";

                //MessageBox.Show("你好啊！");
                //MessageBox.Show(HTTPHelper.HttpGet(Url, null));
                //data.Put(backresult, "执行b");

                if (string.IsNullOrEmpty(Body))
                {
                    MessageBox.Show("body不能为空");
                }
                else
                {
                    string[] sqlQuerys = new string[1];
                    sqlQuerys[0] = Body;
                    string noMatch = "";

                    Form1 f1 = new Form1();

                    #region 判断是否有匹配{{onthehour}}
                    //判断是否有匹配{{onthehour}}
                    if (f1.rgGetOnTheHourAll.IsMatch(sqlQuerys[0]))
                    {
                        f1.getOnTheHour(sqlQuerys);
                    }
                    else
                    {
                        noMatch += "没有匹配项{{onthehour}}\n";
                    }
                    #endregion

                    #region 判断是否有匹配{{onthehour(+|-)7}}
                    //判断是否有匹配{{onthehour(+|-)7}}
                    if (f1.rgGetOnTheHourCustomAll.IsMatch(sqlQuerys[0]))
                    {
                        f1.GetOnTheHourCustom(sqlQuerys);
                    }
                    else
                    {
                        noMatch += "没有匹配项{{onthehour(+|-)7}}\n";
                    }
                    #endregion

                    string[] strlines = noMatch.Split(new string[] { "\n" }, StringSplitOptions.None);
                    int linescount = strlines.Count() - 1;
                    //MessageBox.Show((strlines.Count() - 1).ToString());

                    if (linescount > 1)
                    {
                        MessageBox.Show(noMatch + "\n" + "取消操作");
                    }
                    else
                    {
                        JavaScriptSerializer s = new JavaScriptSerializer();
                        Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(sqlQuerys[0]));

                        //MessageBox.Show(Url + "\n" + Body + "\n" + noMatch + "\n" + sqlQuerys[0] + "\n" + JsonData.Values);

                        //string result = HTTPHelper.HttpPost(Url, JsonData);
                        string result = HTTPHelper.HttpPost(Url, JsonData);
                        MessageBox.Show(result);
                    }
                }
            });
        }
    }
}