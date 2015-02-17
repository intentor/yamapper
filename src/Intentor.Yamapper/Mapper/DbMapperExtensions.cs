/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Data;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Métodos para mapeamento objeto-relacional.
    /// </summary>
    public static class DbMapperExtensions {
        #region Métodos

        #region MapTo

        /// <summary>
        /// Mapeia um <see cref="DataRow"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static T MapTo<T>(this DataRow row)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            return DbMapperHelper.GetFromDataRow<T>(row);
        }

        /// <summary>
        /// Mapeia um <see cref="DataTable"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static List<T> MapTo<T>(this DataTable dt)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            List<T> list = new List<T>(dt.Rows.Count);

            using (dt) {
                foreach (DataRow drw in dt.Rows) {
                    list.Add(DbMapperHelper.GetFromDataRow<T>(drw));
                }
            }

            return list;
        }

        /// <summary>
        /// Mapeia um <see cref="IDataReader"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static List<T> MapTo<T>(this IDataReader dr)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            List<T> list = new List<T>();

            using (dr) {
                while (dr.Read()) {
                    list.Add(DbMapperHelper.GetFromDataReader<T>(dr));
                }
            }

            return list;
        }

        #endregion

        #region MapByPropertyNameTo

        /// <summary>
        /// Mapeia um <see cref="DataRow"/> para um objeto <typeparamref name="T"/> a partir dos nomes de suas propriedades.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static T MapByPropertyNameTo<T>(this DataRow row)
            where T : class {
            return DbMapperHelper.GetFromDataRowUsingPropertyName<T>(row);
        }

        /// <summary>
        /// Mapeia um <see cref="DataTable"/> para um objeto <typeparamref name="T"/> a partir dos nomes de suas propriedades.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static List<T> MapByPropertyNameTo<T>(this DataTable dt)
            where T : class {
            List<T> list = new List<T>(dt.Rows.Count);

            using (dt) {
                foreach (DataRow drw in dt.Rows) {
                    list.Add(DbMapperHelper.GetFromDataRowUsingPropertyName<T>(drw));
                }
            }

            return list;
        }

        /// <summary>
        /// Mapeia um <see cref="IDataReader"/> para um objeto <typeparamref name="T"/> a partir dos nomes de suas propriedades.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static List<T> MapByPropertyNameTo<T>(this IDataReader dr)
            where T : class {
            List<T> list = new List<T>();

            using (dr) {
                while (dr.Read()) {
                    list.Add(DbMapperHelper.GetFromDataReaderUsingPropertyName<T>(dr));
                }
            }

            return list;
        }

        #endregion

        #region BindTo

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para um DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO para o qual a entidade será copiada.</typeparam>
        /// <param name="obj">Objeto da entidade.</param>
        public static T BindTo<T>(this object obj)
            where T : class {
            return DbMapperHelper.BindTo<T>(obj);
        }

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para um DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO para o qual a entidade será copiada.</typeparam>
        /// <param name="obj">Objeto da entidade.</param>
        /// <param name="dto">DTO para o qual os dados serão copiados.</param>
        public static void BindTo<T>(this object obj, T dto)
            where T : class {
            DbMapperHelper.BindTo<T>(obj, dto);
        }

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para outra do mesmo tipo.
        /// </summary>
        /// <typeparam name="T">Tipo da entidade.</typeparam>
        /// <param name="list">Lista de objetos da entidade.</param>
        /// <returns>Lista de objetos copiados.</returns>
        /// <remarks>Este procedimento é recomendado para evitar problemas de DataBind com objetos criados via LazyLoading.</remarks>
        public static List<T> BindTo<T>(this List<T> list)
            where T : class {
            return DbMapperHelper.BindTo<T, T>(list);
        }

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para um DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO para o qual a entidade será copiada.</typeparam>
        /// <typeparam name="E">Tipo da entidade.</typeparam>
        /// <param name="list">Lista de objetos da entidade.</param>
        /// <remarks>Este procedimento é recomendado para evitar problemas de DataBind com objetos criados via LazyLoading.</remarks>
        public static List<T> BindTo<T, E>(this List<E> list)
            where T : class
            where E : class {
            return DbMapperHelper.BindTo<T, E>(list);
        }

        #endregion

        #endregion
    }
}
