/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
 
Parcialmente baseado em
NHCustomProviders.NHCustomMembershipProvider
Copyright © 2007-2008 Manuel Abadia
http://www.manuelabadia.com/
*********************************************/

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.Web;
using System.Web.Hosting;
using System.Web.Security;
using Intentor.Utilities;
using Intentor.Yamapper;

namespace Intentor.Yamapper.Security {
    /// <summary>Membership Provider do Yamapper.</summary>
    public class YamapperMembershipProvider : MembershipProvider {
        #region Campos

        private string _applicationName;
        private string _connectionName;
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;
        private MembershipPasswordFormat _passwordFormat;
        private string _passwordStrengthRegularExpression;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail;

        #endregion

        #region Propriedades

        /// <summary>Nome da aplicação utilizando o provedor.</summary>
        /// <remarks>Esta propriedade não é persistida no banco de dados, sendo mantida apenas por questões de compatibilidade.patibility reasons.</remarks>
        public override string ApplicationName {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        /// <summary>Nome da conexão do Yamapper de acesso ao banco de dados.</summary>
        public string ConnectionName {
            get { return _connectionName; }
        }

        /// <summary>Indicates whether the membership provider is configured to allow users to reset their passwords.</summary>
        /// <returns>true if the membership provider supports password reset; otherwise, false. The default is true.</returns>
        public override bool EnablePasswordReset {
            get { return _enablePasswordReset; }
        }

        /// <summary>Indicates whether the membership provider is configured to allow users to retrieve their passwords.</summary>
        /// <returns>true if the membership provider is configured to support password retrieval; otherwise, false. The default is false.</returns>
        public override bool EnablePasswordRetrieval {
            get { return _enablePasswordRetrieval; }
        }

        /// <summary>Gets the number of invalid password or password-answer attempts allowed before the membership user is locked out.</summary>
        /// <returns>The number of invalid password or password-answer attempts allowed before the membership user is locked out.</returns>
        public override int MaxInvalidPasswordAttempts {
            get { return _maxInvalidPasswordAttempts; }
        }

        /// <summary>Gets the minimum number of special characters that must be present in a valid password.</summary>
        /// <returns>The minimum number of special characters that must be present in a valid password.</returns>
        public override int MinRequiredNonAlphanumericCharacters {
            get { return _minRequiredNonAlphanumericCharacters; }
        }

        /// <summary>Gets the minimum length required for a password.</summary>
        /// <returns>The minimum length required for a password. </returns>
        public override int MinRequiredPasswordLength {
            get { return _minRequiredPasswordLength; }
        }

        /// <summary>Gets the number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</summary>
        /// <returns>The number of minutes in which a maximum number of invalid password or password-answer attempts are allowed before the membership user is locked out.</returns>
        public override int PasswordAttemptWindow {
            get { return _passwordAttemptWindow; }
        }

        /// <summary>Gets a value indicating the format for storing passwords in the membership data store.</summary>
        /// <returns>One of the MembershipPasswordFormat values indicating the format for storing passwords in the data store.</returns>
        public override MembershipPasswordFormat PasswordFormat {
            get { return _passwordFormat; }
        }

        /// <summary>Gets the regular expression used to evaluate a password.</summary>
        /// <returns>A regular expression used to evaluate a password.</returns>
        public override string PasswordStrengthRegularExpression {
            get { return _passwordStrengthRegularExpression; }
        }

        /// <summary>Gets a value indicating whether the membership provider is configured to require the user to answer a password question for password reset and retrieval.</summary>
        /// <returns>true if a password answer is required for password reset and retrieval; otherwise, false. The default is true.</returns>
        public override bool RequiresQuestionAndAnswer {
            get { return _requiresQuestionAndAnswer; }
        }

        /// <summary>Gets a value indicating whether the membership provider is configured to require a unique e-mail address for each user name.</summary>
        /// <returns>true if the membership provider requires a unique e-mail address; otherwise, false. The default is true.</returns>
        public override bool RequiresUniqueEmail {
            get { return _requiresUniqueEmail; }
        }

        #endregion

        #region Métodos

        #region Herdados

        /// <summary>Initializes the provider.</summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        public override void Initialize(string name, NameValueCollection config) {
            if (config.IsNullOrDbNull()) throw new ArgumentNullException("config");
            if (name.IsNullOrEmpty()) name = "YamapperMembershipProvider";
            if (config["description"].IsNullOrEmpty()) config.Add("description", "Yamapper Custom Membership Provider");

            base.Initialize(name, config);

            //COnfigurações específicas do YamapperMembershipProvider.
            _connectionName = this.GetConfigValue(config["connectionName"], String.Empty);

            //Configurações padrão do MembershipProvider.
            _applicationName = this.GetConfigValue(config["applicationName"], HostingEnvironment.ApplicationVirtualPath);
            _enablePasswordReset = this.GetConfigValue<bool>(config["enablePasswordReset"], true);
            _enablePasswordRetrieval = this.GetConfigValue<bool>(config["enablePasswordRetrieval"], false);
            _maxInvalidPasswordAttempts = this.GetConfigValue<int>(config["maxInvalidPasswordAttempts"], 3);
            _minRequiredNonAlphanumericCharacters = this.GetConfigValue<int>(config["minRequiredNonalphanumericCharacters"], 1);
            _minRequiredPasswordLength = this.GetConfigValue<int>(config["minRequiredPasswordLength"], 6);
            _passwordAttemptWindow = this.GetConfigValue<int>(config["passwordAttemptWindow"], 10);
            _passwordFormat = (MembershipPasswordFormat)this.GetConfigValue<int>(config["passwordFormat"], (int)MembershipPasswordFormat.Clear);
            _passwordStrengthRegularExpression = this.GetConfigValue(config["passwordStrengthRegularExpression"], String.Empty);
            _requiresQuestionAndAnswer = this.GetConfigValue<bool>(config["requiresQuestionAndAnswer"], false);
            _requiresUniqueEmail = this.GetConfigValue<bool>(config["requiresUniqueEmail"], false);
        }

        /// <summary>Processes a request to update the password for a membership user.</summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        public bool ChangePassword(string username, string newPassword) {
            if (!this.CheckPasswordPolicy(newPassword)) {
                return false;
            }

            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return false;
            else {
                var user = result[0];

                user.Password = this.EncodePassword(newPassword, _passwordFormat);
                user.LastPasswordChangedDate = DateTime.Now;

                //Obtém o provedor de acesso ao banco de dados.
                DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(user);
            }

            return true;
        }

        /// <summary>Processes a request to update the password for a membership user.</summary>
        /// <param name="username">The user to update the password for.</param>
        /// <param name="oldPassword">The current password for the specified user.</param>
        /// <param name="newPassword">The new password for the specified user.</param>
        /// <returns>true if the password was updated successfully; otherwise, false.</returns>
        public override bool ChangePassword(string username, string oldPassword, string newPassword) {
            if (!this.CheckPasswordPolicy(newPassword)) {
                return false;
            }

            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return false;
            else {
                var user = result[0];

                if (user.Password != this.EncodePassword(oldPassword, _passwordFormat)) return false;
                else {
                    user.Password = this.EncodePassword(newPassword, _passwordFormat);
                    user.LastPasswordChangedDate = DateTime.Now;

                    //Obtém o provedor de acesso ao banco de dados.
                    DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(user);
                }
            }

            return true;
        }

        /// <summary>Processes a request to update the password question and answer for a membership user.</summary>
        /// <param name="username">The user to change the password question and answer for.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <param name="newPasswordQuestion">The new password question for the specified user.</param>
        /// <param name="newPasswordAnswer">The new password answer for the specified user.</param>
        /// <returns>true if the password question and answer are updated successfully; otherwise, false.</returns>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return false;
            else {
                var user = result[0];

                if (user.Password != password) return false;
                else {
                    user.PasswordQuestion = newPasswordQuestion;
                    user.PasswordAnswer = this.EncodePassword(newPasswordAnswer, _passwordFormat);
                    DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(user);
                }
            }

            return true;
        }

        /// <summary>Adds a new membership user to the data source.</summary>
        /// <remarks>As this provider can work on an existing users table, the application must create the users
        /// directly if the data in the users table that is not part of the membership data can't be null.
        /// To create users directly, use the ValidateUserCreation method to get the required data 
        /// and perform the necessary checks.</remarks>
        /// <param name="username">The user name for the new user.</param>
        /// <param name="password">The password for the new user.</param>
        /// <param name="email">The e-mail address for the new user.</param>
        /// <param name="passwordQuestion">The password question for the new user.</param>
        /// <param name="passwordAnswer">The password answer for the new user</param>
        /// <param name="isApproved">Whether or not the new user is approved to be validated.</param>
        /// <param name="providerUserKey">The unique identifier from the membership data source for the user.</param>
        /// <param name="status">A MembershipCreateStatus enumeration value indicating whether the user was created successfully.</param>
        /// <returns>A MembershipUser object populated with the information for the newly created user.</returns>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
            #region Validações

            //TODO: validações de tamanhos de campos e requiresUniqueEmail.     

            if (!this.CheckPasswordPolicy(password)) {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (_requiresQuestionAndAnswer && passwordQuestion.IsNullOrDbNull()) {
                status = MembershipCreateStatus.InvalidQuestion;
                return null;
            }

            if (_requiresQuestionAndAnswer && passwordAnswer.IsNullOrDbNull()) {
                status = MembershipCreateStatus.InvalidAnswer;
                return null;
            }

            #endregion

            var user = new MembershipUserInfo();
            user.Username = username;
            user.Email = email;
            user.Password = this.EncodePassword(password, _passwordFormat);
            user.PasswordFormat = (short)_passwordFormat;
            user.IsApproved = isApproved;
            user.CreationDate = user.LastActivityDate = DateTime.Now;

            if (this.RequiresQuestionAndAnswer) {
                user.PasswordQuestion = passwordQuestion;
                user.PasswordAnswer = this.EncodePassword(passwordAnswer, _passwordFormat);
            }

            DbProviderFactory.GetProvider(_connectionName).Insert<MembershipUserInfo>(user);

            status = MembershipCreateStatus.Success;
            return this.GetUser(user.UserId, true);
        }

        /// <summary>Removes a user from the membership data source.</summary>
        /// <param name="username">The name of the user to delete.</param>
        /// <param name="deleteAllRelatedData">true to delete data related to the user from the database; false to leave data related to the user in the database.</param>
        /// <returns>true if the user was successfully deleted; otherwise, false.</returns>
        public override bool DeleteUser(string username, bool deleteAllRelatedData) {
            var count = DbProviderFactory.GetProvider(_connectionName).Delete<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));
            return (count > 0);
        }

        /// <summary>Gets a collection of membership users where the e-mail address contains the specified e-mail address to match.</summary>
        /// <param name="emailToMatch">The e-mail address to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
            var col = new MembershipUserCollection();
            var results = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create()
                                                            .Add(Expression.Equal("Email", emailToMatch))
                                                            .Offset(pageIndex * pageSize)
                                                            .Limit(pageSize));
            totalRecords = results.Count;

            foreach (var user in results) col.Add(user.GetMembershipUser());

            return col;
        }

        /// <summary>Gets a collection of membership users where the user name contains the specified user name to match.</summary>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
            var col = new MembershipUserCollection();
            var results = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create()
                                                            .Add(Expression.Equal("Username", usernameToMatch))
                                                            .Offset(pageIndex * pageSize)
                                                            .Limit(pageSize));
            totalRecords = results.Count;

            foreach (var user in results) col.Add(user.GetMembershipUser());

            return col;
        }

        /// <summary>Gets a collection of all the users in the data source in pages of data.</summary>
        /// <param name="pageIndex">The index of the page of results to return. pageIndex is zero-based.</param>
        /// <param name="pageSize">The size of the page of results to return.</param>
        /// <param name="totalRecords">The total number of matched users.</param>
        /// <returns>A MembershipUserCollection collection that contains a page of pageSize MembershipUser objects beginning at the page specified by pageIndex.</returns>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) {
            var col = new MembershipUserCollection();
            var results = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create()
                                                            .Offset(pageIndex * pageSize)
                                                            .Limit(pageSize));
            totalRecords = results.Count;

            foreach (var user in results) col.Add(user.GetMembershipUser());

            return col;
        }

        /// <summary>Gets the number of users currently accessing the application.</summary>
        /// <returns>The number of users currently accessing the application.</returns>
        public override int GetNumberOfUsersOnline() {
            var col = new MembershipUserCollection();
            /*Para saber se está online, considera usuários que tenham executado
             *alguma atividade nos últimos 10 minutos.*/
            var results = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create()
                                                            .Add(Expression.GreaterThan("LastActivityDate", DateTime.Now.AddMinutes(-10))));
            return results.Count;
        }

        /// <summary>Gets the password for the specified user name from the data source.</summary>
        /// <param name="username">The user to retrieve the password for.</param>
        /// <param name="answer">The password answer for the user.</param>
        /// <returns>The password for the specified user name.</returns>
        public override string GetPassword(string username, string answer) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return null;
            else return this.DecodePassword(result[0].Password, _passwordFormat);
        }

        /// <summary>Gets information from the data source for a user. Provides an option to update the last-activity date/time stamp for the user.</summary>
        /// <param name="username">The name of the user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A MembershipUser object populated with the specified user's information from the data source.</returns>
        public override MembershipUser GetUser(string username, bool userIsOnline) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return null;
            else return result[0].GetMembershipUser();
        }

        /// <summary>Gets information from the data source for a user based on the unique identifier for the membership user. Provides an option to update the last-activity date/time stamp for the user.</summary>
        /// <param name="providerUserKey">The unique identifier for the membership user to get information for.</param>
        /// <param name="userIsOnline">true to update the last-activity date/time stamp for the user; false to return user information without updating the last-activity date/time stamp for the user.</param>
        /// <returns>A MembershipUser object populated with the specified user's information from the data source or null if the user was not found.</returns>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("UserId", providerUserKey.Parse<int>())));

            if (result.Count == 0) return null;
            else return result[0].GetMembershipUser();
        }

        /// <summary>Gets the user name associated with the specified e-mail address.</summary>
        /// <param name="email">The e-mail address to search for.</param>
        /// <returns>The user name associated with the specified e-mail address. If no match is found, return null.</returns>
        public override string GetUserNameByEmail(string email) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Email", email)));

            if (result.Count == 0) return null;
            else return result[0].Username;
        }

        /// <summary>Resets a user's password to a new, automatically generated password.</summary>
        /// <param name="username">The user to reset the password for.</param>
        /// <param name="answer">The password answer for the specified user.</param>
        /// <returns>The new password for the specified user.</returns>
        public override string ResetPassword(string username, string answer) {
            if (!this.EnablePasswordReset) {
                throw new NotSupportedException("O provedor não está configurado para reset de senha.");
            }

            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return null;
            else {
                var user = result[0];
                var now = DateTime.Now;
                string newPassword = null;

                //Primeiramente, verifica se o usuário está bloqueado.
                if (user.IsLockedOut) {
                    throw new ProviderException("Conta bloqueada.");
                } else if (!user.FailedPasswordAnswerAttemptWindowStart.HasValue ||
                    user.FailedPasswordAnswerAttemptWindowStart.Value.AddMinutes(this.PasswordAttemptWindow) > now) {
                    //Verifica se a resposta está correta.
                    if (user.PasswordAnswer == this.EncodePassword(answer, _passwordFormat)) {
                        //TODO: melhorar geração de password.
                        newPassword = now.ToString("HHmmSS");
                        user.Password = this.EncodePassword(newPassword, _passwordFormat);
                        user.LastPasswordChangedDate = now;
                        user.FailedPasswordAnswerAttemptCount = 0;
                        user.FailedPasswordAnswerAttemptWindowStart = null;
                    } else if (this.RequiresQuestionAndAnswer) {
                        //Estando errada e sendo usado o sistema de questão/resposta, atualiza as tentativas.
                        user.FailedPasswordAnswerAttemptCount++;
                        user.FailedPasswordAnswerAttemptWindowStart = now;

                        if (user.FailedPasswordAnswerAttemptCount >= this.MaxInvalidPasswordAttempts) {
                            user.IsLockedOut = true;
                            user.LastLockOutDate = now;
                        }
                    }

                    DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(user);
                }

                return newPassword;
            }
        }

        /// <summary>Clears a lock so that the membership user can be validated.</summary>
        /// <param name="username">The membership user to clear the lock status for.</param>
        /// <returns>true if the membership user was successfully unlocked; otherwise, false.</returns>
        public override bool UnlockUser(string username) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return false;
            else {
                var user = result[0];
                user.IsLockedOut = false;
                user.FailedPasswordAttemptCount = 0;
                user.FailedPasswordAttemptWindowStart = null;
                user.FailedPasswordAnswerAttemptCount = 0;
                user.FailedPasswordAnswerAttemptWindowStart = null;
                DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(user);

                return true;
            }
        }

        /// <summary>Updates information about a user in the data source.</summary>
        /// <param name="user">A MembershipUser object that represents the user to update and the updated information for the user.</param>
        public override void UpdateUser(MembershipUser user) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("UserId", user.ProviderUserKey.Parse<int>())));

            if (result.Count > 0) {
                result[0].ModifyFromMembershipUser(user);
                DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(result[0]);
            }
        }

        /// <summary>Verifies that the specified user name and password exist in the data source.</summary>
        /// <param name="username">The name of the user to validate.</param>
        /// <param name="password">The password for the specified user.</param>
        /// <returns>true if the specified username and password are valid; otherwise, false.</returns>
        public override bool ValidateUser(string username, string password) {
            var result = DbProviderFactory.GetProvider(_connectionName).Get<MembershipUserInfo>(Criteria.Create().Add(Expression.Equal("Username", username)));

            if (result.Count == 0) return false;
            else {
                var user = result[0];
                var now = DateTime.Now;
                bool res = false;

                if (!user.IsLockedOut && (!user.FailedPasswordAttemptWindowStart.HasValue ||
                    user.FailedPasswordAttemptWindowStart.Value.AddMinutes(this.PasswordAttemptWindow) > now)) {
                    //Verifica se a senha está correta.
                    if (user.Password == this.EncodePassword(password, _passwordFormat)) {
                        user.LastLoginDate = now;
                        user.LastActivityDate = now;
                        user.FailedPasswordAttemptCount = 0;
                        user.FailedPasswordAttemptWindowStart = null;
                        res = true;
                    } else {
                        //Estando errada, atualiza as tentativas.
                        user.FailedPasswordAttemptCount++;
                        user.FailedPasswordAttemptWindowStart = now;

                        if (user.FailedPasswordAttemptCount >= this.MaxInvalidPasswordAttempts) {
                            user.IsLockedOut = true;
                            user.LastLockOutDate = now;
                            user.FailedPasswordAttemptWindowStart = null;
                        }
                    }

                    DbProviderFactory.GetProvider(_connectionName).Update<MembershipUserInfo>(user);
                }

                return res;
            }
        }

        #endregion

        #region Suporte

        /// <summary>Checks if the password meets the defined policy requirements.</summary>
        /// <param name="password">The new password.</param>
        /// <returns>true if the password pass the policy; false otherwise.</returns>
        public virtual bool CheckPasswordPolicy(string password) {
            // password length check
            if (password.Length < this.MinRequiredPasswordLength) {
                return false;
            }

            int nonAlphanumericCharacters = 0;
            foreach (char c in password) {
                if (!Char.IsLetterOrDigit(c)) {
                    nonAlphanumericCharacters++;
                }
            }

            // non alphanumerics check
            if (nonAlphanumericCharacters < this.MinRequiredNonAlphanumericCharacters) {
                return false;
            }

            // regular expression check
            if (!String.IsNullOrEmpty(this.PasswordStrengthRegularExpression)) {
                if (!Regex.IsMatch(password, this.PasswordStrengthRegularExpression)) {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Encodes the password based on the password format.</summary>
        /// <param name="password">The password to encode.</param>
        /// <param name="format">The password format to use.</param>
        /// <returns>The encoded password.</returns>
        public virtual string EncodePassword(string password, MembershipPasswordFormat format) {
            string encodedPassword;

            switch (format) {
                case MembershipPasswordFormat.Encrypted: {
                        encodedPassword = CryptoHelper.Encrypt(password);
                    }
                    break;

                case MembershipPasswordFormat.Hashed: {
                        encodedPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "sha1");
                    }
                    break;

                default: {
                        encodedPassword = password;
                    }
                    break;
            }

            return encodedPassword;
        }

        /// <summary>Decodes the password.</summary>
        /// <param name="password">The password to decode.</param>
        /// <param name="format">The password format used to encode it.</param>
        /// <returns>The decoded password.</returns>
        public virtual string DecodePassword(string password, MembershipPasswordFormat format) {

            string decodedPassword;

            switch (format) {
                case MembershipPasswordFormat.Encrypted: {
                        decodedPassword = CryptoHelper.Decrypt(password);
                    }
                    break;

                case MembershipPasswordFormat.Hashed: {
                        throw new ProviderException("Senhas em hash não podem ser obtidas.");
                    }

                default: {
                        decodedPassword = password;
                    }
                    break;
            }

            return decodedPassword;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private string GetConfigValue(string configValue, string defaultValue) {
            if (configValue.IsNullOrEmpty()) return defaultValue;
            else return configValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="configValue"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private T GetConfigValue<T>(string configValue, T defaultValue)
            where T : IConvertible {
            if (configValue.IsNullOrEmpty()) return defaultValue;
            else return configValue.Parse<T>();
        }

        #endregion

        #endregion
    }
}