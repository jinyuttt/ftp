using System.IO;
using System.Text.Json;

namespace SmartFtpServer
{

    /// <summary>
    /// ftp工具
    /// </summary>
    public class FtpTool
    {

       /// <summary>
       /// 读取配置
       /// </summary>
       /// <param name="file"></param>
       /// <returns></returns>
        public FtpCfg  ReadCfg(string file="ftpcfg.json")
        {
            if (File.Exists(file))
            {
                using (StreamReader rd = new StreamReader(file))
                {
                    string str = rd.ReadToEnd();
                    return JsonSerializer.Deserialize<FtpCfg>(str);
                }
            }
            else
            {
                FtpCfg cfg = new FtpCfg() { Address = "*", Port = 21 };
               string str= JsonSerializer.Serialize(cfg);
                using(StreamWriter sw=new StreamWriter(file))
                {
                    sw.WriteLine(str);
                }
                return cfg;
            }
           
        }
    }
}
