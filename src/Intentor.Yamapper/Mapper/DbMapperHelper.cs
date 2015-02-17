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
using System.Reflection;
using Intentor.Utilities;
using Intentor.Yamapper.Proxy;

namespace Intentor.Yamapper {
    /// <summary>
    /// Apoio no mapeamento de objeto-relacional.
    /// </summary>
    internal static class DbMapperHelper {
        #region Campos

        /// <summary>
        /// Classes mapeadas.
        /// </summary>
        private static TableMappingCollection _mappings;

        /// <summary>
        /// Indica se o mapeamento já foi realizado.
        /// </summary>
        private static bool _isMapped;

        #endregion

        #region Propriedades

        /// <summary>
        /// Tabelas mapeadas.
        /// </summary>
        public static TableMappingCollection Mappings {
            get { return _mappings; }
            set { _mappings = value; }
        }

        /// <summary>
        /// Identifica se já foi realizado o mapeamento das tabelas.
        /// </summary>
        public static bool IsMapped {
            get { return _isMapped; }
            set { _isMapped = value; }
        }

        #endregion

        #region Métodos

        #region Private

        /// <summary>
        /// Cria instância de um objeto.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto.</typeparam>
        /// <param name="tb">Mapeamento do objeto.</param>
        /// <returns>Intância do objeto.</returns>
        private static T CreateInstance<T>(TableMapping tb)
            where T : class {
            //Sendo LazyLoading, cria a entidade como Proxy.
            if (!tb.IsView && tb.IsLazyLoading) {
                T obj = ProxyHelper.CreateProxyForEntity<T>();
                return obj;
            } else {
                return Activator.CreateInstance<T>();
            }
        }

        #endregion

        #region Public

        /// <summary>
        /// Cria mapeamento das entidades de banco de dados.
        /// </summary>
        public static void BuildMappings() {
            if (_mappings.IsNullOrDbNull()) {
                lock (typeof(DbMapperHelper)) {
                    try {
                        _mappings = new TableMappingCollection();

                        //Realiza mapeamento por Assembly.
                        if (!String.IsNullOrEmpty(YamapperConfigurations.MappingAssembly))
                            _mappings = new BuildMappingFromAssembly(YamapperConfigurations.MappingAssembly
                                , _mappings).Build();

                        //Realiza mapeamento por arquivos de configuração.
                        if (!String.IsNullOrEmpty(YamapperConfigurations.MappingsFolder))
                            _mappings = new BuildMappingFromFile(YamapperConfigurations.MappingsFolder
                                , _mappings).Build();

                        //Identifica que já houve o mapeamento.
                        _isMapped = true;
                    } catch (Exception ex) {
                        _mappings = null;
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// Obtém o valor de uma propriedade de uma entidade.
        /// </summary>
        /// <param name="entity">Entidade a ter o valor de propriedade obtido.</param>
        /// <param name="entityType">Tipo da entidade.</param>
        /// <param name="field">Campo a ter o valor de sua propriedade obtido.</param>
        /// <returns>Valor da propriedade.</returns>
        public static object GetPropertyValue(object entity, Type entityType, FieldMapping field) {
            object value = entityType.GetProperty(field.PropertyName).GetValue(entity, null);
            if (value.IsNullOrDbNull() || value.Equals(String.Empty)) value = DBNull.Value;

            return value;
        }

        /// <summary>
        /// Define o valor de uma propriedade de uma entidade.
        /// </summary>
        /// <param name="entity">Entidade a ter o valor de propriedade definido.</param>
        /// <param name="entityType">Tipo da entidade.</param>
        /// <param name="field">Campo a ter o valor de sua propriedade definido.</param>
        /// <param name="value">Valor a ser definido.</param>
        public static void SetPropertyValue(object entity, Type entityType, FieldMapping field, object value) {
            entityType.GetProperty(field.PropertyName).SetValue(entity, value, null);
        }

        /// <summary>
        /// Mapeia um <see cref="DataRow"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <remarks>Supõe-se que todos os campos definidos nos mapeamentos existam no objeto informado.</remarks>
        /// <returns>Objeto <see typeparamref="T"/>.</returns>
        public static T GetFromDataRowUsingPropertyName<T>(DataRow row)
            where T : class {
            Type objType = typeof(T);

            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in row.Table.Columns) {
                //Verifica se o valor do campo não é nulo.
                if (!row[column.ColumnName].IsNullOrDbNull()) {
                    //Verifica se a propriedade existe.
                    if (objType.GetProperties().FirstOrDefault(property => property.Name.Equals(column.ColumnName)) != null) {
                        objType.GetProperty(column.ColumnName).SetValue(obj, row[column.ColumnName], null);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Mapeia um <see cref="DataRow"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <remarks>Supõe-se que todos os campos definidos nos mapeamentos existam no objeto informado.</remarks>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static T GetFromDataReaderUsingPropertyName<T>(IDataReader row)
            where T : class {
            Type objType = typeof(T);

            T obj = Activator.CreateInstance<T>();

            for (int i = 0; i < row.FieldCount; i++) {
                //Verifica se o valor do campo não é nulo.
                if (!row[i].IsNullOrDbNull()) {
                    //Verifica se a propriedade existe.
                    if (objType.GetProperties().FirstOrDefault(property => property.Name.Equals(row.GetName(i))) != null) {
                        objType.GetProperty(row.GetName(i)).SetValue(obj, row[i], null);
                    }
                }
            }

            return obj;
        }

        /// <summary>
        /// Mapeia um <see cref="DataRow"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <remarks>Supõe-se que todos os campos definidos nos mapeamentos existam no objeto informado.</remarks>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static T GetFromDataRow<T>(DataRow row)
            where T : class {
            Check.Require(_isMapped, Messages.EntitiesNotMapped);

            Type objType = typeof(T);
            TableMapping tb = _mappings[objType.FullName];
            T obj = CreateInstance<T>(tb);

            //Obtém os campos para carregamento. Caso seja LazyLoading, obtém apenas as PKs.
            List<FieldMapping> fields = (!tb.IsView && tb.IsLazyLoading ? tb.GetPrimaryKeys() : tb.Fields);

            foreach (FieldMapping field in fields) {
                if (!row[field.FieldName].IsNullOrDbNull())
                    objType.GetProperty(field.PropertyName).SetValue(obj, row[field.FieldName], null);
            }

            return obj;
        }

        /// <summary>
        /// Mapeia um <see cref="DataRow"/> para um objeto <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="row">Objeto <see cref="DataRow"/> a ser mapeado.</param>
        /// <remarks>Supõe-se que todos os campos definidos nos mapeamentos existam no objeto informado.</remarks>
        /// <returns>Objeto <typeparamref name="T"/>.</returns>
        public static T GetFromDataReader<T>(IDataReader row)
            where T : class {
            Check.Require(_isMapped, Messages.EntitiesNotMapped);

            Type objType = typeof(T);
            TableMapping tb = _mappings[objType.FullName];
            T obj = CreateInstance<T>(tb);

            //Obtém os campos para carregamento. Caso seja LazyLoading, obtém apenas as PKs.
            List<FieldMapping> fields = (!tb.IsView && tb.IsLazyLoading ? tb.GetPrimaryKeys() : tb.Fields);

            foreach (FieldMapping field in fields) {
                if (!row[field.FieldName].IsNullOrDbNull())
                    objType.GetProperty(field.PropertyName).SetValue(obj, row[field.FieldName], null);
            }

            return obj;
        }

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para um DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO para o qual a entidade será copiada.</typeparam>
        /// <param name="obj">Objeto da entidade.</param>
        public static T BindTo<T>(object obj)
            where T : class {
            if (obj == null) return null;

            //Obtém os tipos dos objetos.
            Type sourceType = obj.GetType();
            Type bindToType = typeof(T);

            //Se o objeto não for uma classe, retorna nulo.
            if (!sourceType.IsClass) return null;

            //Cria instância do objeto destino.
            T item = Activator.CreateInstance<T>();

            //Popula as propriedades do objeto destino, desde que existam no objeto origem.
            foreach (PropertyInfo property in bindToType.GetProperties()) {
                //Verifica se a propriedade permite escrita de dados (é pública e contém SET).
                if (!property.CanWrite) continue;

                var sourceProperty = sourceType.GetProperties().FirstOrDefault(p => p.Name.Equals(property.Name));
                if (sourceProperty != null && sourceProperty.CanRead) {
                    object value = sourceProperty.GetValue(obj, null);
                    bindToType.GetProperty(property.Name).SetValue(item, value, null);
                }
            }

            return item;
        }

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para um DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO para o qual a entidade será copiada.</typeparam>
        /// <param name="obj">Objeto da entidade.</param>
        /// <param name="dto">DTO para o qual os dados serão copiados.</param>
        public static void BindTo<T>(this object obj, T dto)
            where T : class {
            //Obtém os tipos dos objetos.
            Type sourceType = obj.GetType();
            Type bindToType = typeof(T);

            //Se o objeto não for uma classe, sai do método.
            if (!sourceType.IsClass) return;

            //Popula as propriedades do objeto destino, desde que existam no objeto origem.
            foreach (PropertyInfo property in bindToType.GetProperties()) {
                //Verifica se a propriedade permite escrita de dados (é pública e contém SET).
                if (!property.CanWrite) continue;

                var sourceProperty = sourceType.GetProperties().FirstOrDefault(p => p.Name.Equals(property.Name));
                if (sourceProperty != null && sourceProperty.CanRead) {
                    object value = sourceProperty.GetValue(obj, null);
                    bindToType.GetProperty(property.Name).SetValue(dto, value, null);
                }
            }
        }

        /// <summary>
        /// Realiza cópia dos valores de uma entidade para um DTO.
        /// </summary>
        /// <typeparam name="T">Tipo do DTO para o qual a entidade será copiada.</typeparam>
        /// <typeparam name="E">Tipo da entidade.</typeparam>
        /// <param name="list">Lista de objetos da entidade.</param>
        /// <returns>Lista do objeto a ser </returns>
        public static List<T> BindTo<T, E>(List<E> list)
            where T : class
            where E : class {
            if (list == null) return null;

            var binded = new List<T>(list.Count);

            foreach (E obj in list) {
                binded.Add(obj.BindTo<T>());
            }

            return binded;
        }

        #endregion

        #endregion
    }
}
