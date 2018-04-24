/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:04:28
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:04:28       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    public partial class TableEntity:RowEntity
    {
        public TableEntity() { }

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string KeyName { get; set; }

        /// <summary>
        /// 边框宽度
        /// </summary>
        public float BorderWidth { get; set; }

        /// <summary>
        /// 表格宽度
        /// </summary>
        public float Width { get; set; }

        public List<TrEntity> ListTR { get; set; }
    }
}
