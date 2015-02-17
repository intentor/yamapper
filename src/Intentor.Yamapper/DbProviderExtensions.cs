/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Data;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Contém métodos para apoio na utilização do DbProvider.
    /// </summary>
    public static class DbProviderExtensions {
        #region Métodos

        #region Private

        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="onlyAddIfIsNotNull">Apenas adiciona o parâmetro caso seu valor não seja  <see langword="null"/> ou <see cref="String.Empty"/>.</param>
        /// <param name="direction">Direção do parâmetro.</param>
        /// <param name="type">Tipo de dados do parâmetro.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        private static IDbCommand CreateParameterInDbCommand(ref IDbCommand cmd
            , string name
            , object value
            , bool onlyAddIfIsNotNull
            , DbType? type
            , ParameterDirection direction) {
            if (onlyAddIfIsNotNull &&
                (value == null || value == DBNull.Value || value == String.Empty)) return cmd;

            IDbDataParameter param = cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = (value.IsNullOrDbNull() ? DBNull.Value : value);
            if (type.HasValue) param.DbType = type.Value;
            param.Direction = direction;

            cmd.Parameters.Add(param);

            return cmd;
        }

        #endregion

        #region Public

        #region CreateParameter

        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        public static IDbCommand CreateParameter(this IDbCommand cmd
            , string name
            , object value) {
            return CreateParameterInDbCommand(ref cmd
                , name
                , value
                , false
                , null
                , ParameterDirection.Input);
        }


        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="onlyAddIfIsNotNull">Apenas adiciona o parâmetro caso esteja não seja <see langword="null"/> ou <see cref="String.Empty"/>.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        public static IDbCommand CreateParameter(this IDbCommand cmd
            , string name
            , object value
            , bool onlyAddIfIsNotNull) {
            return CreateParameterInDbCommand(ref cmd
                , name
                , value
                , onlyAddIfIsNotNull
                , null
                , ParameterDirection.Input);
        }


        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="direction">Direção do parâmetro.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        public static IDbCommand CreateParameter(this IDbCommand cmd
            , string name
            , object value
            , ParameterDirection direction) {
            return CreateParameterInDbCommand(ref cmd
                , name
                , value
                , false
                , null
                , direction);
        }

        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="type">Tipo de dados do parâmetro.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        public static IDbCommand CreateParameter(this IDbCommand cmd
            , string name
            , object value
            , DbType type) {
            return CreateParameterInDbCommand(ref cmd
                , name
                , value
                , false
                , type
                , ParameterDirection.Input);
        }

        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="onlyAddIfIsNotNull">Apenas adiciona o parâmetro caso esteja não seja <see langword="null"/> ou <see cref="String.Empty"/>.</param>
        /// <param name="direction">Direção do parâmetro.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        public static IDbCommand CreateParameter(this IDbCommand cmd
            , string name
            , object value
            , bool onlyAddIfIsNotNull
            , ParameterDirection direction) {
            return CreateParameterInDbCommand(ref cmd
                , name
                , value
                , onlyAddIfIsNotNull
                , null
                , direction);
        }

        /// <summary>
        /// Insere um novo parâmetro.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> no qual o parâmetro será inserido.</param>
        /// <param name="name">Nome do parâmetro.</param>
        /// <param name="value">Valor do parâmetro.</param>
        /// <param name="onlyAddIfIsNotNull">Apenas adiciona o parâmetro caso esteja não seja <see langword="null"/> ou <see cref="String.Empty"/>.</param>
        /// <param name="direction">Direção do parâmetro.</param>
        /// <param name="type">Tipo de dados do parâmetro.</param>
        /// <returns>Objeto <see cref="IDbCommand"/>.</returns>
        public static IDbCommand CreateParameter(this IDbCommand cmd
            , string name
            , object value
            , bool onlyAddIfIsNotNull
            , ParameterDirection direction
            , DbType type) {
            return CreateParameterInDbCommand(ref cmd
                , name
                , value
                , onlyAddIfIsNotNull
                , type
                , direction);
        }

        #endregion

        #endregion

        #endregion
    }
}
