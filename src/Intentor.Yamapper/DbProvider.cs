/*********************************************
Intentor.Yamapper
**********************************************
Copyright © 2009-2012 André "Intentor" Martins
http://intentor.com.br/projects/yamapper/
*********************************************/

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using Intentor.Utilities;
using Intentor.Yamapper.Drivers;
using Intentor.Yamapper.Mapper;

namespace Intentor.Yamapper {
    /// <summary>
    /// Manipulador de acesso ao banco de dados.
    /// </summary>
    public sealed class DbProvider : IDisposable {
        #region Construtor

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="connectionString">String de conexão ao banco de dados.
        /// </param>
        /// <param name="driverFactory">Driver para resolução de objetos para acesso ao banco de dados.</param>
        public DbProvider(string connectionString
            , DriverFactory driverFactory)
            :
            this(connectionString
                , driverFactory
                , false
                , false) { }


        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="connectionString">String de conexão ao banco de dados.
        /// </param>
        /// <param name="driverFactory">Driver para resolução de objetos para acesso ao banco de dados.</param>
        /// <param name="openConnection">Identifica se a conexão com o banco de dados deve ser aberta já na criação do objeto.</param>
        public DbProvider(string connectionString
            , DriverFactory driverFactory
            , bool openConnection)
            :
            this(connectionString
                , driverFactory
                , openConnection
                , false) { }

        /// <summary>
        /// Construtor da classe.
        /// </summary>
        /// <param name="connectionString">String de conexão ao banco de dados.
        /// </param>
        /// <param name="driverFactory">Driver para resolução de objetos para acesso ao banco de dados.</param>
        /// <param name="openConnection">Identifica se a conexão com o banco de dados deve ser aberta já na criação do objeto.</param>
        /// <param name="openTransaction">Identifica se se deve iniciar uma transação.</param>
        public DbProvider(string connectionString
            , DriverFactory driverFactory
            , bool openConnection
            , bool openTransaction) {
            _connectionString = connectionString;
            _driverFactory = driverFactory;

            if (openConnection) {
                this.OpenConnection();

                if (openTransaction) {
                    this.BeginTransaction();
                }
            }
        }

        #endregion

        #region Destrutor

        /// <summary>
        /// Destrutor da classe <see cref="DbProvider"/>.
        /// </summary>
        ~DbProvider() {
            this.Dispose(false);
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Identifica se o método Dispose já foi chamado.
        /// </summary>
        private bool _disposed = false;

        /// <summary>
        ///	Libera recursos utilizados pela classe e finaliza sua execução.
        /// </summary>
        /// <remarks>Conforme artigo <see href="http://msdn2.microsoft.com/en-us/library/system.idisposable.dispose(vs.80).aspx">IDisposable.Dispose Method (MSDN)</see>.</remarks>
        public void Dispose() {
            this.Dispose(true);
            /* This object will be cleaned up by the Dispose method.
             * Therefore, you should call GC.SupressFinalize to
             * take this object off the finalization queue 
             * and prevent finalization code for this object
             * from executing a second time.
             */
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///		Libera recursos utilizados pela classe.
        /// </summary>
        /// <param name="disposing">
        ///     Identifica se o dispose está sendo chamado a partir do método <see cref="Dispose"/>
        ///     (seu valor será <see langword="true"/>) ou pelo destrutor (seu valor será 
        ///     <see langword="false"/>).
        /// </param>
        /// <remarks>Conforme artigo <see href="http://msdn2.microsoft.com/en-us/library/system.idisposable.dispose(vs.80).aspx">IDisposable.Dispose Method (MSDN)</see>.</remarks>
        private void Dispose(bool disposing) {
            //Check to see if Dispose has already been called.
            if (!this._disposed) {
                //If "disposing" equals true, dispose all managed and unmanaged resources.
                if (disposing) {
                    this.CloseConnection();
                }

                /* Call the appropriate methods to clean up unmanaged resources here.
                 * If disposing is false, only the following code is executed.
                 */
            }

            this._disposed = true;
        }

        #endregion

        #region Campos

        /// <summary>
        /// Factory responsável por instanciar os objetos nos tipos corretos conforme o driver selecionado.
        /// </summary>
        private DriverFactory _driverFactory;

        /// <summary>
        /// Objeto de conexão com o banco de dados.
        /// </summary>
        private IDbConnection _cn;

        /// <summary>
        /// String de conexão utilizada para acesso ao banco de dados.
        /// </summary>
        private string _connectionString;

        /// <summary>
        /// Objeto <see cref="IDbTransaction"/> que representa uma transação.
        /// </summary>
        private IDbTransaction _transaction;

        /// <summary>
        /// Idenfitifica se uma transação está aberta.
        /// </summary>
        private bool _isTransactionOpen;

        #endregion

        #region Propriedades

        /// <summary>
        /// Indica se o provedor já foi descartado.
        /// </summary>
        internal bool Disposed {
            get { return _disposed; }
        }

        /// <summary>
        /// Factory responsável por instanciar os objetos nos tipos corretos conforme o driver selecionado.
        /// </summary>
        internal DriverFactory DriverFactory {
            get { return _driverFactory; }
        }

        /// <summary>
        /// String de conexão utilizada para acesso ao banco de dados.
        /// </summary>
        public string ConnectionString {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// Conexão utilizada para acesso ao banco de dados.
        /// </summary>
        public IDbConnection Connection {
            get { return _cn; }
        }

        /// <summary>
        /// Transação aberta ou null caso não haja transação.
        /// </summary>
        public IDbTransaction Transaction {
            get { return _transaction; }
        }

        /// <summary>
        /// Identifica se uma conexão está aberta.
        /// </summary>
        public bool IsConnectionOpen {
            get {
                bool isConnectionOpen = false;

                if (!this._cn.IsNullOrDbNull()) {
                    isConnectionOpen = (this._cn.State == ConnectionState.Open);
                }

                return isConnectionOpen;
            }
        }

        /// <summary>
        /// Idenfitifica se uma transação está aberta.
        /// </summary>
        public bool IsTransactionOpen {
            get { return this._isTransactionOpen; }
        }

        #endregion

        #region Métodos

        #region Private

        /// <summary>
        /// Popula um objeto <see cref="DataTable"/> com base em um objeto <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém os dados da conexão.</param>
        /// <returns>Retorna um objeto <see cref="DataTable"/>.</returns>
        private DataTable FillDataTable(IDbCommand cmd) {
            return this.FillDataSet(cmd).Tables[0];
        }

        /// <summary>
        /// Popula um objeto <see cref="DataSet"/> com base em um objeto <see cref="IDbCommand"/>.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém os dados da conexão.</param>
        /// <returns>Retorna um objeto <see cref="DataSet"/>.</returns>
        private DataSet FillDataSet(IDbCommand cmd) {
            DataSet ds = new DataSet();

            IDbDataAdapter da = _driverFactory.CreateDataAdapter(cmd);
            da.Fill(ds);

            return ds;
        }

        /// <summary>
        /// Realiza validação de um objeto <see cref="IDbCommand"/> trocando todos os parâmetros passados com "$" pelo nome formatado.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> a ser validado.</param>
        private void ParseDbCommand(IDbCommand cmd) {
            Check.Require(cmd.Connection == _cn, Messages.CommandCreatedOutsideProvider);

            if (cmd.CommandType == CommandType.Text) {
                string sql = cmd.CommandText;
                MatchCollection matches = Regex.Matches(sql, @"\$(?<param>\w+)\s?", RegexOptions.IgnoreCase);

                if (matches.Count > 0) {
                    foreach (Match m in matches) {
                        string value = m.Groups["param"].Value;
                        sql = sql.Replace("$" + value, _driverFactory.FormatNameForParameter(value));
                    }

                    cmd.CommandText = sql;
                }
            }
        }

        #endregion

        #region Public

        #region CreateCommand

        /// <summary>
        /// Cria um objeto <see cref="IDbCommand"/>.
        /// </summary>
        /// <returns>Retorna um objeto <see cref="IDbCommand"/>.</returns>
        public IDbCommand CreateCommand() {
            Check.Require(this.IsConnectionOpen, Messages.ConnectionClosed);

            IDbCommand cmd = _cn.CreateCommand();
            cmd.Transaction = _transaction;

            return cmd;
        }

        /// <summary>
        /// Cria um objeto <see cref="IDbCommand"/>.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto command a ser criado.</typeparam>
        /// <returns>Retorna um objeto <typeparamref name="T"/>.</returns>
        public T CreateCommand<T>()
            where T : IDbCommand {
            Check.Require(this.IsConnectionOpen, Messages.ConnectionClosed);

            T cmd = Activator.CreateInstance<T>();

            cmd.Connection = _cn;
            cmd.Transaction = _transaction;

            return cmd;
        }

        /// <summary>
        /// Cria um objeto <see cref="IDbCommand"/> para execução de instruções SQL.
        /// </summary>
        /// <param name="sql">Instruções SQL a serem executadas.</param>
        /// <returns>Retorna um objeto <see cref="IDbCommand"/> pronto para utilização.</returns>
        public IDbCommand CreateCommandForText(string sql) {
            IDbCommand cmd = this.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            return cmd;
        }

        /// <summary>
        /// Cria um objeto <see cref="IDbCommand"/> para execução de instruções SQL.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto command a ser criado.</typeparam>
        /// <param name="sql">Instruções SQL a serem executadas.</param>
        /// <returns>Retorna um objeto <see cref="IDbCommand"/> pronto para utilização.</returns>
        public T CreateCommandForText<T>(string sql)
            where T : IDbCommand {
            T cmd = this.CreateCommand<T>();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = sql;

            return cmd;
        }

        /// <summary>
        /// Cria um objeto <see cref="IDbCommand"/> para execução de stored procedures.
        /// </summary>
        /// <param name="procedureName">Nome da stored procedure.</param>
        /// <returns>Retorna um objeto <see cref="IDbCommand"/> pronto para utilização.</returns>
        public IDbCommand CreateCommandForProcedure(string procedureName) {
            IDbCommand cmd = this.CreateCommand();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            return cmd;
        }

        /// <summary>
        /// Cria um objeto <see cref="IDbCommand"/> para execução de stored procedures.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto command a ser criado.</typeparam>
        /// <param name="procedureName">Nome da stored procedure.</param>
        /// <returns>Objeto <see cref="IDbCommand"/> do tipo <typeparamref name="T"/> pronto para utilização.</returns>
        public T CreateCommandForProcedure<T>(string procedureName)
            where T : IDbCommand {
            T cmd = this.CreateCommand<T>();

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            return cmd;
        }

        #endregion

        #region Gerenciamento de conexão

        /// <summary>
        /// Abre conexão com o banco de dados.
        /// </summary>
        public void OpenConnection() {
            if (_cn != null && _cn.State == ConnectionState.Open) return;

            _cn = _driverFactory.CreateConnection(_connectionString);
            _cn.Open();
        }

        /// <summary>
        /// Fecha a conexão com o banco de dados.
        /// </summary>
        public void CloseConnection() {
            if (_cn == null || _cn.State == ConnectionState.Closed) return;

            this.CommitTransaction();

            _cn.Close();
        }

        #endregion

        #region Gerenciamento de Transação

        /// <summary>
        /// Inicia uma transação.
        /// </summary>
        /// <returns>Retorna um objeto <see cref="IDbTransaction"/> que representa a transação criada.</returns>
        public IDbTransaction BeginTransaction() {
            if (_cn != null && !_isTransactionOpen) {
                _transaction = this._cn.BeginTransaction();
                _isTransactionOpen = true;
            }

            return _transaction;
        }

        /// <summary>
        /// Valida uma transação.
        /// </summary>
        public void CommitTransaction() {
            if (_cn != null && _isTransactionOpen) {
                _transaction.Commit();

                _transaction.Dispose();
                _transaction = null;

                _isTransactionOpen = false;
            }
        }

        /// <summary>
        /// Desfaz as ações realizadas durante uma transação.
        /// </summary>
        public void RollbackTransaction() {
            if (_cn != null && _isTransactionOpen) {
                _transaction.Rollback();

                _transaction.Dispose();
                _transaction = null;

                _isTransactionOpen = false;
            }
        }

        #endregion

        #region Procedimentos de acesso ao banco de dados

        #region Utilizando mapeamento

        /// <summary>
        /// Obtém registros a partir de critérios.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="crit">Critérios para obtenção dos dados.</param>
        /// <returns>Retorna uma lista de objetos <typeparamref name="T"/>.</returns>
        public List<T> Get<T>(Criteria crit)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            string query = String.Empty;

            SqlSelectString sql = new SqlSelectString(
                DbMapperQueryBuilder.CreateSelectFor<T>(_driverFactory.ParameterPrefix)
                , crit.GetWhereClause<T>(_driverFactory.ParameterPrefix)
                , crit.GetOrderByClause<T>());
            query = _driverFactory.GetCommandForLimit(sql, crit.OffsetValue, crit.LimitValue);

            List<T> list = null;

            using (IDbCommand cmd = this.CreateCommandForText(query)) {
                crit.FillParameters(cmd);
                list = this.GetDataReader(cmd).MapTo<T>();
            }

            return list;
        }

        /// <summary>
        /// Obtém todos os registros de uma determinada tabela.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ter os dados obtidos.</typeparam>
        /// <returns>Lista contendo todos os registros presentes na tabela.</returns>
        public List<T> GetAll<T>()
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            string sql = DbMapperQueryBuilder.CreateSelectFor<T>(_driverFactory.ParameterPrefix);
            return this.GetDataReader(sql).MapTo<T>();
        }

        /// <summary>
        /// Insere um objeto na base de dados.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser inserido.</typeparam>
        /// <param name="entity">Objeto a ser inserido, passado por referência.</param>
        /// <remarks>Os campos de autonumeração são preenchidos diretamente na entidade.</remarks>
        public void Insert<T>(T obj)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            Type entity = typeof(T);
            var mapping = DbMapperHelper.Mappings[entity.FullName];
            bool hasAutoKey = mapping.HasAutoKey();

            Check.Require(!mapping.IsView, String.Format(Messages.EntityIsViewInsert, mapping.EntityName));

            string sqlToGetPrimaryKey = _driverFactory.GetCommandForPrimaryKeyValue();
            StringBuilder sql = new StringBuilder();
            sql.Append(DbMapperQueryBuilder.CreateInsertFor<T>(_driverFactory.ParameterPrefix));
            if (hasAutoKey) sql.AppendFormat(";{0}", sqlToGetPrimaryKey);

            using (IDbCommand cmd = this.CreateCommandForText(sql.ToString())) {
                #region População das propriedades

                int paramsCount = 0;

                foreach (FieldMapping field in mapping.Fields) {
                    if (field.IsAutoKey || field.IgnoreOnInsert) continue;

                    paramsCount++;

                    object paramValue;
                    string paramName = String.Concat("p", paramsCount);

                    //Caso o campo seja populado por sequence, obtém seu valor.
                    if (field.SequenceName.IsNullOrEmpty()) {
                        paramValue = DbMapperHelper.GetPropertyValue(obj, entity, field);
                    } else {
                        string sqlForSequenceValue = sqlToGetPrimaryKey.Replace("<SequenceName>", field.SequenceName);
                        paramValue = this.ExecuteScalar(sqlForSequenceValue);
                        DbMapperHelper.SetPropertyValue(obj, entity, field, paramValue);
                    }

                    //Verifica se se deve definir explicitamente o tipo do campo.
                    var fieldType = entity.GetProperty(field.PropertyName).PropertyType.Name;
                    if (fieldType.StartsWith("Byte")) {
                        cmd.CreateParameter(paramName, paramValue, DbType.Binary);
                    } else {
                        cmd.CreateParameter(paramName, paramValue);
                    }
                }

                #endregion

                if (hasAutoKey) {
                    var property = entity.GetProperty(mapping.GetAutoKey().PropertyName);
                    object pk = Convert.ChangeType(this.ExecuteScalar(cmd), property.PropertyType);
                    property.SetValue(obj, pk, null);
                } else {
                    this.ExecuteNonQuery(cmd);
                }
            }
        }

        /// <summary>
        /// Atualiza um objeto na base de dados.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser atualizado.</typeparam>
        /// <param name="entity">Objeto a ser atualizado.</param>
        /// <returns>Quantidade de registros excluídos.</returns>
        public void Update<T>(T obj)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            Type entity = typeof(T);
            var mapping = DbMapperHelper.Mappings[entity.FullName];

            Check.Require(!mapping.IsView, String.Format(Messages.EntityIsViewUpdate, mapping.EntityName));

            string sql = DbMapperQueryBuilder.CreateUpdateFor<T>(_driverFactory.ParameterPrefix);

            using (IDbCommand cmd = this.CreateCommandForText(sql)) {
                #region População das propriedades

                int paramsCount = 0;

                foreach (FieldMapping field in mapping.Fields) {
                    if (field.IsPrimaryKey || field.IsAutoKey || field.IgnoreOnUpdate) continue;

                    paramsCount++;
                    object paramValue = DbMapperHelper.GetPropertyValue(obj, entity, field);
                    string paramName = String.Concat("p", paramsCount);

                    //Verifica se se deve definir explicitamente o tipo do campo.
                    var fieldType = entity.GetProperty(field.PropertyName).PropertyType.Name;
                    if (fieldType.StartsWith("Byte")) {
                        cmd.CreateParameter(paramName, paramValue, DbType.Binary);
                    } else {
                        cmd.CreateParameter(paramName, paramValue);
                    }
                }

                #region População da(s) chave(s) primária(s)

                var pks = mapping.GetPrimaryKeys();
                Check.Require(pks.Count > 0, String.Format(Messages.PrimaryKeyNotFoundEntity, mapping.EntityName));
                foreach (var pk in pks) cmd.CreateParameter(pk.PropertyName, DbMapperHelper.GetPropertyValue(obj, entity, pk));

                #endregion

                #endregion

                this.ExecuteNonQuery(cmd);
            }
        }

        /// <summary>
        /// Exclui registros de uma determinada entidade da base de dados.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser deletado.</typeparam>
        /// <param name="crit">Critérios para obtenção dos dados.</param>
        /// <returns>Quantidade de registros excluídos.</returns>
        public int Delete<T>(Criteria crit)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            int count = 0;
            var mapping = DbMapperHelper.Mappings[typeof(T).FullName];

            Check.Require(!mapping.IsView, String.Format(Messages.EntityIsViewDelete, mapping.EntityName));

            StringBuilder sql = new StringBuilder();
            sql.Append(DbMapperQueryBuilder.CreateDeleteFor<T>(_driverFactory.ParameterPrefix, false));
            sql.Append(crit.GetWhereClause<T>(_driverFactory.ParameterPrefix));

            using (IDbCommand cmd = this.CreateCommandForText(sql.ToString())) {
                crit.FillParameters(cmd);
                count = this.ExecuteNonQuery(cmd);
            }

            return count;
        }

        /// <summary>
        /// Verifica se há registros para uma determinada entidade.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <returns>Valor booleano indicando se há registros.</returns>
        public bool Exists<T>()
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            return (this.Count<T>(null) > 0);
        }

        /// <summary>
        /// Verifica se há registros para uma determinada entidade a partir critérios.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="crit">Objeto <see cref="Criteria"/> contendo os critérios para obtenção dos dados.</param>
        /// <returns>Valor booleano indicando se há registros.</returns>
        public bool Exists<T>(Criteria crit)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            return (this.Count<T>(crit) > 0);
        }

        /// <summary>
        /// Realiza contagem de registros para uma determinada entidade.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <returns>Quantidade de registors de acordo com os critérios informados.</returns>
        public int Count<T>()
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            return this.Count<T>(null);
        }

        /// <summary>
        /// Realiza contagem de registros a partir de um determinado critério.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser mapeado.</typeparam>
        /// <param name="crit">Objeto <see cref="Criteria"/> contendo os critérios para obtenção dos dados.</param>
        /// <returns>Quantidade de registors de acordo com os critérios informados.</returns>
        public int Count<T>(Criteria crit)
            where T : class {
            Check.Require((DbMapperHelper.Mappings[typeof(T).FullName] != null), String.Format(Messages.EntityNotMapped, typeof(T).FullName));

            int count = 0;
            StringBuilder sql = new StringBuilder();
            sql.Append(DbMapperQueryBuilder.CreateSelectCountFor<T>());
            if (crit != null) sql.Append(crit.GetWhereClause<T>(_driverFactory.ParameterPrefix));

            using (IDbCommand cmd = this.CreateCommandForText(sql.ToString())) {
                if (crit != null) crit.FillParameters(cmd);
                count = this.ExecuteScalar<int>(cmd);
            }

            return count;
        }

        #endregion

        #region Utilizando instruções SQL

        /// <summary>
        /// Executa instruções SQL que não retornam resultado algum.
        /// </summary>
        /// <param name="sql">Instruções SQL a serem executadas.</param>
        /// <returns>Retorna a quantidade de registros afetados.</returns>
        public int ExecuteNonQuery(string sql) {
            IDbCommand cmd = this.CreateCommandForText(sql);
            return this.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Executa instruções SQL que não retornam resultado algum.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém as definições da instrução a ser executada.</param>
        /// <returns>Retorna a quantidade de registros afetados.</returns>
        public int ExecuteNonQuery(IDbCommand cmd) {
            this.ParseDbCommand(cmd);

            int regQtd = 0;

            try {
                regQtd = cmd.ExecuteNonQuery();
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return regQtd;
        }

        /// <summary>
        /// Executa instruções <c>SELECT</c> e retorna o dado presente na primeira coluna do primeiro registro do resultado obtido com a execução do query SQL.
        /// </summary>
        /// <param name="sql">Query SQL a ser executada.</param>
        /// <returns>Retorna um objeto que representa o dado obtido com a execução da query SQL.</returns>
        public object ExecuteScalar(string sql) {
            IDbCommand cmd = this.CreateCommandForText(sql);
            return this.ExecuteScalar(cmd);
        }

        /// <summary>
        /// Executa instruções <c>SELECT</c> e retorna o dado presente na primeira coluna do primeiro registro do resultado obtido com a execução do query SQL.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém as definições da instrução a ser executada.</param>
        /// <returns>Retorna um objeto que representa o dado obtido com a execução da query SQL.</returns>
        public object ExecuteScalar(IDbCommand cmd) {
            this.ParseDbCommand(cmd);

            object res;

            try {
                res = cmd.ExecuteScalar();
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return res;
        }

        /// <summary>
        /// Executa instruções <c>SELECT</c> e retorna o dado presente na primeira coluna do primeiro registro do resultado obtido com a execução do query SQL.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser retornado.</typeparam>
        /// <param name="sql">Query SQL a ser executada.</param>
        /// <returns>Retorna um objeto que representa o dado obtido com a execução da query SQL.</returns>
        public T ExecuteScalar<T>(string sql)
            where T : IConvertible {
            IDbCommand cmd = this.CreateCommandForText(sql);
            return this.ExecuteScalar<T>(cmd);
        }

        /// <summary>
        /// Executa instruções <c>SELECT</c> e retorna o dado presente na primeira coluna do primeiro registro do resultado obtido com a execução do query SQL.
        /// </summary>
        /// <typeparam name="T">Tipo do objeto a ser retornado.</typeparam>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém as definições da instrução a ser executada.</param>
        /// <returns>Retorna um objeto que representa o dado obtido com a execução da query SQL.</returns>
        public T ExecuteScalar<T>(IDbCommand cmd)
            where T : IConvertible {
            this.ParseDbCommand(cmd);

            T obj = default(T);

            try {
                object res = cmd.ExecuteScalar();
                if (!res.IsNullOrDbNull()) obj = res.Parse<T>();
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return obj;
        }

        /// <summary>
        /// Executa uma stored procedure que não contém parâmetros e dados de retorno.
        /// </summary>
        /// <param name="procedureName">Nome da procedure a ser executada.</param>
        /// <returns>Retorna a quantidade de registros afetados.</returns>
        public int ExecuteProcedure(string procedureName) {
            int regCountr = 0;

            try {
                using (IDbCommand cmd = this.CreateCommandForProcedure(procedureName)) {
                    regCountr = cmd.ExecuteNonQuery();
                }
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return regCountr;
        }

        /// <summary>
        /// Executa uma query SQL.
        /// </summary>
        /// <param name="sql">Query SQL a ser executada.</param>
        /// <returns>Retorna um <see cref="DataTable"/> contendo o resultado da query.</returns>
        public DataTable GetDataTable(string sql) {
            IDbCommand cmd = this.CreateCommandForText(sql);
            return this.GetDataTable(cmd);
        }

        /// <summary>
        /// Executa uma query SQL.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém as definições da instrução a ser executada.</param>
        /// <returns> Retorna um <see cref="DataTable"/> contendo o resultado da query.</returns>
        public DataTable GetDataTable(IDbCommand cmd) {
            this.ParseDbCommand(cmd);

            DataTable dt = null;

            try {
                dt = this.FillDataTable(cmd);
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return dt;
        }

        /// <summary>
        /// Executa uma query SQL.
        /// </summary>
        /// <param name="sql">Query SQL a ser executada.</param>
        /// <returns> Retorna um <see cref="DataSet"/> contendo o resultado da query.</returns>
        public DataSet GetDataSet(string sql) {
            IDbCommand cmd = this.CreateCommandForText(sql);
            return this.GetDataSet(cmd);
        }

        /// <summary>
        /// Executa uma query SQL.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém as definições da instrução a ser executada.</param>
        /// <returns> Retorna um <see cref="DataSet"/> contendo o resultado da query.</returns>        
        public DataSet GetDataSet(IDbCommand cmd) {
            this.ParseDbCommand(cmd);

            DataSet ds = null;

            try {
                ds = this.FillDataSet(cmd);
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return ds;
        }

        /// <summary>
        /// Executa uma query SQL.
        /// </summary>
        /// <param name="sql">Query SQL a ser executada.</param>
        /// <returns> Retorna um <see cref="IDataReader"/> para acesso aos resultados da query.</returns>
        public IDataReader GetDataReader(string sql) {
            IDbCommand cmd = this.CreateCommandForText(sql);
            return this.GetDataReader(cmd);
        }

        /// <summary>
        /// Executa uma query SQL.
        /// </summary>
        /// <param name="cmd">Objeto <see cref="IDbCommand"/> que contém as definições da instrução a ser executada.</param>
        /// <returns> Retorna um <see cref="IDataReader"/> para acesso aos resultados da query.</returns>   
        public IDataReader GetDataReader(IDbCommand cmd) {
            this.ParseDbCommand(cmd);

            IDataReader dr = null;

            try {
                dr = cmd.ExecuteReader();
            } catch {
                this.RollbackTransaction();
                throw;
            }

            return dr;
        }

        #endregion

        #endregion

        #endregion

        #endregion
    }
}