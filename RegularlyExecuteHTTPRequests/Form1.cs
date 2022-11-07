using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Web.Script.Serialization;
using System.Text.RegularExpressions;
using Quartz.Impl;
using Quartz;
using Quartz.Impl.Matchers;
using System.Threading;
using System.Drawing;

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

        public static Form1 form1 = null;

        //{{onthehour}}
        //{{onthehour+7}}

        private void button4_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string str = "";
            string type = "";
            string symbol = "";
            int length = 0;

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
            form1 = this;
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

            button3.Enabled = false;

            txtbox_WaitingTime.Enabled = false;

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

            //配置文件读取默认配置
            ConfigSettings.getConfigSettings();
            ConfigSettings.setDefaultConfigSettings();
            ConfigSettings.getConfigSettings();
            DefaultConfigSettingsFill();

            //label8.Text = "数据同步中，请勿关闭！";
            label8.Visible = false;
            //label放在panel中，配合以下两参数超长自动换行
            label8.Dock = DockStyle.Fill;
            label8.AutoSize = false;
            label8.Font = new Font("宋体", Convert.ToInt32(ConfigSettings.close_warning_fontsize), FontStyle.Bold);
            label8.ForeColor = Color.Red;

            //panel2、label9.Text = "数据同步中，请勿关闭！";
            panel2.Visible = false;
            //label9.Text = "数据同步中，请勿关闭！";
            //label9.Visible = false;
            //label放在panel中，配合以下两参数超长自动换行
            label9.Dock = DockStyle.Fill;
            label9.AutoSize = false;
            label9.Font = new Font("宋体", Convert.ToInt32(ConfigSettings.mid_close_warning_fontsize), FontStyle.Bold);
            label9.ForeColor = Color.Red;

            this.Icon = Properties.Resources.ac0nh_hcz4m_001;
        }

        private void DefaultConfigSettingsFill()
        {
            textBox2.Text = ConfigSettings.default_url;
            richTextBox1.Text = ConfigSettings.default_json;
            textBox_cron.Text = ConfigSettings.default_cron;
            string configIsAuthorization = ConfigSettings.isAuthorization;
            if (configIsAuthorization == "true")
            {
                chkbox_Authorization.Checked = true;
            }
            else
            {
                chkbox_Authorization.Checked = false;
            }
            textbox_uid.Text = ConfigSettings.login_uid;
            textbox_pid.Text = ConfigSettings.login_pid;
            textBox_loginurl.Text = ConfigSettings.login_url;
            this.Text = ConfigSettings.form_text;

            if (ConfigSettings.control_enable == "true")
            {
                textBox1.Enabled = true;
                btn_DELETE.Enabled = true;
                btn_PUT.Enabled = true;
                btn_POST.Enabled = true;
                btn_GET.Enabled = true;
            }
            else
            {
                textBox1.Enabled = false;
                btn_DELETE.Enabled = false;
                btn_PUT.Enabled = false;
                btn_POST.Enabled = false;
                btn_GET.Enabled = false;
            }

            label8.Text = ConfigSettings.close_warning;
            label9.Text = ConfigSettings.mid_close_warning;
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

        private string[] GenBody(int times, string sourceBody)
        {
            string[] sqlQuerys;
            if (string.IsNullOrEmpty(times.ToString()))
            {
                MessageBox.Show("执行次数不能为空！");
                textBox1.Focus();
                sqlQuerys = new string[1];
                return sqlQuerys;
            }
            else
            {
                sqlQuerys = new string[times];
                for (int i = 0; i < times; i++)
                {
                    sqlQuerys[i] = sourceBody.Trim();
                }

                DateTime dt = DateTime.Now;
                string str = "";
                string type = "";
                string symbol = "";
                int length = 0;

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

                return sqlQuerys;
            }
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

                JavaScriptSerializer js = new JavaScriptSerializer();

                richTextBox2.Text = "";
                if (chkbox_WaitingTime.Checked == true && string.IsNullOrEmpty(txtbox_WaitingTime.Text.Trim()))
                {
                    MessageBox.Show("勾选等待时间后，不能为空");
                }
                else
                {

                    for (int i = 0; i < times; i++)
                    {
                        Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(sqlQuerys[i]));
                        //Dictionary<string, string> JsonData = (Dictionary<string, string>)JsonConvert.DeserializeObject(ConvertJsonString(richTextBox1.Text));
                        //Dictionary<string, string> JsonData = richTextBox1.Text.Trim().Split(',').ToDictionary(s => s.Split(':')[0].Replace("\"", ""), s => s.Split(':')[1].Replace("\"", ""));
                        //MessageBox.Show(HttpPut("http://192.168.30.73:9002/admin/task", obj));

                        try
                        {
                            DateTime execStart = DateTime.Now;

                            //string result = HttpPut("http://192.168.30.73:9002/admin/task", JsonData);
                            string result = HttpPut(textBox2.Text.Trim(), JsonData);

                            DateTime execEnd = DateTime.Now;
                            TimeSpan ts = execEnd - execStart;

                            string tempResult = "";

                            if (result != "{\"msg\":\"success\"}")
                            {
                                //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss") + "_" + type + symbol + i + "：" + result);
                                //richTextBox2.Text += "\r\n" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "_" + type + symbol + i + "：" + result;
                                tempResult = "失败";

                            }
                            else
                            {
                                tempResult = "成功";
                            }

                            string execresult = tempResult + "\r\n" + "####################第 " + (i + 1) + " 次执行####################\r\n\r\n" +
                                "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                "-->请求参数：\r\n" + sqlQuerys[i] + "\r\n\r\n" +
                                //"执行结果：\r\n" + result;
                                //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                "<--执行结果：\r\n" + result + "\r\n\r\n";
                            richTextBox2.Text = richTextBox2.Text.Insert(0, execresult);
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(ex.Message);
                            richTextBox2.Text = richTextBox2.Text.Insert(0, "\r\n" + ex.Message);
                        }

                        //MessageBox.Show(dt.ToString("yyyy-MM-dd HH:mm:ss") + "_" + type + symbol + i + "：" + HttpPut("http://192.168.30.73:9002/admin/task", JsonData));

                        if (i + 1 < times && chkbox_WaitingTime.Checked == true)
                        {
                            int waitingTime = Convert.ToInt32(txtbox_WaitingTime.Text.Trim());
                            richTextBox2.Text = richTextBox2.Text.Insert(0, "已配置等待时间，下次执行在" + DateTime.Now.AddSeconds(waitingTime).ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                            Delay(waitingTime * 1000);
                        }
                    }
                    MessageBox.Show("执行结束");
                }
            }

        }
        //休眠指定时间，解决Thread.Sleep导致界面卡死问题
        public static void Delay(int mm)
        {
            DateTime current = DateTime.Now;
            while (current.AddMilliseconds(mm) > DateTime.Now)
            {
                Application.DoEvents();
            }
            return;
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
            richTextBox2.Text = "";
            int times = Convert.ToInt32(textBox1.Text.Trim());
            string[] querys = GenBody(times, richTextBox1.Text.Trim());

            if (chkbox_WaitingTime.Checked == true && string.IsNullOrEmpty(txtbox_WaitingTime.Text.Trim()))
            {
                MessageBox.Show("勾选等待时间后，不能为空");
            }
            else
            {
                if (chkbox_Authorization.Checked == true && (string.IsNullOrEmpty(textbox_uid.Text) || string.IsNullOrEmpty(textbox_pid.Text) || string.IsNullOrEmpty(textBox_loginurl.Text)))
                {
                    MessageBox.Show("uid、pid、login_url不能为空");
                }
                else if (chkbox_Authorization.Checked == true && string.IsNullOrEmpty(textbox_uid.Text) == false && string.IsNullOrEmpty(textbox_pid.Text) == false)
                {
                    //MessageBox.Show("empty");

                    /*WebClient wc = new WebClient();
                    string strUrlPara = "{\"t0\":\"1627660800000000000\",\"t1\":\"1627660800000000000\",\"name\":\"point_audit_1\"}";
                    strUrlPara = HttpUtility.UrlEncode(strUrlPara);
                    byte[] data = new ASCIIEncoding().GetBytes(strUrlPara);
                    byte[] responseArray = wc.UploadData("http://192.168.30.73:9002/admin/task", data);
                    var response = Encoding.UTF8.GetString(responseArray);
                    //return response;
                    MessageBox.Show(response);*/

                    //string json = "{\"token\":\"eyJhbGciOiJSUzI1NiJ9.eyJzdWIiOiJzdXBlcmFkbWluIiwiaXNzIjoiZnJhbWUiLCJ1aWkiOjEsInVpZCI6ImVkN2NhMGM4OTg4YTQ1ZDViNmEzMWE3ZjRhM2E1ZjY2IiwidW4iOiLotoXnuqfnrqHnkIblkZgiLCJjcGkiOiIyNDdmNWE4MTkxZWU0YjU3YmM4YWU1M2QzMWNmZjU0NSIsInVkaSI6ImVkN2ExZDQ5Zjc3MDQzNTRhODgyNTY1ZWQwYjA2NmY1IiwianRpIjoic3VwZXJhZG1pbi0xNjI4NjkwODkwIiwiZXhwIjoxNjI4NjkwODkwfQ.YCpcFyfOR2-5g6KD2JTTfJujGJkT5QI-7TiiO56urGCWXY6n3PCdvPpcrtlnrBR6n7qU5fuSk1z3IlcYn6HmqiSkOz5vx-MDEvFhbDKeUaCn7LebST3uL0wPfVHUBM-o51D88C7U6EmUQJ7T518iZQMlUEdGM4ElDh7kv-DF_d8\"}";
                    //string jsontoken = GetJsonValue(json, "token")[0].ToString();
                    //MessageBox.Show(jsontoken);

                    try
                    {
                        JavaScriptSerializer tokenjs = new JavaScriptSerializer();
                        string loginjson = "{\"uid\":\"" + textbox_uid.Text.Trim() + "\",\"pid\":\"" + textbox_pid.Text.Trim() + "\"}";
                        Dictionary<string, object> tokenJsonData = (Dictionary<string, object>)tokenjs.DeserializeObject(ConvertJsonString(loginjson));
                        string tokenresult = HTTPHelper.HttpPost(textBox_loginurl.Text.Trim(), tokenJsonData);
                        string token = GetJsonValue(tokenresult, "token")[0].ToString();


                        JavaScriptSerializer js = new JavaScriptSerializer();

                        #region 批量执行改造前备份
                        //Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(richTextBox1.Text.Trim()));
                        //以下四行在 批量执行改造前备份 前就注释了
                        /*foreach (var item in JsonData)
                        {
                            MessageBox.Show(item.ToString());
                        }*/
                        //string result = HTTPHelper.HttpPostAuthorization(textBox2.Text.Trim(), JsonData, token);
                        //MessageBox.Show(result); 
                        #endregion

                        #region 批量执行改造后
                        int index = 0;
                        if (chkbox_JSONArray.Checked == true)
                        {
                            foreach (var item in querys)
                            {
                                DateTime execStart = DateTime.Now;

                                string strBody = item.Trim();
                                if (strBody.Substring(0, 1).Contains('['))
                                {
                                    StringBuilder builder = new StringBuilder(strBody);
                                    builder.Replace("[", "", 0, 1);
                                    strBody = builder.ToString();
                                }
                                Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(strBody.Trim()));
                                string result = HTTPHelper.HttpPostAuthorizationArray(textBox2.Text.Trim(), JsonData, token);
                                //richTextBox2.Text = richTextBox2.Text.Insert(0, "-->请求：\r\n" + item + "\r\n" + "<--响应：\r\n" + result + "\r\n\r\n");

                                DateTime execEnd = DateTime.Now;
                                TimeSpan ts = execEnd - execStart;

                                string execresult = "####################第 " + (index + 1) + " 次执行####################\r\n\r\n" +
                                    "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                    "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                    "-->请求参数：\r\n" + item + "\r\n\r\n" +
                                    //"执行结果：\r\n" + result;
                                    //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                    "<--执行结果：\r\n" + result + "\r\n\r\n";
                                richTextBox2.Text = richTextBox2.Text.Insert(0, execresult);

                                if (index + 1 < querys.Length && chkbox_WaitingTime.Checked == true)
                                {
                                    int waitingTime = Convert.ToInt32(txtbox_WaitingTime.Text.Trim());
                                    richTextBox2.Text = richTextBox2.Text.Insert(0, "已配置等待时间，下次执行在" + DateTime.Now.AddSeconds(waitingTime).ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                                    Delay(waitingTime * 1000);
                                }
                                index++;
                            }
                        }
                        else
                        {
                            foreach (var item in querys)
                            {
                                DateTime execStart = DateTime.Now;

                                Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(item.Trim()));
                                string result = HTTPHelper.HttpPostAuthorization(textBox2.Text.Trim(), JsonData, token);
                                //richTextBox2.Text = richTextBox2.Text.Insert(0, "-->请求：\r\n" + item + "\r\n" + "<--响应：\r\n" + result + "\r\n\r\n");

                                DateTime execEnd = DateTime.Now;
                                TimeSpan ts = execEnd - execStart;

                                string execresult = "####################第 " + (index + 1) + " 次执行####################\r\n\r\n" +
                                    "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                    "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                    "-->请求参数：\r\n" + item + "\r\n\r\n" +
                                    //"执行结果：\r\n" + result;
                                    //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                    "<--执行结果：\r\n" + result + "\r\n\r\n";
                                richTextBox2.Text = richTextBox2.Text.Insert(0, execresult);

                                if (index + 1 < querys.Length && chkbox_WaitingTime.Checked == true)
                                {
                                    int waitingTime = Convert.ToInt32(txtbox_WaitingTime.Text.Trim());
                                    richTextBox2.Text = richTextBox2.Text.Insert(0, "已配置等待时间，下次执行在" + DateTime.Now.AddSeconds(waitingTime).ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                                    Delay(waitingTime * 1000);
                                }
                                index++;
                            }
                        }
                        #endregion

                        //Clipboard.SetText(result);
                        //string token = GetJsonValue(result, "token")[0].ToString();
                        //MessageBox.Show(token);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else if (chkbox_Authorization.Checked == false)
                {
                    try
                    {
                        JavaScriptSerializer js = new JavaScriptSerializer();

                        #region 批量执行改造前备份
                        //Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(richTextBox1.Text.Trim()));
                        //以下四行在 批量执行改造前备份 前就注释了
                        //foreach (var item in JsonData)
                        //{
                        //    MessageBox.Show("Key：" + item.Key + "\r\nValue：" + item.Value.ToString() + "\r\nValueFormat：" + item.Value.GetType());
                        //}
                        //string result = HTTPHelper.HttpPost(textBox2.Text.Trim(), JsonData);
                        //MessageBox.Show(result);
                        #endregion

                        #region 批量执行改造后
                        int index = 0;
                        if (chkbox_JSONArray.Checked == true)
                        {
                            foreach (var item in querys)
                            {
                                DateTime execStart = DateTime.Now;

                                string strBody = item.Trim();
                                if (strBody.Substring(0, 1).Contains('['))
                                {
                                    StringBuilder builder = new StringBuilder(strBody);
                                    builder.Replace("[", "", 0, 1);
                                    strBody = builder.ToString();
                                }
                                Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(strBody.Trim()));
                                string result = HTTPHelper.HttpPostArray(textBox2.Text.Trim(), JsonData);
                                //richTextBox2.Text = richTextBox2.Text.Insert(0, "-->请求：\r\n" + item + "\r\n" + "<--响应：\r\n" + result + "\r\n\r\n");

                                DateTime execEnd = DateTime.Now;
                                TimeSpan ts = execEnd - execStart;

                                string execresult = "####################第 " + (index + 1) + " 次执行####################\r\n\r\n" +
                                    "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                    "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                    "-->请求参数：\r\n" + item + "\r\n\r\n" +
                                    //"执行结果：\r\n" + result;
                                    //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                    "<--执行结果：\r\n" + result + "\r\n\r\n";
                                richTextBox2.Text = richTextBox2.Text.Insert(0, execresult);

                                if (index + 1 < querys.Length && chkbox_WaitingTime.Checked == true)
                                {
                                    int waitingTime = Convert.ToInt32(txtbox_WaitingTime.Text.Trim());
                                    richTextBox2.Text = richTextBox2.Text.Insert(0, "已配置等待时间，下次执行在" + DateTime.Now.AddSeconds(waitingTime).ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                                    Delay(waitingTime * 1000);
                                }
                                index++;
                            }
                        }
                        else
                        {
                            foreach (var item in querys)
                            {
                                DateTime execStart = DateTime.Now;

                                Dictionary<string, object> JsonData = (Dictionary<string, object>)js.DeserializeObject(ConvertJsonString(item.Trim()));
                                string result = HTTPHelper.HttpPost(textBox2.Text.Trim(), JsonData);
                                //richTextBox2.Text = richTextBox2.Text.Insert(0, "-->请求：\r\n" + item + "\r\n" + "<--响应：\r\n" + result + "\r\n\r\n");

                                DateTime execEnd = DateTime.Now;
                                TimeSpan ts = execEnd - execStart;

                                string execresult = "####################第 " + (index + 1) + " 次执行####################\r\n\r\n" +
                                    "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                    "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                    "-->请求参数：\r\n" + item + "\r\n\r\n" +
                                    //"执行结果：\r\n" + result;
                                    //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                    "<--执行结果：\r\n" + result + "\r\n\r\n";
                                richTextBox2.Text = richTextBox2.Text.Insert(0, execresult);

                                if (index + 1 < querys.Length && chkbox_WaitingTime.Checked == true)
                                {
                                    int waitingTime = Convert.ToInt32(txtbox_WaitingTime.Text.Trim());
                                    richTextBox2.Text = richTextBox2.Text.Insert(0, "已配置等待时间，下次执行在" + DateTime.Now.AddSeconds(waitingTime).ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                                    Delay(waitingTime * 1000);
                                }
                                index++;
                            }
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("ERROR");
                }
            }
        }

        /// <summary>
        /// 获取JSON字符串中指定KEY的值
        /// </summary>
        /// <param name="jsonString"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<String> GetJsonValue(String jsonString, String key)
        {
            String pattern = $"\"{key}\":\"(.*?)\\\"";
            MatchCollection matches = Regex.Matches(jsonString, pattern, RegexOptions.IgnoreCase);
            List<string> lst = new List<string>();
            foreach (Match m in matches)
            {
                lst.Add(m.Groups[1].Value);
            }

            return lst;
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
        public string[] getNewDateTime(string[] sourceSQL)
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
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddDays(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddDays(length * i).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddDays(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//日-
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddDays(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddDays(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                    }
                    if (type == "h")//小时+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddHours(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//小时+
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddHours(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddHours(length * i).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddHours(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//小时-
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddHours(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddHours(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                    }
                    if (type == "m")//分钟+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddMinutes(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//分钟+
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddMinutes(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddMinutes(length * i).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddMinutes(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//分钟-
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddMinutes(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddMinutes(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                    }
                    if (type == "s")//秒+/-
                    {
                        if (symbol == "+")//+
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddSeconds(length * i).ToString("yyyy-MM-dd HH:mm:ss"));//秒+
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddSeconds(length * i).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddSeconds(length * i).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                        if (symbol == "-")//-
                        {
                            //用这条，替换的时候 如果有相同匹配对象，会全部替换成同一个值
                            //sourceSQL[i] = sourceSQL[i].Replace(matchDateTimeAll.Groups[0].Value, dt.AddSeconds(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"));//秒-
                            //用这条，仅替换第一个匹配对象
                            if (chkbox_Tstamp.Checked == true)
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], DateTimeToTstamp(Convert.ToDateTime(dt.AddSeconds(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss"))).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                            else
                            {
                                sourceSQL[i] = rgGetDateTimeAll.Replace(sourceSQL[i], Convert.ToDateTime(dt.AddSeconds(-(length * i)).ToString("yyyy-MM-dd HH:mm:ss")).ToString("yyyy-MM-dd HH:mm:ss"), 1);
                            }
                        }
                    }
                }
            }

            return sourceSQL;
        }
        #endregion

        public static string MillisecondsToRightTimes(double milliSeconds)
        {
            string result = "";
            double temp = 0;
            if (milliSeconds >= 1000)
            {
                temp = milliSeconds / 1000;
                result = temp + " 秒";
            }
            else if (milliSeconds >= 60000)
            {
                temp = milliSeconds / 60000;
                result = temp + " 分钟";
            }
            else
            {
                result = milliSeconds + " 毫秒";
            }
            return result;
        }

        //创建调度单元
        static Task<IScheduler> tsk = StdSchedulerFactory.GetDefaultScheduler();
        IScheduler scheduler = tsk.Result;

        private async void button2_Click(object sender, EventArgs e)
        {
            //创建调度单元
            //Task<IScheduler> tsk = StdSchedulerFactory.GetDefaultScheduler();
            //IScheduler scheduler = tsk.Result;
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

            job.JobDataMap.Put(SendMessageJob.isauthorization, chkbox_Authorization.Checked.ToString());
            job.JobDataMap.Put(SendMessageJob.isjsonarray, chkbox_JSONArray.Checked.ToString());
            job.JobDataMap.Put(SendMessageJob.uid, textbox_uid.Text.Trim());
            job.JobDataMap.Put(SendMessageJob.pid, textbox_pid.Text.Trim());
            job.JobDataMap.Put(SendMessageJob.loginurl, textBox_loginurl.Text.Trim());

            //监听器
            //scheduler.ListenerManager.GetJobListeners();
            //scheduler.ListenerManager.RemoveJobListener("CustomJobListener");
            scheduler.ListenerManager.AddJobListener(new CustomJobListener(), GroupMatcher<JobKey>.AnyGroup());

            //4.将job和trigger加入到作业调度池中
            await scheduler.ScheduleJob(job, _CronTrigger);
            //5.开启调度
            await scheduler.Start();
            //Console.ReadLine();
            //richTextBox2.Text += "\r\n定时开启";
            richTextBox2.Text = "";
            richTextBox2.Text = richTextBox2.Text.Insert(0, "==============================定时开启==============================\r\n\r\n");

            button2.Enabled = false;
            button3.Enabled = true;

            //关闭红字提示
            label8.Visible = true;
            //mid关闭红字提示
            if (ConfigSettings.is_show_mid_close_warning == "1")
            {
                panel2.Visible = true;
            }
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            //Task<IScheduler> tsk = StdSchedulerFactory.GetDefaultScheduler();
            //IScheduler scheduler = tsk.Result;
            await scheduler.Shutdown();

            PublicVariables.execTimesCronReset();

            //richTextBox2.Text += "\r\n定时结束";
            richTextBox2.Text = richTextBox2.Text.Insert(0, "==============================定时结束==============================\r\n\r\n");

            /*
             ##############################定时结束##############################
             ------------------------------定时结束------------------------------
             ******************************定时结束******************************
             !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!定时结束!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
             ==============================定时结束==============================
             ———————————————定时结束———————————————
             */

            button2.Enabled = true;
            button3.Enabled = false;

            //创建调度单元
            tsk = StdSchedulerFactory.GetDefaultScheduler();
            scheduler = tsk.Result;

            //关闭红字提示
            label8.Visible = false;
            //关闭红字提示
            panel2.Visible = false;
        }

        public void printlog(string newText)
        {
            //richTextBox2.Text += "\r\n" + newText;

            if (richTextBox2.InvokeRequired)
            {
                while (richTextBox2.IsHandleCreated == false)
                {
                    if (richTextBox2.Disposing || richTextBox2.IsDisposed) return;
                }

                CustomJobListener.MyDelegate myDelegate = new CustomJobListener.MyDelegate(printlog);
                richTextBox2.Invoke(myDelegate, new object[] { newText });
            }
            else
            {
                //richTextBox2.Text = "";
                //richTextBox2.Text += "\r\n" + newText;
                //richTextBox2.Text += richTextBox2.Text.Insert(0, newText);
                richTextBox2.Text = richTextBox2.Text.Insert(0, newText);
            }
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

        private void chkbox_WaitingTime_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_WaitingTime.Checked==true)
            {
                txtbox_WaitingTime.Text = "";
                txtbox_WaitingTime.Enabled = true;
                txtbox_WaitingTime.Focus();
            }
            else
            {
                txtbox_WaitingTime.Text = "";
                txtbox_WaitingTime.Enabled = false;
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (label8.Visible == true)
            {
                if (DialogResult.OK == MessageBox.Show("正在执行定时服务，确认关闭？", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning))
                {
                    CreateNewFile(Environment.CurrentDirectory + "\\ClosingLog.txt", "正在执行定时服务，程序关闭时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                    //解决会弹窗两次的问题
                    this.Dispose();
                    Application.Exit();
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                CreateNewFile(Environment.CurrentDirectory + "\\ClosingLog.txt", "定时服务未开启，程序关闭时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n");
                //解决会记录两次的问题
                this.Dispose();
                Application.Exit();
            }
        }

        /// <summary>
        /// 新建文件并写入内容，如果已存在，则覆盖
        /// </summary>
        /// <param name="fileName">文件路径</param>
        /// <param name="content">文件内容</param>
        public static bool CreateNewFile(string fileName, string content)
        {
            try
            {
                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                {
                    StreamWriter sw = new StreamWriter(fs);
                    sw.Write(content);
                    sw.Close();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }

    public class CustomJobListener : IJobListener
    {
        public delegate void MyDelegate(string Item1);
        MyDelegate myDelegate = new MyDelegate(Form1.form1.printlog);

        public static string ExecResult = "";

        public string Name => "CustomJobListener";

        public async Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                //Console.WriteLine($"CustomJobListener JobExecutionVetoed {context.JobDetail.Description}");
                //MessageBox.Show($"CustomJobListener JobExecutionVetoed {context.JobDetail.Description}");
            });
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                //Console.WriteLine($"CustomJobListener JobToBeExecuted {context.JobDetail.Description}");
                //MessageBox.Show($"CustomJobListener JobToBeExecuted {context.JobDetail.Description}");
            });
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default(CancellationToken))
        {
            await Task.Run(() =>
            {
                //Console.WriteLine($"CustomJobListener JobWasExecuted {context.JobDetail.Description}");
                //MessageBox.Show($"CustomJobListener JobWasExecuted {context.JobDetail.Description}");
                //myDelegate("测试委托");
                //MessageBox.Show(ExecResult);
                myDelegate(ExecResult);
            });
        }
    }

    public class SendMessageJob : IJob
    {
        public const string url = "url";
        public const string body = "body";
        public const string backresult = "backresult";
        public const string isauthorization = "false";
        public const string isjsonarray = "false";
        public const string uid = "username";
        public const string pid = "password";
        public const string loginurl = "loginurl";

        public delegate void MyDelegate(string Item1);
        MyDelegate myDelegate = new MyDelegate(Form1.form1.printlog);

        private string resultSubString(string str)
        {
            string result = "";
            if (string.IsNullOrEmpty(ConfigSettings.result_substring))
            {
                result = str;
            }
            else
            {
                result = str.Substring(0, Convert.ToInt32(ConfigSettings.result_substring));
            }
            return result;
        }


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
            string IsAuthorization = data.GetString(isauthorization);
            string IsJSONArray = data.GetString(isjsonarray);
            string Uid = data.GetString(uid);
            string Pid = data.GetString(pid);
            string LoginUrl = data.GetString(loginurl);


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

                    #region 判断是否有匹配{{time(d|h|m|s)(+|-)7:datetime}}
                    //判断是否有匹配{{time(d|h|m|s)(+|-)7:datetime}}
                    if (f1.rgGetDateTimeAll.IsMatch(sqlQuerys[0]))
                    {
                        DateTime dt = DateTime.Now;
                        string str = "";
                        string type = "";
                        string symbol = "";
                        int length = 0;
                        Match matchDateTimeAll;
                        Match matchDateTimeDiff;
                        Match matchDateTime;
                        matchDateTimeAll = f1.rgGetDateTimeAll.Match(sqlQuerys[0]);//{{time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}取整块 日、小时、分钟、秒
                        matchDateTimeDiff = f1.rgGetDateTimeDiff.Match(matchDateTimeAll.Groups[0].Value);//{{timed+-7:2020-03-29 20:00:00}}取(d|h|m|s)(+|-)数字
                        matchDateTime = f1.rgGetDateTime.Match(matchDateTimeAll.Groups[0].Value);//{{timed+-7:2020-03-29 20:00:00}}取时间
                        dt = Convert.ToDateTime(matchDateTime.Groups[0].Value);
                        str = matchDateTimeDiff.Groups[0].Value;//取(d|h|m|s)(+|-)数字
                        type = str.Substring(0, 1);//(d|h|m|s)
                        symbol = str.Substring(1, 1);//(+|-)
                        length = Convert.ToInt32(str.Substring(2, str.Length - 2));//数字


                        //MessageBox.Show("true");
                        f1.getNewDateTime(sqlQuerys);
                    }
                    else
                    {
                        //MessageBox.Show("没有匹配项{{time(d|h|m|s)(+|-)7:datetime}}");
                        noMatch += "没有匹配项{{time(d|h|m|s)(+|-)7:datetime}}\n";
                    }
                    #endregion

                    string[] strlines = noMatch.Split(new string[] { "\n" }, StringSplitOptions.None);
                    int linescount = strlines.Count() - 1;
                    //MessageBox.Show((strlines.Count() - 1).ToString());

                    //if (linescount > 1)
                    if (linescount == 3)
                    {
                        MessageBox.Show(noMatch + "\n" + "请手动取消操作");
                    }
                    else
                    {
                        /*
                        //请求加token前
                        DateTime execStart = DateTime.Now;
                        JavaScriptSerializer s = new JavaScriptSerializer();
                        Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(sqlQuerys[0]));

                        //MessageBox.Show(Url + "\n" + Body + "\n" + noMatch + "\n" + sqlQuerys[0] + "\n" + JsonData.Values);

                        //string result = HTTPHelper.HttpPost(Url, JsonData);
                        string result = HTTPHelper.HttpPost(Url, JsonData);
                        //Thread.Sleep(60000);
                        DateTime execEnd = DateTime.Now;
                        TimeSpan ts = execEnd - execStart;

                        PublicVariables.execTimesCronAddOne();
                        MessageBox.Show(
                            "第 " + PublicVariables.exectimescron + " 次执行\r\n\r\n" +
                            "开始执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                            "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                            "执行结果：\r\n" + result
                            );
                        */

                        string execresult = "";

                        //请求加token后
                        if (IsAuthorization == "True" && (string.IsNullOrEmpty(Uid) || string.IsNullOrEmpty(Pid) || string.IsNullOrEmpty(LoginUrl)))
                        {
                            MessageBox.Show("uid、pid、login_url不能为空");
                        }
                        else if (IsAuthorization == "True" && string.IsNullOrEmpty(Uid) == false && string.IsNullOrEmpty(Pid) == false)
                        {
                            try
                            {
                                JavaScriptSerializer tokenjs = new JavaScriptSerializer();
                                string loginjson = "{\"uid\":\"" + Uid + "\",\"pid\":\"" + Pid + "\"}";
                                Dictionary<string, object> tokenJsonData = (Dictionary<string, object>)tokenjs.DeserializeObject(f1.ConvertJsonString(loginjson));
                                string tokenresult = HTTPHelper.HttpPost(LoginUrl, tokenJsonData);
                                string token = f1.GetJsonValue(tokenresult, "token")[0].ToString();



                                DateTime execStart = DateTime.Now;
                                JavaScriptSerializer s = new JavaScriptSerializer();

                                #region 批量执行改造前
                                //Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(sqlQuerys[0]));

                                //以下两行原本就注释
                                //MessageBox.Show(Url + "\n" + Body + "\n" + noMatch + "\n" + sqlQuerys[0] + "\n" + JsonData.Values);
                                //string result = HTTPHelper.HttpPost(Url, JsonData);

                                //string result = HTTPHelper.HttpPostAuthorization(Url, JsonData, token);
                                #endregion

                                string result = "";

                                #region 批量执行改造后
                                if (IsJSONArray == "True")
                                {
                                    string strBody = sqlQuerys[0];
                                    if (strBody.Substring(0, 1).Contains('['))
                                    {
                                        StringBuilder builder = new StringBuilder(strBody);
                                        builder.Replace("[", "", 0, 1);
                                        strBody = builder.ToString();
                                    }
                                    Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(strBody.Trim()));
                                    result = HTTPHelper.HttpPostAuthorizationArray(Url, JsonData, token);
                                }
                                else
                                {
                                    Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(sqlQuerys[0]));
                                    result = HTTPHelper.HttpPostAuthorization(Url, JsonData, token);
                                }
                                #endregion



                                //Thread.Sleep(60000);
                                DateTime execEnd = DateTime.Now;
                                TimeSpan ts = execEnd - execStart;

                                PublicVariables.execTimesCronAddOne();
                                execresult = "####################第 " + PublicVariables.exectimescron + " 次执行####################\r\n\r\n" +
                                    noMatch + "\r\n\r\n" +
                                    "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                    "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                    "请求参数：\r\n" + sqlQuerys[0] + "\r\n\r\n" +
                                //"执行结果：\r\n" + result;
                                //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                "执行结果：\r\n" + resultSubString(result) + "\r\n\r\n";
                                //MessageBox.Show(execresult);
                                //myDelegate(execresult);
                                CustomJobListener.ExecResult = execresult;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else if (IsAuthorization == "False")
                        {
                            try
                            {
                                DateTime execStart = DateTime.Now;
                                JavaScriptSerializer s = new JavaScriptSerializer();

                                #region 批量执行改造前
                                //Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(sqlQuerys[0].Replace("\n", "").Replace("\t", "").Replace("\r", "")));

                                //以下两行原本就注释
                                //MessageBox.Show(Url + "\n" + Body + "\n" + noMatch + "\n" + sqlQuerys[0] + "\n" + JsonData.Values);
                                //string result = HTTPHelper.HttpPost(Url, JsonData);

                                //string result = HTTPHelper.HttpPost(Url, JsonData);
                                #endregion

                                string result = "";

                                #region 批量执行改造后
                                if (IsJSONArray == "True")
                                {
                                    string strBody = sqlQuerys[0];
                                    if (strBody.Substring(0, 1).Contains('['))
                                    {
                                        StringBuilder builder = new StringBuilder(strBody);
                                        builder.Replace("[", "", 0, 1);
                                        strBody = builder.ToString();
                                    }
                                    Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(strBody.Trim()));
                                    result = HTTPHelper.HttpPostArray(Url, JsonData);
                                }
                                else
                                {
                                    Dictionary<string, object> JsonData = (Dictionary<string, object>)s.DeserializeObject(f1.ConvertJsonString(sqlQuerys[0].Replace("\n", "").Replace("\t", "").Replace("\r", "")));
                                    result = HTTPHelper.HttpPost(Url, JsonData);
                                }
                                #endregion

                                //Thread.Sleep(60000);
                                DateTime execEnd = DateTime.Now;
                                TimeSpan ts = execEnd - execStart;

                                PublicVariables.execTimesCronAddOne();
                                execresult = "####################第 " + PublicVariables.exectimescron + " 次执行####################\r\n\r\n" +
                                    noMatch + "\r\n\r\n" +
                                    "本次执行时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "\r\n\r\n" +
                                    "执行耗时：" + Form1.MillisecondsToRightTimes(ts.TotalMilliseconds) + "\r\n\r\n" +
                                    "请求参数：\r\n" + sqlQuerys[0] + "\r\n\r\n" +
                                    //"执行结果：\r\n" + result;
                                    //"执行结果：\r\n" + result.Substring(0, 3) + "\r\n\r\n";
                                    "执行结果：\r\n" + resultSubString(result) + "\r\n\r\n";
                                //MessageBox.Show(execresult);
                                //myDelegate(execresult);
                                CustomJobListener.ExecResult = execresult;
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                            }
                        }
                        else
                        {
                            MessageBox.Show("ERROR");
                        }
                    }



                }
            });
        }
    }
}