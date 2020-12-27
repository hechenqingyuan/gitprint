using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Git.Framework.Printer.Test
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            var a = HttpGet("http://www.ip138.com", "gbk");
            var a1 = parseHtml(a);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        public static string HttpGet(string url, string encoding)
        {
            Encoding encode = System.Text.Encoding.GetEncoding(encoding);
            WebClient MyWebClient = new WebClient();
            MyWebClient.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/68.0.3440.84 Safari/537.36");
            MyWebClient.Credentials = CredentialCache.DefaultCredentials;//获取或设置用于向Internet资源的请求进行身份验证的网络凭据
            Byte[] pageData = MyWebClient.DownloadData("http://www.net.cn/static/customercare/yourip.asp"); //从指定网站下载数据
            string pageHtml = encode.GetString(pageData);  //如果获取网站页面采用的是GB2312，则 使用这句
            return pageHtml;
        }

        public static string parseHtml(String pageHtml)
        {
            string ip = "";
            Match m = Regex.Match(pageHtml, @"((25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))\.){3}(25[0-5]|2[0-4]\d|((1\d{2})|([1-9]?\d)))");

            if (m.Success)
            {
                ip = m.Value;
            }
            
            return ip;
        }
    }
}
