using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Git.Framework.Printer.Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnDocumentDic_Click(object sender, EventArgs e)
        {
            string tempalte = System.AppDomain.CurrentDomain.BaseDirectory + "\\Template\\Template.xml";
            Dictionary<string, object> dic = GetDataSource();
            IPrint instance = new DocumentPrint(tempalte, "", dic);
            instance.Init().Print();
        }

        private Dictionary<string, object> GetDataSource()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();

            dic.Add("Logo", System.AppDomain.CurrentDomain.BaseDirectory + "abc.jpg");
            dic.Add("OrderCode", "V3454596546");
            dic.Add("DtReceive", "菜霞");
            dic.Add("ReceiveAddress", "634");
            dic.Add("ReceiveUser", "120457");
            dic.Add("ReceiverPhone", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            dic.Add("DtCreate", "65223.00");
            dic.Add("DAmount", "1254");
            dic.Add("DCount", "232");
            dic.Add("StrComment", "MU54232");

            List<Dictionary<string, object>> Info = new List<Dictionary<string, object>>()
            {
                new Dictionary<string, object>() { { "Index", "1"},{ "StrID", "120223"},{ "StrName", "中华烟"},{ "DCount", "2"},{ "DPrice", "49"},{ "DAmount", "98"} },
                new Dictionary<string, object>() { { "Index", "2"},{ "StrID", "565666"},{ "StrName", "玻璃杯"},{ "DCount", "7"},{ "DPrice", "45"},{ "DAmount", "45545"} },
                new Dictionary<string, object>() { { "Index", "3"},{ "StrID", "897845"},{ "StrName", "烟灰缸"},{ "DCount", "5"},{ "DPrice", "2435"},{ "DAmount", "67767"} },
                new Dictionary<string, object>() { { "Index", "4"},{ "StrID", "904395"},{ "StrName", "茶几"},{ "DCount", "3"},{ "DPrice", "45245"},{ "DAmount", "6767"} },

            };
            dic.Add("Detials", Info);

            return dic;
        }
    }
}
