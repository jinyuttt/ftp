// <copyright file="CustomMembershipProvider.cs" company="Fubar Development Junker">
// Copyright (c) Fubar Development Junker. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

using FubarDev.FtpServer.AccountManagement;

namespace SmartFtpServer
{
    /// <summary>
    /// Custom membership provider
    /// </summary>
    public class CustomMembershipProvider : IMembershipProvider
    {
        public static List<UserInfo> Users { get; set; }
        /// <inheritdoc />
        public Task<MemberValidationResult> ValidateUserAsync(string username, string password)
        {
            if (Users == null || Users.Count == 0)
            {
                return Task.FromResult(new MemberValidationResult(MemberValidationStatus.InvalidLogin));
            }

            if (Users.FindIndex(X => X.UserName == username && X.Password == password) > -1)
            {
                var user = new ClaimsPrincipal(
                    new ClaimsIdentity(
                        new[]
                        {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, username),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType, "user"),
                        },
                        "custom"));

                return Task.FromResult(
                    new MemberValidationResult(
                        MemberValidationStatus.AuthenticatedUser,
                        user));
            }
            else
            {
                return Task.FromResult(new MemberValidationResult(MemberValidationStatus.InvalidLogin));
            }

        }
    }
}
