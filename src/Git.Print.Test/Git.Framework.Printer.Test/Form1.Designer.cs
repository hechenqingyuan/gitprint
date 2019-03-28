namespace Git.Framework.Printer.Test
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
            this.btnDocumentDic = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDocumentDic
            // 
            this.btnDocumentDic.Location = new System.Drawing.Point(12, 12);
            this.btnDocumentDic.Name = "btnDocumentDic";
            this.btnDocumentDic.Size = new System.Drawing.Size(138, 34);
            this.btnDocumentDic.TabIndex = 1;
            this.btnDocumentDic.Text = "Document打印(数据源)";
            this.btnDocumentDic.UseVisualStyleBackColor = true;
            this.btnDocumentDic.Click += new System.EventHandler(this.btnDocumentDic_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(73, 113);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(138, 34);
            this.button1.TabIndex = 2;
            this.button1.Text = "蓝牙打印打印(数据源)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnDocumentDic);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnDocumentDic;
        private System.Windows.Forms.Button button1;
    }
}

