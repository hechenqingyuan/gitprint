/*******************************************************************************
 * Copyright (C) Git Corporation. All rights reserved.
 *
 * Author: 情缘
 * Create Date: 2018/4/24 17:21:09
 *
 * Description: Git.Framework
 * http://www.cnblogs.com/qingyuan/
 * Revision History:
 * Date         Author               Description
 * 2018/4/24 17:21:09       情缘
 * 吉特仓储管理系统 开源地址 https://github.com/hechenqingyuan/gitwms
 * 项目地址:http://yun.gitwms.com/
*********************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Framework.Printer
{
    public static class DataSourceExtension
    {
        /// <summary>
        /// 获取值
        /// </summary>
        /// <typeparam name="K"></typeparam>
        /// <typeparam name="Value"></typeparam>
        /// <param name="dataSource"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
        public static Value Value<K, Value>(this Dictionary<K, Value> dataSource, K Key)
        {
            Value Result = default(Value);
            if (dataSource == null)
            {
                return Result;
            }
            if (!dataSource.ContainsKey(Key))
            {
                return Result;
            }

            return dataSource[Key];
        }
    }
}
