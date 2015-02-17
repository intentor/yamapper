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
using System.Xml;
using System.Reflection;
using System.Xml.Schema;
using System.IO;
using Intentor.Utilities;

namespace Intentor.Yamapper {
    /// <summary>
    /// Parser dos mapeamentos.
    /// </summary>
    internal class DbMapperParser {
        #region Campos

        /// <summary>
        /// Schema XML dos arquivos de mapeamento de tabelas.
        /// </summary>
        private const string XmlSchemaNamespace = "urn:yamapper-mapping-10.8.23.0";

        #endregion

        #region Métodos

        #region From File

        /// <summary>
        /// Obtém um mapeamento de tabela a paritr de um arquivo.
        /// </summary>
        /// <param name="filePath">Caminho do arquivo.</param>
        /// <returns>Object <see cref="TableMapping"/>.</returns>
        public TableMapping GetTableMappingFromFile(string filePath) {
            TableMapping tb;

            using (FileStream f = File.Open(filePath, FileMode.Open)) {
                var doc = this.LoadFile(f, f.Name);
                tb = this.GetTableMapping(doc, f.Name);
            }

            return tb;
        }

        /// <summary>
        /// Obtém um mapeamento de tabela a paritr de um arquivo.
        /// </summary>
        /// <param name="fileStream">Stream do arquivo.</param>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <returns>Object <see cref="TableMapping"/>.</returns>
        public TableMapping GetTableMappingFromFile(Stream fileStream, string fileName) {
            var doc = this.LoadFile(fileStream, fileName);
            return this.GetTableMapping(doc, fileName);
        }

        /// <summary>
        /// Valida e carrega o arquivo XML de configuração.
        /// </summary>
        /// <param name="fileStream">Stream do arquivo.</param>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <returns>Object <see cref="XmlDocument"/>.</returns>
        private XmlDocument LoadFile(Stream fileStream, String fileName) {
            XmlSchema mappingSchema =
                XmlSchema.Read(Assembly.GetExecutingAssembly().GetManifestResourceStream("Intentor.Yamapper.Schemas.yamapper-mapping.xsd"), null);

            XmlDocument doc = new XmlDocument();
            doc.Schemas.Add(mappingSchema);
            doc.Load(fileStream);
            doc.Validate((o, e) => {
                if (e.Severity == XmlSeverityType.Error) {
                    throw new FileValidationException(
                        String.Format(Messages.FileValidationExceptionSchema, Path.GetFileName(fileName), e.Message));
                }
            });

            return doc;
        }

        /// <summary>
        /// Obtém um mapeamento de classe a partir de um arquivo XML.
        /// </summary>
        /// <param name="doc">Arquivo XML de mapeamento.</param>
        /// <param name="fileName">Nome do arquivo.</param>
        /// <returns>Objeto <see cref="TableMapping"/>.</returns>
        private TableMapping GetTableMapping(XmlDocument doc, string fileName) {
            var nameSpaceManager = new XmlNamespaceManager(doc.NameTable);
            nameSpaceManager.AddNamespace("ymap", XmlSchemaNamespace);

            XmlNode classMapping = doc.SelectSingleNode("//ymap:class", nameSpaceManager);

            if (classMapping == null) {
                throw new FileValidationException(
                       String.Format(Messages.FileValidationExceptionNamespace, fileName, XmlSchemaNamespace));
            }

            XmlNodeList propertiesMapping = doc.SelectNodes("//ymap:property", nameSpaceManager);

            #region TableMapping

            TableMapping table = new TableMapping();
            table.EntityName = classMapping.Attributes["name"].Value;
            table.IsView = classMapping.Attributes["isView"].Value.Parse<bool>();

            if (classMapping.Attributes["useClassNameAsMemberName"].Value.Parse<bool>())
                table.TableName = classMapping.Attributes["name"].Value;
            else
                table.TableName = classMapping.Attributes["table"].Value;

            table.IsLazyLoading = classMapping.Attributes["lazyLoading"].Value.Parse<bool>();
            table.ConnectionName = classMapping.Attributes["connectionName"].Value;

            #endregion

            foreach (XmlNode propertyMapping in propertiesMapping)
                table.Fields.Add(this.GetFieldMapping(propertyMapping));

            return table;
        }

        /// <summary>
        /// Obtém o objeto que representa o mapeamento de uma propriedade.
        /// </summary>
        /// <param name="propertyMapping">Nó XML que representa a propriedade.</param>
        /// <returns>Objeto <see cref="FieldMapping"/>.</returns>
        private FieldMapping GetFieldMapping(XmlNode propertyMapping) {
            FieldMapping field = new FieldMapping();
            field.PropertyName = propertyMapping.Attributes["name"].Value;

            if (propertyMapping.Attributes["usePropertyNameAsMemberName"].Value.Parse<bool>())
                field.FieldName = propertyMapping.Attributes["name"].Value;
            else
                field.FieldName = propertyMapping.Attributes["column"].Value;

            field.IsPrimaryKey = propertyMapping.Attributes["isPrimaryKey"].Value.Parse<bool>();
            field.IsAutoKey = propertyMapping.Attributes["isAutoKey"].Value.Parse<bool>();
            field.SequenceName = propertyMapping.Attributes["sequenceName"].Value;
            field.AllowNull = propertyMapping.Attributes["allowNull"].Value.Parse<bool>();
            field.IgnoreOnInsert = propertyMapping.Attributes["ignoreOnInsert"].Value.Parse<bool>();
            field.IgnoreOnUpdate = propertyMapping.Attributes["ignoreOnUpdate"].Value.Parse<bool>();

            return field;
        }

        #endregion

        #region From Class

        /// <summary>
        /// Obtém o objeto que representa o mapeamento de uma tabela.
        /// </summary>
        /// <param name="className">Nome da classe a ser mapeada.</param>
        /// <param name="attTable">Objeto <see cref="TableAttribute"/> de mapeamento para tabelas anexado ao objeto.</param>
        /// <returns>Objeto <see cref="TableMapping"/>.</returns>
        public TableMapping GetTableMapping(string className
            , TableAttribute attTable) {
            TableMapping table = new TableMapping();
            table.EntityName = className;
            table.IsView = attTable.IsView;
            table.TableName = (attTable.UsePropertyNameAsMemberName ? className : attTable.Name);
            table.IsLazyLoading = attTable.IsLazyLoading;
            table.ConnectionName = attTable.ConnectionName;

            return table;
        }

        /// <summary>
        /// Obtém o objeto que representa o mapeamento de uma propriedade.
        /// </summary>
        /// <param name="propertyName">Nome da propriedade a ser mapeada.</param>
        /// <param name="attField">Objeto <see cref="ColumnAttribute"/> de mapeamento para propriedades anexado ao objeto.</param>
        /// <returns>Objeto <see cref="FieldMapping"/>.</returns>
        public FieldMapping GetFieldMapping(string propertyName
            , ColumnAttribute attField) {
            FieldMapping field = new FieldMapping();
            field.PropertyName = propertyName;
            field.FieldName = (attField.UsePropertyNameAsMemberName ? propertyName : attField.Name);
            field.IsPrimaryKey = attField.IsPrimaryKey;
            field.IsAutoKey = attField.IsAutoKey;
            field.SequenceName = attField.SequenceName;
            field.AllowNull = attField.AllowNull;
            field.IgnoreOnInsert = attField.IgnoreOnInsert;
            field.IgnoreOnUpdate = attField.IgnoreOnUpdate;

            return field;
        }

        #endregion

        #endregion
    }
}
