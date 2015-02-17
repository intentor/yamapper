/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Intentor.Utilities;
using System.IO;

namespace Intentor.Yamapper {
    /// <summary>
    /// Realiza geração de objetos de mapeamento a partir de um assembly.
    /// </summary>
    internal class BuildMappingFromAssembly :
        IBuildMapping {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="assemblyName">Nome do assembly contendo as classes a serem mapeadas.</param>
        /// <param name="mappingCollection">Coleção de objetos já mapeados.</param>
        public BuildMappingFromAssembly(string assemblyName
            , TableMappingCollection mappingCollection) {
            _assemblyName = assemblyName;
            _mappingCollection = mappingCollection;
        }

        #endregion

        #region Campos

        /// <summary>
        /// Nome do assembly contendo as classes a  serem mapeadas.
        /// </summary>
        private string _assemblyName;

        /// <summary>
        /// Coleção de objetos já mapeados.
        /// </summary>
        private TableMappingCollection _mappingCollection;

        #endregion

        #region Propriedades

        /// <summary>   
        /// Nome do assembly contendo as classes a serem mapeadas.
        /// </summary>
        public string AssemblyName {
            get { return _assemblyName; }
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Verifica a existência do atributo <see cref="ColumnAttribute"/> em uma coluna da entidade de domínio.
        /// </summary>
        /// <param name="property">Objeto <see cref="PropertyInfo"/> que representa a propriedade.</param>
        /// <returns>Objeto <see cref="ColumnAttribute"/> ou <see langword="null"/> caso o atributo não tenha sido encontrado.</returns>
        public ColumnAttribute CheckColumnAttribute(PropertyInfo property) {
            //Verifica se a propriedade possui o attribute "ColumnAttribute".
            object[] atts = property.GetCustomAttributes(typeof(ColumnAttribute), false);

            //Cria um objeto "ColumnAttribute" inicializando-o como "null".
            ColumnAttribute att = null;

            //Caso haja o atributo, atribue-o a att.
            if (atts.Length == 1) {
                att = (ColumnAttribute)atts[0];
            }

            return att;
        }

        #endregion

        #region IBuildMapping Members

        public TableMappingCollection Build() {
            //Obtém o assembly a ser mapeado.
            Assembly ass = Assembly.Load(_assemblyName);

            var parser = new DbMapperParser();

            #region Resources

            //Verifica se o assembly possui resources do tipo ".cmf".
            string[] resources = ass.GetManifestResourceNames();

            foreach (var resource in resources) {
                if (resource.EndsWith(".cmf")) {
                    Stream sr = ass.GetManifestResourceStream(resource);
                    _mappingCollection.Add(parser.GetTableMappingFromFile(sr, resource));
                }
            }

            #endregion

            #region Classes

            /* Verifica quais das classes presentes no assembly possuem o atributo
             * "TableAttribute". Possui tal atributo, mapeia a classe e suas propriedades.*/
            foreach (Type t in ass.GetTypes()) {
                //Verifica se a classe possui o atributo "TableAttribute".
                object[] atts = t.GetCustomAttributes(typeof(TableAttribute), false);

                if (atts.Length == 1) {
                    string classFullQualifiedName = String.Concat(t.FullName + ", " + _assemblyName);
                    var table = parser.GetTableMapping(classFullQualifiedName, (TableAttribute)atts[0]);

                    //Mapeia as propriedades da classe.
                    foreach (PropertyInfo pi in t.GetProperties()) {
                        ColumnAttribute attField = this.CheckColumnAttribute(pi);
                        if (attField == null) continue; //Não possui o atributo.

                        table.Fields.Add(parser.GetFieldMapping(pi.Name, attField));
                    }

                    _mappingCollection.Add(table);
                }
            }

            #endregion

            return _mappingCollection;
        }

        #endregion
    }
}
