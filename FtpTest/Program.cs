using System;
using System.IO;

namespace FtpTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            FtpClient.FtpRequest ftp = new FtpClient.FtpRequest() { User = "ftpuser", Password = "ftpuser" };
            ftp.FtpProgress += Ftp_FtpProgress;
            var sucess= ftp.Connect();

            if(sucess)
            {
                ftp.CreateDirectory("/cnewdir/2");
                ftp.UploadDirectory(@"d:\local", "/test/");
                ftp.UploadFile(@"d:\\my.txt", "/testfile/my.txt");
                var files = Directory.GetFiles(@"d:\batch");
                ftp.UploadFiles(files, "/uploadbatch");

                //
                ftp.CreateDirectory("/cnewdir/1");
                ftp.DeleteDirectory("//cnewdir/2");
                //
                ftp.DownloadDirectory(@"d:\localdown", "/test/");
                ftp.DownloadFile(@"d:\mydown.txt", "/testfile/my.txt");
                ftp.DownloadFiles(@"d:\mydown", new string[] { "1.txt", "45.txt" });

                ftp.DeleteFile("/test/1.txt");

                ftp.FileExists("/testfile/my.txt");
            }
                    
                    
                    }

        private static void Ftp_FtpProgress(double arg1, int arg2)
        {
            Console.WriteLine($"进度:{ arg1}个数:{ arg2}");
        }
    }
}
