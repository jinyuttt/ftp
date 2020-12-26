namespace SmartFtpServer
{

    /// <summary>
    /// 配置信息
    /// </summary>
    public class FtpCfg
    {
        /// <summary>
        /// 绑定IP(默认*)
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 绑定端口(默认21）
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
    }
}
