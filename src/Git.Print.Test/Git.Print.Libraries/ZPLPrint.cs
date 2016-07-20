using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Print.Libraries
{
    public partial class ZPLPrint:IPrint
    {
        /// <summary>
        /// 打印模板文件路径
        /// </summary>
        private string FilePath { get; set; }

        /// <summary>
        /// 打印机名称(COM 口)
        /// </summary>
        private string ComName { get; set; }

        /// <summary>
        /// 纸张是否自动高度
        /// </summary>
        private bool IsAutoHeight { get; set; }

        /// <summary>
        /// 打印数据源
        /// </summary>
        private Dictionary<string, object> DataSource { get; set; }

        public ZPLPrint()
        {
        }

        public ZPLPrint(string filePath,string comName,bool isAutoHeight,Dictionary<string,object> dataSource)
        {
            this.FilePath = filePath;
            this.ComName = comName;
            this.IsAutoHeight = isAutoHeight;
            this.DataSource = dataSource;
        }

        public ZPLPrint(string filePath, string comName, bool isAutoHeight, string dataSource)
        {
            this.FilePath = filePath;
            this.ComName = comName;
            this.IsAutoHeight = isAutoHeight;
            JsonDataSource convert = new JsonDataSource();
            this.DataSource = convert.To(dataSource);
        }
    }
}
