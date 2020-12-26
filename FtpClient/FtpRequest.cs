#region   文件版本注释
/************************************************************************
*CLR版本  ：4.0.30319.42000
*机器名称 ：DESKTOP-IUKG6RA
*项目名称 ：FtpClient
*项目描述 ：
*命名空间 ：FtpClient
*文件名称 ：FtpRequest.cs
*版本号   :   2020|V1.0.0.0 
---------------------------------------------------------------------
* Copyright @ jinyu 2020. All rights reserved.
---------------------------------------------------------------------

***********************************************************************/
#endregion


using FluentFTP;
using FluentFTP.Rules;
using System;
using System.Collections.Generic;

namespace FtpClient
{

    /// <summary>
    /// Ftp请求
    /// </summary>
    public class FtpRequest
    {
        private FluentFTP.FtpClient ftp = null;

        /// <summary>
        /// 用户名称
        /// </summary>
        public string User { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 端口（默认21）
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 服务器地址（默认localhost）
        /// </summary>
        public string Host { get; set; }

        public event Action<double,int> FtpProgress;

        public FtpRequest()
        {
            Host = "localhost";
            Port = 21;
        }

        /// <summary>
        /// 进度
        /// </summary>
        /// <param name="obj"></param>
        private void ReturnProgress(FtpProgress obj)
        {
            FtpProgress?.Invoke(obj.Progress, obj.FileCount);
        }
        /// <summary>
        /// 连接FTP
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            try
            {
                ftp = new FluentFTP.FtpClient(Host, Port, User, Password)
                {
                    EncryptionMode = FtpEncryptionMode.None,
                    ValidateAnyCertificate = true
                };
                ftp.Connect();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  创建文件夹("/path/that")
        /// </summary>
        /// <param name="dir">服务器文件夹路径</param>
        /// <returns></returns>
        public bool CreateDirectory(string dir)
        {
           return ftp.CreateDirectory(dir, true);
        }

        /// <summary>
        /// 删除文件夹
        /// </summary>
        /// <param name="dir">文件夹路径</param>
        /// <returns></returns>
        public bool DeleteDirectory(string dir)
        {
            try
            {
                ftp.DeleteDirectory(dir);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

         /// <summary>
         /// 删除服务器文件
         /// </summary>
         /// <param name="file">文件路径</param>
         /// <returns>是否成功</returns>
        public bool DeleteFile(string file)
        {
            try
            {
                ftp.DeleteFile(file);
                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        /// 判断服务器文件夹是否存在
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public bool DirectoryExists(string dir)
        {
           return ftp.DirectoryExists(dir);
        }

        /// <summary>
        /// 下载文件夹所有文件（排除特殊文件夹`.git`, `.svn`, `node_modules`等）
        /// </summary>
        /// <param name="localDir">本地路径</param>
        /// <param name="dir">服务器路径</param>
        ///<param name="rule">下载规则</param>
        /// <returns></returns>
        public List<string> DownloadDirectory(string localDir, string dir, DownLoadRule rule=null)
        {
             var lstrules= new List<FtpRule>();
            if (rule != null)
            {
                if(rule.FtpFileExtension!=null&&rule.FtpFileExtension.Length>0)
                {
                    lstrules.Add(new FtpFileExtensionRule(!rule.IsExclude, rule.FtpFileExtension));
                }
                if(rule.FileSize>0)
                {
                    FtpOperator ftpOperator = rule.IsLessThan ? FtpOperator.LessThan : FtpOperator.MoreThan;
                    lstrules.Add(new FtpSizeRule(ftpOperator, rule.FileSize));
                }
            }
            lstrules.Add(new FtpFolderNameRule(false, FtpFolderNameRule.CommonBlacklistedFolders));
            List<string> lst = new List<string>();
            var result=  ftp.DownloadDirectory(localDir, dir, FtpFolderSyncMode.Update, FtpLocalExists.Skip, FtpVerify.None, lstrules,this.ReturnProgress);
            foreach(var p in result)
            {
               if(p.IsSuccess)
                {
                    lst.Add(p.LocalPath);
                }
            }
            return lst;
        }

       

        /// <summary>
        /// 下载文件(覆盖本地文件)
        /// </summary>
        /// <param name="localFile">下载到本地文件</param>
        /// <param name="file">服务器文件</param>
        public void  DownloadFile(string localFile,string file)
        {
            if (ftp.FileExists(file))
            {
                ftp.DownloadFile(localFile, file, FtpLocalExists.Overwrite, FtpVerify.None, this
                    .ReturnProgress);
            }
        }

       /// <summary>
       /// 下载文件到本地文件夹(已经存在的文件被忽略)
       /// </summary>
       /// <param name="localDir">本地文件夹</param>
       /// <param name="files">服务器文件</param>
        public void DownloadFiles(string localDir,string[]files)
        {
            ftp.DownloadFiles(localDir, files, FtpLocalExists.Skip, FtpVerify.None, FtpError.None,this.ReturnProgress);
        }

        /// <summary>
        /// 判断服务器文件是否存在
        /// </summary>
        /// <param name="file">服务器文件</param>
        /// <returns></returns>
        public bool FileExists(string file)
        {
           return ftp.FileExists(file);
        }

        /// <summary>
        /// 获取文件大小
        /// </summary>
        /// <param name="file">服务器文件</param>
        /// <returns></returns>
        public long GetFileSize(string file)
        {
            return ftp.GetFileSize(file);
        }

        

        /// <summary>
        /// 获取服务器列表
        /// </summary>
        /// <param name="dir">获取路径下所有文件夹合文件</param>
        /// <returns>路径</returns>
        public string[] GetAllDirAndFileList(string dir)
        {
            
           return ftp.GetNameListing(dir);
            
        }

       /// <summary>
       /// 上传文件夹(服务器存在则忽略上传)
       /// </summary>
       /// <param name="localDir">本地文件夹</param>
       /// <param name="dir">服务器路径</param>
       /// <param name="rule">上传规则</param>
       /// <returns></returns>
        public List<string> UploadDirectory(string localDir,string dir,UploadRule rule=null)
        {
            List<string> lst = new List<string>();
            List<FtpResult> result = null;
            var lstrules = new List<FtpRule>();
            if (rule != null)
            {
                if (rule.FtpFileExtension != null && rule.FtpFileExtension.Length > 0)
                {
                    lstrules.Add(new FtpFileExtensionRule(!rule.IsExclude, rule.FtpFileExtension));
                }
                if (rule.FileSize > 0)
                {
                    FtpOperator ftpOperator = rule.IsLessThan ? FtpOperator.LessThan : FtpOperator.MoreThan;
                    lstrules.Add(new FtpSizeRule(ftpOperator, rule.FileSize));
                }
            }
            lstrules.Add(new FtpFolderNameRule(false, FtpFolderNameRule.CommonBlacklistedFolders));

            
                result= ftp.UploadDirectory(localDir, dir, FtpFolderSyncMode.Update, FtpRemoteExists.Skip, FtpVerify.None, lstrules,this.ReturnProgress);
               
           
            foreach(var p in result)
            {
                if(p.IsSuccess)
                lst.Add(p.LocalPath);
            }
            return lst;
        }


        /// <summary>
        /// 上传文件(服务器存在则覆盖)
        /// </summary>
        /// <param name="local">本地文件</param>
        /// <param name="file">服务器路径</param>
        /// <returns></returns>
        public bool UploadFile(string local,string file)
        {
           return ftp.UploadFile(local, file, FtpRemoteExists.Overwrite, true,FtpVerify.None,this.ReturnProgress).IsSuccess();
        }

        /// <summary>
        /// 上传批量文件
        /// </summary>
        /// <param name="local">本地文件</param>
        /// <param name="dir">远端目录</param>
        /// <param name="IsForce">是否覆盖服务器已经存在文件</param>
        /// <returns></returns>
        public int UploadFiles(string[] local, string dir, bool IsForce = false)
        {
            List<string> lst = new List<string>();
            if (IsForce)
            {
                return ftp.UploadFiles(local, dir, FtpRemoteExists.Overwrite, true,FtpVerify.None,FtpError.None,this.ReturnProgress);
            }
            else
            {
                return ftp.UploadFiles(local, dir, FtpRemoteExists.Skip, true, FtpVerify.None, FtpError.None, this.ReturnProgress);
            }
          
        }
    }
}
