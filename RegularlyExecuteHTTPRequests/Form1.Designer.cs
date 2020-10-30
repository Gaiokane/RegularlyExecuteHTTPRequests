namespace RegularlyExecuteHTTPRequests
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
            this.SuspendLayout();
            // 
            // btn_DELETE
            // 
            this.btn_DELETE.Location = new System.Drawing.Point(365, 149);
            this.btn_DELETE.Name = "btn_DELETE";
            this.btn_DELETE.Size = new System.Drawing.Size(75, 23);
            this.btn_DELETE.TabIndex = 0;
            this.btn_DELETE.Text = "DELETE";
            this.btn_DELETE.UseVisualStyleBackColor = true;
            this.btn_DELETE.Click += new System.EventHandler(this.btn_DELETE_Click);
            // 
            // btn_PUT
            // 
            this.btn_PUT.Location = new System.Drawing.Point(365, 178);
            this.btn_PUT.Name = "btn_PUT";
            this.btn_PUT.Size = new System.Drawing.Size(75, 23);
            this.btn_PUT.TabIndex = 1;
            this.btn_PUT.Text = "PUT";
            this.btn_PUT.UseVisualStyleBackColor = true;
            this.btn_PUT.Click += new System.EventHandler(this.btn_PUT_Click);
            // 
            // btn_POST
            // 
            this.btn_POST.Location = new System.Drawing.Point(365, 207);
            this.btn_POST.Name = "btn_POST";
            this.btn_POST.Size = new System.Drawing.Size(75, 23);
            this.btn_POST.TabIndex = 2;
            this.btn_POST.Text = "POST";
            this.btn_POST.UseVisualStyleBackColor = true;
            this.btn_POST.Click += new System.EventHandler(this.btn_POST_Click);
            // 
            // btn_GET
            // 
            this.btn_GET.Location = new System.Drawing.Point(365, 236);
            this.btn_GET.Name = "btn_GET";
            this.btn_GET.Size = new System.Drawing.Size(75, 23);
            this.btn_GET.TabIndex = 3;
            this.btn_GET.Text = "GET";
            this.btn_GET.UseVisualStyleBackColor = true;
            this.btn_GET.Click += new System.EventHandler(this.btn_GET_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btn_GET);
            this.Controls.Add(this.btn_POST);
            this.Controls.Add(this.btn_PUT);
            this.Controls.Add(this.btn_DELETE);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_DELETE;
        private System.Windows.Forms.Button btn_PUT;
        private System.Windows.Forms.Button btn_POST;
        private System.Windows.Forms.Button btn_GET;
    }
}

