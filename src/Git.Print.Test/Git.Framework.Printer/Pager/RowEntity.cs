/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 14:55:03
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 14:55:03       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    /// <summary>
    /// 行基础类
    /// </summary>
    public partial class RowEntity
    {
        public RowEntity() { }

        /// <summary>
        /// 索引值
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 行类型 1 Line 2 Loop 3 Table
        /// </summary>
        public int RowType { get; set; }
    }
}
