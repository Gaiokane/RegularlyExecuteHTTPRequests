﻿namespace RegularlyExecuteHTTPRequests
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_DELETE = new System.Windows.Forms.Button();
            this.btn_PUT = new System.Windows.Forms.Button();
            this.btn_POST = new System.Windows.Forms.Button();
            this.btn_GET = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.textBox_cron = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textbox_uid = new System.Windows.Forms.TextBox();
            this.textbox_pid = new System.Windows.Forms.TextBox();
            this.chkbox_Authorization = new System.Windows.Forms.CheckBox();
            this.textBox_loginurl = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.chkbox_Tstamp = new System.Windows.Forms.CheckBox();
            this.chkbox_JSONArray = new System.Windows.Forms.CheckBox();
            this.chkbox_WaitingTime = new System.Windows.Forms.CheckBox();
            this.txtbox_WaitingTime = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btn_DELETE
            // 
            this.btn_DELETE.Location = new System.Drawing.Point(588, 62);
            this.btn_DELETE.Name = "btn_DELETE";
            this.btn_DELETE.Size = new System.Drawing.Size(75, 23);
            this.btn_DELETE.TabIndex = 0;
            this.btn_DELETE.Text = "DELETE";
            this.btn_DELETE.UseVisualStyleBackColor = true;
            this.btn_DELETE.Click += new System.EventHandler(this.btn_DELETE_Click);
            // 
            // btn_PUT
            // 
            this.btn_PUT.Location = new System.Drawing.Point(588, 91);
            this.btn_PUT.Name = "btn_PUT";
            this.btn_PUT.Size = new System.Drawing.Size(75, 23);
            this.btn_PUT.TabIndex = 1;
            this.btn_PUT.Text = "PUT";
            this.btn_PUT.UseVisualStyleBackColor = true;
            this.btn_PUT.Click += new System.EventHandler(this.btn_PUT_Click);
            // 
            // btn_POST
            // 
            this.btn_POST.Location = new System.Drawing.Point(588, 120);
            this.btn_POST.Name = "btn_POST";
            this.btn_POST.Size = new System.Drawing.Size(75, 23);
            this.btn_POST.TabIndex = 2;
            this.btn_POST.Text = "POST";
            this.btn_POST.UseVisualStyleBackColor = true;
            this.btn_POST.Click += new System.EventHandler(this.btn_POST_Click);
            // 
            // btn_GET
            // 
            this.btn_GET.Location = new System.Drawing.Point(588, 149);
            this.btn_GET.Name = "btn_GET";
            this.btn_GET.Size = new System.Drawing.Size(75, 23);
            this.btn_GET.TabIndex = 3;
            this.btn_GET.Text = "GET";
            this.btn_GET.UseVisualStyleBackColor = true;
            this.btn_GET.Click += new System.EventHandler(this.btn_GET_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(14, 33);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(568, 199);
            this.richTextBox1.TabIndex = 4;
            this.richTextBox1.Text = "";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(588, 33);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "验证Json";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(563, 6);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 21);
            this.textBox1.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(492, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "执行次数：";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(83, 6);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(403, 21);
            this.textBox2.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "接口地址：";
            // 
            // richTextBox2
            // 
            this.richTextBox2.Location = new System.Drawing.Point(14, 239);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(568, 231);
            this.richTextBox2.TabIndex = 10;
            this.richTextBox2.Text = "";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(588, 178);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(81, 23);
            this.button2.TabIndex = 11;
            this.button2.Text = "Quartz POST";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(588, 207);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(81, 23);
            this.button3.TabIndex = 12;
            this.button3.Text = "Quartz Stop";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // textBox_cron
            // 
            this.textBox_cron.Location = new System.Drawing.Point(669, 239);
            this.textBox_cron.Name = "textBox_cron";
            this.textBox_cron.Size = new System.Drawing.Size(176, 21);
            this.textBox_cron.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(586, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 12);
            this.label3.TabIndex = 14;
            this.label3.Text = "Cron表达式：";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(675, 178);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 15;
            this.button4.Text = "测试正则";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(586, 263);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(263, 120);
            this.label4.TabIndex = 16;
            this.label4.Text = "{{time(d|h|m|s)(+|-)7:2020-03-29 20:00:00}}\r\n\r\n{{onthehour}}：取当前整点小时\r\n\r\n{{ontheho" +
    "ur(+|-)小时数}}：\r\n取当前整点小时+或-指定小时数\r\n\r\n以上时间格式均为yyyy-MM-dd HH:mm:ss\r\n\r\n可点击测试正则查看生成的时间";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(589, 398);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 17;
            this.label5.Text = "uid：";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(589, 425);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 18;
            this.label6.Text = "pid：";
            // 
            // textbox_uid
            // 
            this.textbox_uid.Location = new System.Drawing.Point(630, 395);
            this.textbox_uid.Name = "textbox_uid";
            this.textbox_uid.Size = new System.Drawing.Size(215, 21);
            this.textbox_uid.TabIndex = 19;
            // 
            // textbox_pid
            // 
            this.textbox_pid.Location = new System.Drawing.Point(630, 422);
            this.textbox_pid.Name = "textbox_pid";
            this.textbox_pid.Size = new System.Drawing.Size(215, 21);
            this.textbox_pid.TabIndex = 20;
            // 
            // chkbox_Authorization
            // 
            this.chkbox_Authorization.AutoSize = true;
            this.chkbox_Authorization.Location = new System.Drawing.Point(669, 124);
            this.chkbox_Authorization.Name = "chkbox_Authorization";
            this.chkbox_Authorization.Size = new System.Drawing.Size(102, 16);
            this.chkbox_Authorization.TabIndex = 21;
            this.chkbox_Authorization.Text = "Authorization";
            this.chkbox_Authorization.UseVisualStyleBackColor = true;
            // 
            // textBox_loginurl
            // 
            this.textBox_loginurl.Location = new System.Drawing.Point(664, 449);
            this.textBox_loginurl.Name = "textBox_loginurl";
            this.textBox_loginurl.Size = new System.Drawing.Size(181, 21);
            this.textBox_loginurl.TabIndex = 22;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(587, 452);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 23;
            this.label7.Text = "login_url：";
            // 
            // chkbox_Tstamp
            // 
            this.chkbox_Tstamp.AutoSize = true;
            this.chkbox_Tstamp.Location = new System.Drawing.Point(669, 146);
            this.chkbox_Tstamp.Name = "chkbox_Tstamp";
            this.chkbox_Tstamp.Size = new System.Drawing.Size(132, 28);
            this.chkbox_Tstamp.TabIndex = 24;
            this.chkbox_Tstamp.Text = "DateTimeToTstamp\r\n（仅{{time}}生效）";
            this.chkbox_Tstamp.UseVisualStyleBackColor = true;
            // 
            // chkbox_JSONArray
            // 
            this.chkbox_JSONArray.AutoSize = true;
            this.chkbox_JSONArray.Location = new System.Drawing.Point(756, 182);
            this.chkbox_JSONArray.Name = "chkbox_JSONArray";
            this.chkbox_JSONArray.Size = new System.Drawing.Size(96, 16);
            this.chkbox_JSONArray.TabIndex = 25;
            this.chkbox_JSONArray.Text = "请求体[]包裹";
            this.chkbox_JSONArray.UseVisualStyleBackColor = true;
            // 
            // chkbox_WaitingTime
            // 
            this.chkbox_WaitingTime.AutoSize = true;
            this.chkbox_WaitingTime.Location = new System.Drawing.Point(669, 11);
            this.chkbox_WaitingTime.Name = "chkbox_WaitingTime";
            this.chkbox_WaitingTime.Size = new System.Drawing.Size(120, 16);
            this.chkbox_WaitingTime.TabIndex = 26;
            this.chkbox_WaitingTime.Text = "等待时间（秒）：";
            this.chkbox_WaitingTime.UseVisualStyleBackColor = true;
            this.chkbox_WaitingTime.CheckedChanged += new System.EventHandler(this.chkbox_WaitingTime_CheckedChanged);
            // 
            // txtbox_WaitingTime
            // 
            this.txtbox_WaitingTime.Location = new System.Drawing.Point(784, 6);
            this.txtbox_WaitingTime.Name = "txtbox_WaitingTime";
            this.txtbox_WaitingTime.Size = new System.Drawing.Size(68, 21);
            this.txtbox_WaitingTime.TabIndex = 27;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 3);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "label8";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(669, 33);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(132, 81);
            this.panel1.TabIndex = 25;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label9);
            this.panel2.Location = new System.Drawing.Point(27, 149);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(541, 81);
            this.panel2.TabIndex = 26;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 3);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(41, 12);
            this.label9.TabIndex = 24;
            this.label9.Text = "label9";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(857, 482);
            this.Controls.Add(this.txtbox_WaitingTime);
            this.Controls.Add(this.chkbox_WaitingTime);
            this.Controls.Add(this.chkbox_JSONArray);
            this.Controls.Add(this.chkbox_Tstamp);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.textBox_loginurl);
            this.Controls.Add(this.chkbox_Authorization);
            this.Controls.Add(this.textbox_pid);
            this.Controls.Add(this.textbox_uid);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_cron);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btn_GET);
            this.Controls.Add(this.btn_POST);
            this.Controls.Add(this.btn_PUT);
            this.Controls.Add(this.btn_DELETE);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_DELETE;
        private System.Windows.Forms.Button btn_PUT;
        private System.Windows.Forms.Button btn_POST;
        private System.Windows.Forms.Button btn_GET;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        public System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.TextBox textBox_cron;
        public System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textbox_uid;
        private System.Windows.Forms.TextBox textbox_pid;
        private System.Windows.Forms.CheckBox chkbox_Authorization;
        private System.Windows.Forms.TextBox textBox_loginurl;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chkbox_Tstamp;
        private System.Windows.Forms.CheckBox chkbox_JSONArray;
        private System.Windows.Forms.CheckBox chkbox_WaitingTime;
        private System.Windows.Forms.TextBox txtbox_WaitingTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label9;
    }
}

