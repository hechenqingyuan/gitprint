/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 14:59:17
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 14:59:17       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    public partial class LoopEntity:RowEntity
    {
        public LoopEntity() { }

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string KeyName { get; set; }

        public List<LineEntity> ListLine { get; set; }
    }
}
