#region   文件版本注释
/************************************************************************
*CLR版本  ：4.0.30319.42000
*机器名称 ：DESKTOP-IUKG6RA
*项目名称 ：FtpClient
*项目描述 ：
*命名空间 ：FtpClient
*文件名称 ：DownLoadRule.cs
*版本号   :   2020|V1.0.0.0 
---------------------------------------------------------------------
* Copyright @ jinyu 2020. All rights reserved.
---------------------------------------------------------------------

***********************************************************************/
#endregion


using System;
using System.Collections.Generic;
using System.Text;

namespace FtpClient
{
   public class DownLoadRule: FilterRule
    {
    }
    public class UploadRule: FilterRule
    {
        
    }

    /// <summary>
    /// 筛选文件
    /// </summary>
    public class FilterRule
    {
        /// <summary>
        /// 文件扩展名规则
        /// </summary>
       public  string[] FtpFileExtension { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public  long FileSize { get; set; }

        /// <summary>
        /// 是否是排除设置的扩展类型文件
        /// </summary>
        public  bool IsExclude { get; set; }

        /// <summary>
        /// 是否是小于设置的大小文件
        /// </summary>
        public bool IsLessThan { get; set; }
    }
}
