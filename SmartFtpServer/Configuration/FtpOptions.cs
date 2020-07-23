// <copyright file="FtpOptions.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System;

namespace SmartFtpServer.Configuration
{
    /// <summary>
    /// The root object for all options.
    /// </summary>
    public class FtpOptions
    {
      

        /// <summary>
        /// Gets or sets authentication providers to use.
        /// </summary>
        public MembershipProviderType Authentication { get; set; } = MembershipProviderType.Default;

        /// <summary>
        /// Gets or sets a value indicating whether user/group IDs will be set for file system operations.
        /// </summary>
        public bool SetFileSystemId { get; set; }

        /// <summary>
        /// Gets or sets the bits to be removed from the default file system entry permissions.
        /// </summary>
        public string? Umask { get; set; }

        /// <summary>
        /// Gets or sets the PAM authorization options.
        /// </summary>
        public PamAuthOptions Pam { get; set; } = new PamAuthOptions();

        /// <summary>
        /// Gets or sets the FTP server options.
        /// </summary>
        public FtpServerOptions Server { get; set; } = new FtpServerOptions();

        /// <summary>
        /// Gets or sets the FTPS options.
        /// </summary>
        public FtpsOptions Ftps { get; set; } = new FtpsOptions();

      
     
        
    }
}
