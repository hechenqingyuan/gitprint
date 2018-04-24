/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 14:51:08
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 14:51:08       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    public partial class PageEntity
    {
        public PageEntity() { }

        /// <summary>
        /// 打印纸张的宽度
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// 打印纸张的高度
        /// </summary>
        public float Heigth { get; set; }

        /// <summary>
        /// 该页面模板的打印机
        /// </summary>
        public string DefaultPrinter { get; set; }

        /// <summary>
        /// 是否自动高度,自动高度 Height 属性失效
        /// </summary>
        public bool AutoHeight { get; set; }

        public List<RowEntity> Rows { get; set; }
    }
}
