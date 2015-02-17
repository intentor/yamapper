/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Cria instruções SQL para acesso ao banco de dados.
    /// </summary>
    internal static class DbMapperQueryBuilder {
        /// <summary>
        /// Obtém a cláusula WHERE a partir de PKs.
        /// </summary>
        /// <param name="tb">Mapeamento da tabela que se deseja obter as PKs.</param>
        /// <param name="parameterPrefix">Prefixo dos parâmetros.</param>
        /// <returns>Cláusula WHERE do mapeamento, sem os valores.</returns>
        public static string GetWhereForPK(TableMapping tb, string parameterPrefix) {
            StringBuilder sql = new StringBuilder();
            var pks = tb.GetPrimaryKeys();

            Check.Require(pks.Count > 0, String.Format(Messages.PrimaryKeyNotFoundEntity, tb.EntityName));

            sql.Append(String.Format(CultureHelper.Invariant, " WHERE"));

            bool isFirstExecution = true;
            foreach (var pk in pks) {
                if (isFirstExecution) isFirstExecution = false;
                else sql.Append(" AND");
                sql.Append(String.Format(CultureHelper.Invariant, " {0} = {1}{2}", pk.FieldName, parameterPrefix, pk.PropertyName));
            }

            return sql.ToString();
        }

        /// <summary>
        /// Cria um SELECT para uma determinada propriedade de um mapeamento.
        /// </summary>
        /// <param name="tb">Mapeamento da tabela que se deseja obter as PKs.</param>
        /// <param name="propertyName">Nome da propriedade para obtenção do SELECT.</param>
        /// <param name="parameterPrefix">Prefixo dos parâmetros.</param>
        /// <returns>Cláusula SQL.</returns>
        public static string CreateSelectForProperty(TableMapping tb, string propertyName, string parameterPrefix) {
            string sql = String.Format("SELECT {0} FROM {1}{2}"
                , tb.Fields.First(field => field.PropertyName.Equals(propertyName)).FieldName
                , tb.TableName
                , GetWhereForPK(tb, parameterPrefix));

            return sql;
        }

        /// <summary>
        /// Cria uma instrução SELECT para uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade. </typeparam>
        /// <param name="parameterPrefix"> Prefixo dos parâmetros.</param>
        /// <returns>Instrução SQL a ser executada.</returns>
        public static string CreateSelectFor<T>(string parameterPrefix) {
            return CreateSelectFor<T>(parameterPrefix, false);
        }

        /// <summary>
        /// Cria uma instrução SELECT para uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <param name="parameterPrefix">Prefixo dos parâmetros.</param>
        /// <param name="includeWherePk">Identifica se se deve incluir um WHERE cujo parâmetro seja(m) a(s) primary key(s).</param>
        /// <remarks>Este procedimento somente funciona corretamente com UMA primary key.</remarks>
        /// <returns>Retorna a instrução SQL a ser executada.</returns>
        public static string CreateSelectFor<T>(string parameterPrefix, bool includeWherePk) {
            Type entity = typeof(T);
            TableMapping tb = DbMapperHelper.Mappings[entity.FullName];

            //Variável identificadora de colocação de vírgula na query SQL.
            bool isFirstExecution = true;
            StringBuilder sql = new StringBuilder();

            sql.Append("SELECT ");

            //Obtém os campos para carregamento. Caso seja LazyLoading, obtém apenas as PKs.
            List<FieldMapping> fields = (!tb.IsView && tb.IsLazyLoading ? tb.GetPrimaryKeys() : tb.Fields);

            foreach (FieldMapping field in fields) {
                if (isFirstExecution) isFirstExecution = false;
                else sql.Append(",");

                sql.Append(field.FieldName);
            }

            sql.Append(String.Format(CultureHelper.Invariant, " FROM {0} ", tb.TableName));
            if (includeWherePk) sql.Append(GetWhereForPK(tb, parameterPrefix));

            return sql.ToString();
        }

        /// <summary>
        /// Cria uma instrução SELECT COUNT para uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <returns>Instrução SQL a ser executada.</returns>
        public static string CreateSelectCountFor<T>() {
            Type entity = typeof(T);
            TableMapping tb = DbMapperHelper.Mappings[entity.FullName];

            String sql = String.Format(
                "SELECT COUNT(*) FROM {0}"
                , tb.TableName);

            return sql.ToString();
        }

        /// <summary>
        /// Cria uma instrução INSERT para uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <param name="parameterPrefix">Prefixo dos parâmetros.</param>
        /// <returns>Retorna a instrução SQL a ser executada.</returns>
        public static string CreateInsertFor<T>(string parameterPrefix) {
            Type entity = typeof(T);
            TableMapping tb = DbMapperHelper.Mappings[entity.FullName];

            //Variável identificadora de colocação de vírgula na query SQL.
            bool isFirstExecution = true;
            //Identifica a quantidade de parâmetros utilizados na instrução.
            int paramsCount = 0;
            StringBuilder sqlParams = new StringBuilder();
            StringBuilder sqlValues = new StringBuilder();

            sqlParams.Append(String.Concat("INSERT INTO ", tb.TableName, " ("));
            sqlValues.Append(" VALUES (");

            foreach (FieldMapping field in tb.Fields) {
                if (field.IsAutoKey || field.IgnoreOnInsert) continue;

                if (isFirstExecution) isFirstExecution = false;
                else {
                    sqlParams.Append(",");
                    sqlValues.Append(",");
                }

                paramsCount++;
                sqlParams.Append(field.FieldName);
                sqlValues.Append(String.Concat(parameterPrefix, String.Concat("p", paramsCount)));
            }

            sqlParams.Append(")");
            sqlValues.Append(")");

            return String.Concat(sqlParams, sqlValues);
        }

        /// <summary>
        /// Cria uma instrução UPDATE para uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <param name="parameterPrefix">Prefixo dos parâmetros.</param>
        /// <remarks>Este procedimento somente funciona corretamente com UMA primary key.</remarks>
        /// <returns>Retorna a instrução SQL a ser executada.</returns>
        public static string CreateUpdateFor<T>(string parameterPrefix) {
            Type entity = typeof(T);
            TableMapping tb = DbMapperHelper.Mappings[entity.FullName];

            //Variável identificadora de colocação de vírgula na query SQL.
            bool isFirstExecution = true;
            //Identifica a quantidade de parâmetros utilizados na instrução.
            int paramsCount = 0;

            StringBuilder sql = new StringBuilder();

            sql.Append(String.Concat("UPDATE ", tb.TableName, " SET "));

            foreach (FieldMapping field in tb.Fields) {
                if (field.IsPrimaryKey || field.IsAutoKey || field.IgnoreOnUpdate) continue;

                if (isFirstExecution) isFirstExecution = false;
                else sql.Append(",");

                paramsCount++;

                sql.Append(String.Format(CultureHelper.Invariant, "{0} = {1}{2}"
                    , field.FieldName
                    , parameterPrefix
                    , String.Concat("p", paramsCount)));
            }

            sql.Append(GetWhereForPK(tb, parameterPrefix));

            return sql.ToString();
        }

        /// <summary>
        /// Cria uma instrução DELETE para uma entidade.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <param name="parameterPrefix">Prefixo dos parâmetros.</param>
        /// <param name="usePkForDelete">Indica se se deve utilizar o valor da PK para exclusão.</param>
        /// <remarks>Este procedimento somente funciona corretamente com UMA primary key.</remarks>
        /// <returns>Retorna a instrução SQL a ser executada.</returns>
        public static string CreateDeleteFor<T>(string parameterPrefix, bool usePkForDelete) {
            Type entity = typeof(T);
            TableMapping tb = DbMapperHelper.Mappings[entity.FullName];
            StringBuilder sql = new StringBuilder();

            sql.Append(String.Format(CultureHelper.Invariant, "DELETE FROM {0}", tb.TableName));
            if (usePkForDelete) sql.Append(GetWhereForPK(tb, parameterPrefix));

            return sql.ToString();
        }
    }
}
