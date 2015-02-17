/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Realiza geração de objetos de mapeamento a partir de arquivos externos.
    /// </summary>
    internal class BuildMappingFromFile :
        IBuildMapping {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="mappingsFolder">Diretório contendo os arquivos .cmf.</param>
        /// <param name="mappingCollection">Coleção de objetos já mapeados.</param>
        public BuildMappingFromFile(string mappingsFolder
            , TableMappingCollection mappingCollection) {
            _mappingsFolder = mappingsFolder;
            _mappingCollection = mappingCollection;
        }

        #endregion

        #region Campos

        /// <summary>
        /// Diretório contendo os arquivos .cmf.
        /// </summary>
        private string _mappingsFolder;

        /// <summary>
        /// Coleção de objetos já mapeados.
        /// </summary>
        private TableMappingCollection _mappingCollection;

        #endregion

        #region Propriedades

        /// <summary>
        /// Diretório contendo os arquivos .cmf.
        /// </summary>
        public string MappingsFolder {
            get { return _mappingsFolder; }
        }

        #endregion

        #region IBuildMapping Members

        public TableMappingCollection Build() {
            //Obtém todos os arquivo .cmf existentes no caminho informado.
            string[] filePaths = Directory.GetFiles(_mappingsFolder, "*.cmf");
            var parser = new DbMapperParser();

            foreach (string filePath in filePaths) {
                var table = parser.GetTableMappingFromFile(filePath);
                if (table != null) _mappingCollection.Add(table);
            }

            return _mappingCollection;
        }

        #endregion
    }
}
