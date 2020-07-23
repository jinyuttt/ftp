using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace SmartFtpServer
{
   public class FtpTool
    {
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
