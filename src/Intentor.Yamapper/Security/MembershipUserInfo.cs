/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
 
Parcialmente baseado em
NHCustomProviders.MembershipUserInfo
Copyright © 2007-2008 Manuel Abadia
http://www.manuelabadia.com/
*********************************************/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Security;
using Intentor.Yamapper;
using Intentor.Utilities;

namespace Intentor.Yamapper.Security {
    /// <summary>Detalhes de membership do usuário.</summary>
    [Serializable]
    public class MembershipUserInfo {
        /// <summary>ID do usuário.</summary>
        public virtual int UserId { get; set; }

        /// <summary>Nome do usuário.</summary>
        public virtual string Username { get; set; }

        /// <summary>E-mail do usuário.</summary>
        public virtual string Email { get; set; }

        /// <summary>Senha do usuário.</summary>
        public virtual string Password { get; set; }

        /// <summary>Formato da senha do usuário. 0 - Clear; 1 - Hashed; 2 - Encrypted.</summary>
        public virtual short PasswordFormat { get; set; }

        /// <summary>Pergunta para recuperação de senha.</summary>
        public virtual string PasswordQuestion { get; set; }

        /// <summary>Resposta da pergunta para recuperação de senha.</summary>
        public virtual string PasswordAnswer { get; set; }

        /// <summary>Contador de tentativas de falha de validação de senha.</summary>
        public virtual short FailedPasswordAttemptCount { get; set; }

        /// <summary>Data e hora da última falha de validação de senha.</summary>
        public virtual DateTime? FailedPasswordAttemptWindowStart { get; set; }

        /// <summary>Contador de tentativas de falha de validação de resposta de pergunta.</summary>
        public virtual short FailedPasswordAnswerAttemptCount { get; set; }

        /// <summary>Data e hora da última falha de validação de resposta de pergunta.</summary>
        public virtual DateTime? FailedPasswordAnswerAttemptWindowStart { get; set; }

        /// <summary>Data e hora da última troca de senha.</summary>
        public virtual DateTime? LastPasswordChangedDate { get; set; }

        /// <summary>Indica se o usuário está aprovado.</summary>
        public virtual bool IsApproved { get; set; }

        /// <summary>Indica se o usuário está bloqueado para acesso.</summary>
        public virtual bool IsLockedOut { get; set; }

        /// <summary>Data e hora de criação do usuário.</summary>
        public virtual DateTime CreationDate { get; set; }

        /// <summary>Data e hora da última atividade realizada pelo usuário.</summary>
        public virtual DateTime LastActivityDate { get; set; }

        /// <summary>Data e hora do último travamento do usuário.</summary>
        public virtual DateTime? LastLockOutDate { get; set; }

        /// <summary>Data e hora do último login do usuário.</summary>
        public virtual DateTime? LastLoginDate { get; set; }

        /// <summary>Comentários sobre o usuário.</summary>
        public virtual string Comments { get; set; }

        #region Métodos

        /// <summary>Modifica a instância atual a partir de um objeto <see cref="MembershipUser"/>.</summary>
        /// <param name="user">Objeto <see cref="MembershipUser"/> com os dados a serem utilizados.</param>
        public void ModifyFromMembershipUser(MembershipUser user) {
            this.Username = user.UserName;
            this.UserId = user.ProviderUserKey.Parse<int>();
            this.Email = user.Email;
            this.PasswordQuestion = user.PasswordQuestion;
            this.Comments = user.Comment;
            this.IsApproved = user.IsApproved;
            this.IsLockedOut = user.IsLockedOut;
            this.CreationDate = user.CreationDate;
            this.LastLoginDate = user.LastLoginDate;
            this.LastActivityDate = user.LastActivityDate;
            this.LastPasswordChangedDate = user.LastPasswordChangedDate;
            this.LastLockOutDate = user.LastLockoutDate;
        }

        /// <summary>Obtém um MembershipUser com os dados de MembershipUserInfo.</summary>
        /// <returns>Objeto <see cref="MembershipUser"/>.</returns>
        public MembershipUser GetMembershipUser() {
            return new MembershipUser("YamapperMembershipProvider"
                                        , this.Username
                                        , this.UserId
                                        , this.Email
                                        , this.PasswordQuestion
                                        , this.Comments
                                        , this.IsApproved
                                        , this.IsLockedOut
                                        , this.CreationDate
                                        , this.LastLoginDate.GetValueOrDefault()
                                        , this.LastActivityDate
                                        , this.LastPasswordChangedDate.GetValueOrDefault()
                                        , this.LastLockOutDate.GetValueOrDefault());
        }

        /// <summary>Verifica se um System.Object informado é igual ao System.Object atual.</summary>
        /// <param name="obj">Objeto a ser comparado ao objeto atual.</param>
        /// <returns>Valor booleano indicando a igualdade.</returns>
        public override bool Equals(object obj) {
            if (ReferenceEquals(this, obj)) return true;

            MembershipUserInfo user = obj as MembershipUserInfo;
            if (user == null) return false;

            if (this.Username != user.Username) return false;

            return true;
        }

        /// <summary>Obtém o hashcode do objeto.</summary>
        /// <returns>Hashcode do objeto.</returns>
        public override int GetHashCode() {
            int result = 0;
            if (!this.Username.IsNullOrEmpty()) result += this.Username.GetHashCode();
            return result;
        }

        /// <summary>String que representa o objeto atual.</summary>
        /// <returns>String que representa o objeto atual.</returns>
        public override string ToString() {
            return String.Format(CultureInfo.CurrentCulture, "[{0}] {1}", (this.UserId != null) ? UserId.ToString() : "null", this.Username ?? "null");
        }

        #endregion
    }
}