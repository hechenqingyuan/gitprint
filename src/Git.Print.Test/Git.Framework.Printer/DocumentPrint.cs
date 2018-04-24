/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:28:53
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:28:53       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using Git.Framework.Printer.Pager;
using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer
{
    public partial class DocumentPrint : IPrint
    {
        /// <summary>
        /// 打印Document
        /// </summary>
        private PrintDocument PrintDoc { get; set; }

        /// <summary>
        /// 打印页面参数
        /// </summary>
        private PageEntity Page { get; set; }

        /// <summary>
        /// 打印数据源
        /// </summary>
        private Dictionary<string, object> DataSource { get; set; }

        public DocumentPrint(string filePath, string printName, bool isAutoHeigth, Dictionary<string, object> dataSource)
        {
            
        }

        public IPrint Init()
        {
            throw new NotImplementedException();
        }

        public IPrint Print()
        {
            throw new NotImplementedException();
        }

        public IPrint PrintFile(string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
