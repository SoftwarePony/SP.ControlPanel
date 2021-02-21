using System;
using Microsoft.AspNetCore.Authorization;

namespace SP.ControlPanel.Api.Authentication.Requirements
{
    public class HasGlobalRoleRequirement : IAuthorizationRequirement
    {
        public string Issuer { get; }
        public string Claim { get; }

        public HasGlobalRoleRequirement(string claim, string issuer)
        {
            Claim = claim ?? throw new ArgumentNullException(nameof(claim));
            Issuer = issuer ?? throw new ArgumentNullException(nameof(issuer));
        }
    }
}