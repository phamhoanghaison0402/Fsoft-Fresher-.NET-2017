using System;
using System.Security;
using Microsoft.Exchange.WebServices.Data;

namespace Authentication
{
    /// <summary>
    /// IUserData
    /// </summary>
    public interface IUserData
    {
        ExchangeVersion Version { get; }
        string EmailAddress { get; }
        SecureString Password { get; }
        Uri AutodiscoverUrl { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Authentication.IUserData" />
    public class UserDataFromConsole : IUserData
    {
        /// <summary>
        /// The user data
        /// </summary>
        public static UserDataFromConsole UserData;

        /// <summary>
        /// Gets the user data.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public static IUserData GetUserData(string account,string password)
        {
            if (UserData == null)
            {
                GetUserDataFromConsole(account,password);
            }
            return UserData;
        }

        /// <summary>
        /// Gets the user data from console.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="password">The password.</param>
        private static void GetUserDataFromConsole(string account,string password)
        {
            UserData = new UserDataFromConsole();
            UserData.EmailAddress =account;
            UserData.Password = new SecureString();
            foreach(char a in password)
            {
                UserData.Password.AppendChar(a);
            }
            UserData.Password.MakeReadOnly();
        }

        /// <summary>
        /// Gets the version.
        /// </summary>
        /// <value>
        /// The version.
        /// </value>
        public ExchangeVersion Version { get { return ExchangeVersion.Exchange2013;} }

        public string EmailAddress
        {
            get;
            private set;
        }

        public SecureString Password
        {
            get;
            private set;
        }

        public Uri AutodiscoverUrl
        {
            get;
            set;
        }
    }
}
