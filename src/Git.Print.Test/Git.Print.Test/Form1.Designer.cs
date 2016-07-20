namespace Git.Print.Test
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
            this.btnDocumentJson = new System.Windows.Forms.Button();
            this.btnDocumentFile = new System.Windows.Forms.Button();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.btnZPL = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnDocumentDic
            // 
            this.btnDocumentDic.Location = new System.Drawing.Point(12, 12);
            this.btnDocumentDic.Name = "btnDocumentDic";
            this.btnDocumentDic.Size = new System.Drawing.Size(138, 34);
            this.btnDocumentDic.TabIndex = 0;
            this.btnDocumentDic.Text = "Document打印(数据源)";
            this.btnDocumentDic.UseVisualStyleBackColor = true;
            this.btnDocumentDic.Click += new System.EventHandler(this.btnDocumentDic_Click);
            // 
            // btnDocumentJson
            // 
            this.btnDocumentJson.Location = new System.Drawing.Point(166, 12);
            this.btnDocumentJson.Name = "btnDocumentJson";
            this.btnDocumentJson.Size = new System.Drawing.Size(138, 34);
            this.btnDocumentJson.TabIndex = 1;
            this.btnDocumentJson.Text = "Document打印(JSON)";
            this.btnDocumentJson.UseVisualStyleBackColor = true;
            // 
            // btnDocumentFile
            // 
            this.btnDocumentFile.Location = new System.Drawing.Point(12, 64);
            this.btnDocumentFile.Name = "btnDocumentFile";
            this.btnDocumentFile.Size = new System.Drawing.Size(138, 34);
            this.btnDocumentFile.TabIndex = 2;
            this.btnDocumentFile.Text = "Document打印(File)";
            this.btnDocumentFile.UseVisualStyleBackColor = true;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(166, 64);
            this.txtFilePath.Multiline = true;
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.Size = new System.Drawing.Size(302, 34);
            this.txtFilePath.TabIndex = 3;
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(474, 64);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(138, 34);
            this.btnSelectFile.TabIndex = 4;
            this.btnSelectFile.Text = "选择文件";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            // 
            // btnZPL
            // 
            this.btnZPL.Location = new System.Drawing.Point(12, 152);
            this.btnZPL.Name = "btnZPL";
            this.btnZPL.Size = new System.Drawing.Size(138, 34);
            this.btnZPL.TabIndex = 5;
            this.btnZPL.Text = "ZPL打印(数据源)";
            this.btnZPL.UseVisualStyleBackColor = true;
            this.btnZPL.Click += new System.EventHandler(this.btnZPL_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 417);
            this.Controls.Add(this.btnZPL);
            this.Controls.Add(this.btnSelectFile);
            this.Controls.Add(this.txtFilePath);
            this.Controls.Add(this.btnDocumentFile);
            this.Controls.Add(this.btnDocumentJson);
            this.Controls.Add(this.btnDocumentDic);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印测试";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnDocumentDic;
        private System.Windows.Forms.Button btnDocumentJson;
        private System.Windows.Forms.Button btnDocumentFile;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnZPL;
    }
}

