/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 14:54:24
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 14:54:24       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    public partial class LineEntity: RowEntity
    {
        public LineEntity() { }

        /// <summary>
        /// 行高度
        /// </summary>
        public float Height { get; set; }

        public List<ContentEntity> ListContent { get; set; }
    }
}
