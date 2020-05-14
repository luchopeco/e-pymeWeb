using System;
using System.Data;
using System.Text;
using Npgsql;
using NpgsqlTypes;
using System.Text.RegularExpressions;
using System.Configuration;


namespace Datos
{
    public enum EndTransactionOperation
    {

        Rollback,
        Commit
    }
    public enum State
    {
        Open,
        Closed
    }
    public enum ExecutionType
    {
        Text,
        Function
    }

    public sealed class Connection : IDisposable
    {
        internal IDbConnection connection;
        IDbTransaction transaction;
        private long LastInsertion = -1;
        private bool disposed = false;
        public bool TransaccionActiva = false;
        /// <summary>
        /// Devuelve la cantidad de filas Afectadas en la ultima ejecucion
        /// OJO solo utilizar para los test
        /// </summary>
        public int RowAffectedTest{get;set;}


        public Connection()
            : this(System.Configuration.ConfigurationManager.ConnectionStrings["pg_local"].ConnectionString)
        {

        }

        //public Connection() : this("server=localhost;user id=postgres;pwd=anita02;database=gestionproyectos") { }

        public Connection(string connectionString)
        {
            this.connection = new NpgsqlConnection(connectionString);
        }

        ~Connection()
        {
            if (!disposed)
                if (this.State == State.Open)
                {

                    this.connection.Close();
                    this.Dispose();
                }

        }

        public void Open()
        {
            if (this.connection.State != ConnectionState.Broken && this.connection.State != ConnectionState.Closed)
                throw new Exception ("Already connected to server");
            this.connection.Open();
        }

        public void Close()
        {
            if (this.transaction != null)
            {
                // The transaction needs to be rolledback if it is a connection pooling. (Due to MySql Connector Bug
                try
                {
                    this.transaction.Rollback();
                }
                catch (InvalidOperationException ex)
                {
                    ex.ToString();
                }
                this.transaction.Dispose();
            }
            if (this.connection.State != ConnectionState.Closed)
            {
                this.connection.Close();
            }
            else
                throw new ConnectionException("Not connected to server");
        }

        public void BeginTransaction()
        {
            if (this.transaction != null)
                this.transaction.Dispose();
            this.transaction = this.connection.BeginTransaction();
            TransaccionActiva = true;
        }

        private void EndTransaction(EndTransactionOperation operation)
        {
            if (transaction != null)
            {
                if (operation == EndTransactionOperation.Commit)
                    this.transaction.Commit();
                else
                    this.transaction.Rollback();
            }
            TransaccionActiva = false;
        }

        //modificacion clase original
        public void CommitTransaction()
        {
            EndTransaction(EndTransactionOperation.Commit);
        }
        public void RollbackTransaction()
        {
            EndTransaction(EndTransactionOperation.Rollback);
        }

        public State State
        {
            get
            {
                if (this.connection == null)
                    throw new ConnectionException("Connection is null");
                if (this.connection.State == ConnectionState.Closed || this.connection.State == ConnectionState.Broken)
                    return State.Closed;
                else
                    return State.Open;
            }
        }

        public override string ToString()
        {
            return "Data Object";
        }

        public int LastInsertedId(string secuencia)
        {
            return LastInsertedId(secuencia, false);
        }

        public int LastInsertedId(string secuencia, bool transaccion)
        {
            if (this.connection.State == ConnectionState.Closed)
                return 0;
            if (transaccion)
                return Convert.ToInt32(this.ExecuteScalar(true, ExecutionType.Text, string.Format("SELECT CURRVAL('{0}')", secuencia)));
            else
                return Convert.ToInt32(this.ExecuteScalar(string.Format("SELECT CURRVAL('{0}')", secuencia)));
        }

        public static DataTable ShowConnectionTrace()
        {
            using (Connection c = new Connection())
            {
                return c.GetDT(ExecutionType.Text, "SELECT datname,usename,procpid,client_addr,waiting,query_start,current_query FROM pg_stat_activity;");
            }
        }

        #region Execution Methods
        public object ExecuteScalar(string text, params object[] parameters)
        {
            return ExecuteScalar(false, ExecutionType.Text, text, parameters);
        }
        public object ExecuteScalar(ExecutionType execType, string text, params object[] parameters)
        {
            return this.ExecuteScalar(false, execType, text, parameters);
        }
        public object ExecuteScalar(bool useTransaction, ExecutionType execType, string text, params object[] parameters)
        {
            object retVal;
            NpgsqlCommand loCmd;

            if (execType == ExecutionType.Function)
                loCmd = (NpgsqlCommand)this.GetFunctionCommand(useTransaction, text, parameters);
            else
            {
                loCmd = (NpgsqlCommand)this.GetSqlCommand(useTransaction, text, parameters);
                UpdateParams(ref loCmd, text, parameters);
            }
            retVal = loCmd.ExecuteScalar();
            loCmd.Dispose();
            return retVal;
        }
        /// <summary>
        /// Devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="execType"></param>
        /// <param name="text"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(string text, params object[] parameters)
        {
            
          return  this.Execute(false, ExecutionType.Text, text, parameters);
        }
        /// <summary>
        /// Devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="execType"></param>
        /// <param name="text"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(ExecutionType execType, string text, params object[] parameters)
        {
            //en true para q use siempre la transaccion
           return this.Execute(true, execType, text, parameters);
        }
        /// <summary>
        /// Devuelve la cantidad de filas afectadas
        /// </summary>
        /// <param name="execType"></param>
        /// <param name="text"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int Execute(bool useTransaction, ExecutionType execType, string text, params object[] parameters)
        {
            NpgsqlCommand loCmd;
            if (execType == ExecutionType.Function)
                loCmd = (NpgsqlCommand)this.GetFunctionCommand(useTransaction, text, parameters);
            else
            {
                loCmd = (NpgsqlCommand)this.GetSqlCommand(useTransaction, text, parameters);
                UpdateParams(ref loCmd, text, parameters);
            }            
            int rowAffected = loCmd.ExecuteNonQuery();
            RowAffectedTest = rowAffected;
            this.LastInsertion = loCmd.LastInsertedOID;
            loCmd.Dispose();
            return rowAffected;
        }

        public DataSet GetDS(string text, params object[] parameters)
        {
            return this.GetDS(ExecutionType.Text, text, parameters);
        }
        public DataSet GetDS(ExecutionType execType, string text, params object[] parameters)
        {
            return this.GetDS(false, execType, text, parameters);
        }
        public DataSet GetDS(bool useTransaction, ExecutionType execType, string text, params object[] parameters)
        {
            NpgsqlDataAdapter dAdapter;
            NpgsqlCommand loCmd;

            if (execType == ExecutionType.Function)
                loCmd = (NpgsqlCommand)this.GetFunctionCommand(useTransaction, text, parameters);
            else
            {
                loCmd = (NpgsqlCommand)this.GetSqlCommand(useTransaction, text, parameters);
                UpdateParams(ref loCmd, text, parameters);
            }
            dAdapter = new NpgsqlDataAdapter(loCmd);
            DataSet dsResult = new DataSet();
            dAdapter.Fill(dsResult);
            this.LastInsertion = loCmd.LastInsertedOID;
            dAdapter.Dispose();
            loCmd.Dispose();
            return dsResult;
        }

        public DataTable GetDT(string text, params object[] parameters)
        {
            return this.GetDT(false, ExecutionType.Text, text, parameters);
        }
        public DataTable GetDT(ExecutionType execType, string text, params object[] parameters)
        {

            return this.GetDT(false, execType, text, parameters);
        }
        public DataTable GetDT(bool useTransaction, ExecutionType execType, string text, params object[] parameters)
        {
            NpgsqlCommand loCmd;
            if (execType == ExecutionType.Function)
                loCmd = (NpgsqlCommand)this.GetFunctionCommand(useTransaction, text, parameters);
            else
            {
                loCmd = (NpgsqlCommand)this.GetSqlCommand(useTransaction, text, parameters);
                UpdateParams(ref loCmd, text, parameters);
            }
            DataTable dtResult = new DataTable();
            NpgsqlDataAdapter dAdapter = new NpgsqlDataAdapter(loCmd);
            dAdapter.Fill(dtResult);
            this.LastInsertion = loCmd.LastInsertedOID;
            loCmd.Dispose();
            dAdapter.Dispose();
            return dtResult;
        }

        #endregion
        
        #region Protected
        private IDbCommand GetSqlCommand(bool useTransaction, string sqlText, params object[] parameters)
        {
            if (useTransaction && this.transaction == null)
                throw new InvalidOperationException("The transaction is not started");
            NpgsqlCommand cmd = useTransaction ? new NpgsqlCommand(sqlText, (NpgsqlConnection)this.connection, (NpgsqlTransaction)this.transaction) : new NpgsqlCommand(sqlText, (NpgsqlConnection)this.connection);
            return cmd;

        }
        private object GetFunctionCommand(bool useTransaction, string sqlText, params object[] parameters)
        {
            StringBuilder pars = new StringBuilder("");
            object ret;
            // string pars = "";
            foreach (object item in parameters)
            {
                pars.Append((pars.ToString().Trim() != "" ? "," : ""));
                if (item is string)
                    pars.Append(string.Concat("'", item, "'"));
                else if (item is DateTime)
                    pars.Append(String.Concat("'", GetNpgsqlDateTime(item).ToString(), "'"));
                else
                    pars.Append(item);
            }
            ret = this.GetSqlCommand(useTransaction, "SELECT * from " + sqlText + "(" + pars.ToString() + ")");
            return ret;
        }
        #endregion
        public static DateTime GetDateTime(object dt)
        {
            return Connection.GetDateTime((NpgsqlDate)dt);
        }

        public static NpgsqlTimeStamp GetNpgsqlDateTime(object dt)
        {
            NpgsqlDate dtDate = new NpgsqlDate(Convert.ToDateTime(dt));
            NpgsqlTime dtTime = new NpgsqlTime(((DateTime)dt).Ticks);

            return new NpgsqlTimeStamp(dtDate, dtTime);
        }

        private void UpdateParams(ref NpgsqlCommand cmd, string text, params object[] pars)
        {
            Regex reg = new Regex(":([a-z]|[0-9]|_)*", RegexOptions.IgnoreCase);
            MatchCollection colls = reg.Matches(text);
            int idx = 0;
            foreach (Match ma in colls)
            {
                if (cmd.Parameters.IndexOf(ma.Value) < 0)
                {
                    cmd.Parameters.AddWithValue(ma.Value, pars[idx]);
                    idx++;
                }
            }
            if (cmd.Parameters.Count != pars.Length)
                throw new Exception("No coinciden los parametros con la cantidad informada.");

        }


        public static DateTime GetDateTime(NpgsqlDate dt)
        {
            return (DateTime)dt;
        }

        public static NpgsqlDate GetNpgsqlDateTime(DateTime dt)
        {
            return new NpgsqlDate(dt);
        }


        public static string GetSqlCodeFromResource(string fileName)
        {
            System.Reflection.Assembly asm = System.Reflection.Assembly.GetCallingAssembly();
            string name = string.Empty;

            foreach (string n in asm.GetManifestResourceNames())
            {
                if (n.Contains(fileName.Replace('\\', '.')))
                    name = n;
 
            }
            
            System.IO.StreamReader reader = new System.IO.StreamReader(asm.GetManifestResourceStream(name));
            return reader.ReadToEnd();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.connection.Dispose();
            GC.SuppressFinalize(this.connection);
            disposed = true;
        }

        #endregion

    }
}
