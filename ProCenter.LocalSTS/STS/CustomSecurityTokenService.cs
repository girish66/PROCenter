﻿//-----------------------------------------------------------------------------
//
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved.
//
//
//-----------------------------------------------------------------------------

namespace ProCenter.LocalSTS.STS
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IdentityModel;
    using System.IdentityModel.Configuration;
    using System.IdentityModel.Protocols.WSTrust;
    using System.IdentityModel.Tokens;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Web.Configuration;

    #endregion

    /// <summary>
    ///     A custom SecurityTokenService implementation.
    /// </summary>
    public class CustomSecurityTokenService : SecurityTokenService
    {
        // TODO: Set enableAppliesToValidation to true to enable only the RP Url's specified in the PassiveRedirectBasedClaimsAwareWebApps array to get a token from this STS
        // TODO: Add relying party Url's that will be allowed to get token from this STS
        private static readonly string[] PassiveRedirectBasedClaimsAwareWebApps =
            {
                /*"https://localhost/PassiveRedirectBasedClaimsAwareWebApp"*/
            };

        private static readonly string EmergencyAccessCapableSecurityGroup = "EmergencyAccessCapable";
        private bool enableAppliesToValidation = false;

        /// <summary>
        ///     Creates an instance of CustomSecurityTokenService.
        /// </summary>
        /// <param name="configuration">The SecurityTokenServiceConfiguration.</param>
        public CustomSecurityTokenService(SecurityTokenServiceConfiguration configuration)
            : base(configuration)
        {
        }

        /// <summary>
        ///     Validates appliesTo and throws an exception if the appliesTo is null or contains an unexpected address.
        /// </summary>
        /// <param name="appliesTo">The AppliesTo value that came in the RST.</param>
        /// <exception cref="ArgumentNullException">If 'appliesTo' parameter is null.</exception>
        /// <exception cref="InvalidRequestException">If 'appliesTo' is not valid.</exception>
        private void ValidateAppliesTo(EndpointReference appliesTo)
        {
            if (appliesTo == null)
            {
                throw new ArgumentNullException("appliesTo");
            }

            // TODO: Enable AppliesTo validation for allowed relying party Urls by setting enableAppliesToValidation to true. By default it is false.
            if (enableAppliesToValidation)
            {
                bool validAppliesTo = false;
                foreach (var rpUrl in PassiveRedirectBasedClaimsAwareWebApps)
                {
                    if (appliesTo.Uri.Equals(new Uri(rpUrl)))
                    {
                        validAppliesTo = true;
                        break;
                    }
                }

                if (!validAppliesTo)
                {
                    throw new InvalidRequestException(String.Format("The 'appliesTo' address '{0}' is not valid.",
                                                                    appliesTo.Uri.OriginalString));
                }
            }
        }


        public override RequestSecurityTokenResponse Issue(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            var tokenLifetime = ConfigurationManager.AppSettings["TokenLifetimeMinutes"];

            // Set a non-default lifetime in minutes from configuration.
            if (tokenLifetime != string.Empty)
            {
                request.Lifetime = new Lifetime(DateTime.UtcNow,
                                                DateTime.UtcNow.AddMinutes(Convert.ToDouble(tokenLifetime)));
            }

            var token = base.Issue(principal, request);

            return token;
        }

        /// <summary>
        ///     This method returns the configuration for the token issuance request. The configuration
        ///     is represented by the Scope class. In our case, we are only capable of issuing a token for a
        ///     single RP identity represented by the EncryptingCertificateName.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST.</param>
        /// <returns>The scope information to be used for the token issuance.</returns>
        protected override Scope GetScope(ClaimsPrincipal principal, RequestSecurityToken request)
        {
            ValidateAppliesTo(request.AppliesTo);

            //
            // Note: The signing certificate used by default has a Distinguished name of "CN=STSTestCert",
            // and is located in the Personal certificate store of the Local Computer. Before going into production,
            // ensure that you change this certificate to a valid CA-issued certificate as appropriate.
            //
            var scope = new Scope(request.AppliesTo.Uri.OriginalString,
                                  SecurityTokenServiceConfiguration.SigningCredentials);

            string encryptingCertificateName = WebConfigurationManager.AppSettings["EncryptingCertificateName"];
            if (!string.IsNullOrEmpty(encryptingCertificateName))
            {
                // Important note on setting the encrypting credentials.
                // In a production deployment, you would need to select a certificate that is specific to the RP that is requesting the token.
                // You can examine the 'request' to obtain information to determine the certificate to use.
                scope.EncryptingCredentials =
                    new X509EncryptingCredentials(CertificateUtil.GetCertificate(StoreName.My,
                                                                                 StoreLocation.LocalMachine,
                                                                                 encryptingCertificateName));
            }
            else
            {
                // If there is no encryption certificate specified, the STS will not perform encryption.
                // This will succeed for tokens that are created without keys (BearerTokens) or asymmetric keys.  
                scope.TokenEncryptionRequired = false;
            }

            // Set the ReplyTo address for the WS-Federation passive protocol (wreply). This is the address to which responses will be directed. 
            // In this template, we have chosen to set this to the AppliesToAddress.
            scope.ReplyToAddress = scope.AppliesToAddress;

            return scope;
        }

        /// <summary>
        ///     This method returns the claims to be issued in the token.
        /// </summary>
        /// <param name="principal">The caller's principal.</param>
        /// <param name="request">The incoming RST, can be used to obtain additional information.</param>
        /// <param name="scope">The scope information corresponding to this request.</param>
        /// <exception cref="ArgumentNullException">If 'principal' parameter is null.</exception>
        /// <returns>The outgoing claimsIdentity to be included in the issued token.</returns>
        protected override ClaimsIdentity GetOutputClaimsIdentity(ClaimsPrincipal principal, RequestSecurityToken request, Scope scope)
        {
            string roleClaim;
            string emailClaim;
            string nameClaim = string.Empty;
            var claims = new List<Claim>();

            if (null == principal)
            {
                throw new ArgumentNullException("principal");
            }

            var section = (UserRoleSection) ConfigurationManager.GetSection("userRoleGroup/userRole");

            // Hard-coded claims for testing only.
            switch (principal.Identity.Name)
            {
                case "leo.smith":
                    nameClaim = "Leo Smith";
                    roleClaim = section.Roles;
                    emailClaim = "leo.smith@safeharbor.com";

                    break;
                case "oren.ishii":
                    nameClaim = "O-Ren Ishii";
                    roleClaim = section.Roles;
                    emailClaim = "oren.ishii@gmail.com";

                    break;
                case "system.admin":
                    nameClaim = "System Admin";
                    roleClaim = section.Roles;
                    emailClaim = "system.admin@feisystems.com";

                    break;
                case "chris.white":
                    nameClaim = "Chris White";
                    roleClaim = section.Roles + ", Demoer";
                    emailClaim = "chris.white@safeharbor1x1.com";

                    break;

                case "cindy.thomas":
                    nameClaim = "Cindy Thomas";
                    roleClaim = "FrontDesk";
                    emailClaim = "cindy.thomas@safeharbor1x1.com";

                    // User has permission to Exercise Emergency Access.
                    claims.Add(new Claim(ClaimTypes.GroupSid, EmergencyAccessCapableSecurityGroup));

                    break;
                case "lily.foley":
                    roleClaim = "Intern";
                    emailClaim = "lily.foley@safeharbor1x1.com";
                    break;

                case "user03":
                    roleClaim = "Patient";
                    emailClaim = "john352@gmail.com";
                    break;

                case "dennis.ladder":

                    roleClaim = section.Roles;
                    emailClaim = "dennis.ladder@safeharbor1x1.com";

                    break;
                case "BillingClaimsProcessor":

                    roleClaim = section.Roles;
                    emailClaim = "BillingClaimsProcessor@safeharbor1x1.com";

                    break;
                default:
                    roleClaim = "Unauthorized";
                    emailClaim = "unknown@nowhere.com";
                    break;
            }


            // Issue custom claims.
            // TODO: Change the claims below to issue custom claims required by your application.
            // Update the application's configuration file too to reflect new claims requirement.

            claims.Add(new Claim(ClaimTypes.Name, nameClaim));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, emailClaim));

            roleClaim.Split(new[] {',', ' '}, StringSplitOptions.RemoveEmptyEntries)
                     .ToList()
                     .ForEach(role =>
                              claims.Add(new Claim(ClaimTypes.GroupSid, role.Trim()))
                );

            claims.Add(new Claim(ClaimTypes.Email, emailClaim));

            var outputIdentity = new ClaimsIdentity(claims);

            return outputIdentity;
        }
    }
}