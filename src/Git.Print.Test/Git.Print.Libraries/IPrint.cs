using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Git.Print.Libraries
{
    public partial interface IPrint
    {
        /// <summary>
        /// 初始化打印
        /// </summary>
        /// <returns></returns>
        IPrint Init();

        /// <summary>
        /// 打印调用执行
        /// </summary>
        /// <returns></returns>
        IPrint Print();

        /// <summary>
        /// 打印文件
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        IPrint PrintFile(string fileName);
    }
}