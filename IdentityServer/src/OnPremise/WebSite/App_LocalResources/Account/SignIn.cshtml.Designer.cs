﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.18033
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Thinktecture.IdentityServer.Web.App_LocalResources.Account
{


    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class SignIn_cshtml {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal SignIn_cshtml() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Thinktecture.IdentityServer.Web.App_LocalResources.Account.SignIn.cshtml", typeof(SignIn_cshtml).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Login.
        /// </summary>
        public static string Login {
            get {
                return ResourceManager.GetString("Login", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sign In.
        /// </summary>
        public static string SignIn {
            get {
                return ResourceManager.GetString("SignIn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Sign In was unsuccessful. Please correct the errors and try again..
        /// </summary>
        public static string SignInWasUnsuccessful {
            get {
                return ResourceManager.GetString("SignInWasUnsuccessful", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to You are currenty logged in as {0}, but you requested a resource that you were not authorized for. Either provide credentials that do have access or contact your administrator to grant you access..
        /// </summary>
        public static string UnauthorizedAuthenticatedUser {
            get {
                return ResourceManager.GetString("UnauthorizedAuthenticatedUser", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Use client certificate.
        /// </summary>
        public static string UseClientCertificate {
            get {
                return ResourceManager.GetString("UseClientCertificate", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Username / Password Sign In.
        /// </summary>
        public static string UsernamePasswordSignIn {
            get {
                return ResourceManager.GetString("UsernamePasswordSignIn", resourceCulture);
            }
        }
    }
}
