/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 15:17:19
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 15:17:19       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer.Pager
{
    public partial class ContentEntity
    {
        public ContentEntity() { }

        /// <summary>
        /// 内容类型 1 文本 2 占位符
        /// </summary>
        public int ContentType { get; set; }

        /// <summary>
        /// 内容名称
        /// </summary>
        public string Content { get; set; }
    }
}
