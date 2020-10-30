using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RegularlyExecuteHTTPRequests
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btn_DELETE_Click(object sender, EventArgs e)
        {

        }

        private void btn_PUT_Click(object sender, EventArgs e)
        {
            string serviceUrl = "http://localhost/";
            HTTPHelper httphelper = new HTTPHelper(serviceUrl);
            string data = @"{""UserName"":""1111"",""Age"":123,""Id"":133}";
            string uriPost = "api/values/3";
            string retPost = httphelper.Put(data, uriPost);
        }

        private void btn_POST_Click(object sender, EventArgs e)
        {

        }

        private void btn_GET_Click(object sender, EventArgs e)
        {
            MessageBox.Show(HTTPHelper.Get("https://www.baidu.com/s?ie=utf-8&mod=1&isbd=1&isid=BB39D62DFB618099&ie=utf-8&f=8&rsv_bp=1&tn=baidu&wd=RestClient&oq=Rest%2526lt%253Blient&rsv_pq=b9fbf0f70005442f&rsv_t=c488k%2BX1FLp26DkB2hECfqAkSnV4FLK9E7FdQKb3VoI%2Bwrw66RSWTKo9%2BSY&rqlang=cn&rsv_enter=0&rsv_dl=tb&rsv_btype=t&bs=RestClient&rsv_sid=undefined&_ss=1&clist=&hsug=&f4s=1&csor=0&_cr1=26925"));
        }
    }
}