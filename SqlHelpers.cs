/*SqlHelper
*Created by: Nguyen Tat Thanh
*Created date: 31/03/2021
*Email: thanhntt89bk@gmail.com
*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SqlHelpers
{
    public class Parameters
    {
        private IList<Parameter> _parameters;

        public Parameters()
        {
            _parameters = new List<Parameter>();
        }

        public void Add(Parameter paramter)
        {
            _parameters.Add(paramter);
        }

        public int Count { get { return _parameters.Count; } }

        public IList<Parameter> GetParameters { get { return _parameters; } }
    }

    public class Parameter
    {
        public string Name { get; set; }
        public object Values { get; set; }
    }

    public class ConnectionInfo
    {
        public string ServerName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DatabaseName { get; set; }
        public int TimeOutConnection { get; set; }
        public int TimeOutCommand { get; set; }
    }

    public class SqlHelpers
    {
        /// <summary>
        /// Do not create new instanance  
        /// </summary>
        private SqlHelpers()
        {

        }

        /// <summary>
        /// Create connection string 
        /// </summary>
        /// <param name="serverName"></param>
        /// <param name="userName"></param>
        /// <param name="passwords"></param>
        /// <param name="databaseName"></param>
        /// <param name="timeOutTesting"></param>
        /// <param name="commandTimeOut"></param>
        public static void CreateConnectionString(ConnectionInfo connectionInfo)
        {
            ConnectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Connect Timeout={4};", connectionInfo.ServerName, connectionInfo.DatabaseName, connectionInfo.UserName, connectionInfo.Password, connectionInfo.TimeOutConnection);

            ConnectionStringTesting = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Connect Timeout={4};", connectionInfo.ServerName, connectionInfo.DatabaseName, connectionInfo.UserName, connectionInfo.Password, connectionInfo.TimeOutConnection);

            CommandTimeOut = connectionInfo.TimeOutCommand;
        }

        /// <summary>
        /// Check Sql Connection String
        /// </summary>        
        /// <returns>True when connected | False when fail connect</returns>
        public static bool CheckConnectionString()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ConnectionStringTesting))
                    return false;

                SqlConnection sqlConnection = new SqlConnection(ConnectionStringTesting);
                sqlConnection.Open();
                sqlConnection.Close();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string ConnectionStringTesting { get; set; }

        public static string ConnectionString { get; set; }

        private static SqlConnection _sqConnection { get; set; }

        private static SqlTransaction _sqlTransaction { get; set; }

        public static void CreateSqlConnection(string sqlConnectionString)
        {
            _sqConnection = new SqlConnection(sqlConnectionString);
        }

        public static int CommandTimeOut { get; set; }
        private const int DefaultTimeOut = 30;

        public static void ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            command.Connection = sqlConnection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;

            sqlConnection.Open();
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public static void ExecuteNonQuery(SqlConnection sqlConnection, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();

            command.Connection = sqlConnection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.ExecuteNonQuery();
            sqlConnection.Close();
        }

        public static void ExecuteNonQuery(SqlTransaction sqlTransaction, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            command.Transaction = sqlTransaction;
            command.Connection = sqlTransaction.Connection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;
            command.ExecuteNonQuery();
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, Parameters parameters = null)
        {
            DataSet dataSet = new DataSet();
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            command.Connection = sqlConnection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;

            sqlConnection.Open();
            dataSet = ExecuteDataSet(command);
            sqlConnection.Close();
            return dataSet;
        }

        public static DataSet ExecuteDataset(SqlConnection sqlConnection, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            DataSet dataSet = new DataSet();
            command.Connection = sqlConnection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;

            if (sqlConnection.State == ConnectionState.Closed)
                sqlConnection.Open();
            dataSet = ExecuteDataSet(command);
            sqlConnection.Close();
            return dataSet;
        }

        public static DataSet ExecuteDataset(SqlTransaction sqlTransaction, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            DataSet dataSet = new DataSet();

            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;

            command.Connection = sqlTransaction.Connection;
            dataSet = ExecuteDataSet(command);
            return dataSet;
        }

        private static DataSet ExecuteDataSet(SqlCommand sqlCommand)
        {
            var ds = new DataSet();
            using (var dataAdapter = new SqlDataAdapter(sqlCommand))
            {
                dataAdapter.Fill(ds);
            }

            return ds;
        }

        public static IEnumerable<T> ExecuteObject<T>(SqlTransaction sqlTransaction, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            IList<T> items = new List<T>();
            DataSet dataSet = new DataSet();

            command.Connection = sqlTransaction.Connection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;
            dataSet = ExecuteDataSet(command);

            foreach (var row in dataSet.Tables[0].Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), row);
                items.Add(item);
            }

            return items;
        }

        public static IEnumerable<T> ExecuteObject<T>(SqlConnection connection, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            IList<T> items = new List<T>();
            DataSet dataSet = new DataSet();

            if (connection.State == ConnectionState.Closed)
                connection.Open();

            command.Connection = connection;
            command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
            command.CommandType = commandType;
            command.CommandText = commandText;
            dataSet = ExecuteDataSet(command);

            connection.Close();

            foreach (var row in dataSet.Tables[0].Rows)
            {
                T item = (T)Activator.CreateInstance(typeof(T), row);
                items.Add(item);
            }

            return items;
        }

        public static IEnumerable<T> ExecuteObject<T>(string connectionString, CommandType commandType, string commandText, Parameters parameters = null)
        {
            SqlCommand command = new SqlCommand();

            // Add parameter
            if (parameters != null)
            {
                foreach (var par in parameters.GetParameters)
                {
                    command.Parameters.Add(new SqlParameter(par.Name, par.Values));
                }
            }

            IList<T> items = new List<T>();
            DataSet dataSet = new DataSet();
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();

                command.Connection = sqlConnection;
                command.CommandTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
                command.CommandType = commandType;
                command.CommandText = commandText;
                dataSet = ExecuteDataSet(command);

                sqlConnection.Close();

                foreach (var row in dataSet.Tables[0].Rows)
                {
                    T item = (T)Activator.CreateInstance(typeof(T), row);
                    items.Add(item);
                }

                return items;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute bulk insert into table
        /// </summary>
        /// <param name="dataTable">Datatable with table name has exist in destination database</param>
        public static void ExecuteBulkInsert(string connectionString, DataTable dataTable)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // make sure to enable triggers
                    // more on triggers in next post
                    SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    // set the destination table name
                    bulkCopy.BulkCopyTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
                    bulkCopy.DestinationTableName = dataTable.TableName;
                    connection.Open();

                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute bulk insert into table
        /// </summary>
        /// <param name="dataTable">Datatable with table name has exist in destination database</param>
        public static void ExecuteBulkInsert(SqlConnection sqlConnection, DataTable dataTable)
        {
            try
            {
                using (SqlConnection connection = sqlConnection)
                {
                    // make sure to enable triggers
                    // more on triggers in next post
                    SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    // set the destination table name
                    bulkCopy.BulkCopyTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
                    bulkCopy.DestinationTableName = dataTable.TableName;
                    connection.Open();

                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dataTable);
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Execute bulk insert into table
        /// </summary>
        /// <param name="dataTable">Datatable with table name has exist in destination database</param>
        public static void ExecuteBulkInsert(SqlTransaction sqlTransaction, DataTable dataTable)
        {
            try
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    sqlTransaction.Connection,
                    SqlBulkCopyOptions.TableLock,
                    sqlTransaction
                    );

                // set the destination table name
                bulkCopy.BulkCopyTimeout = CommandTimeOut == 0 ? DefaultTimeOut : CommandTimeOut;
                bulkCopy.DestinationTableName = dataTable.TableName;
                // write the data in the "dataTable"
                bulkCopy.WriteToServer(dataTable);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void TransactionCreate(string sqlConnecionString)
        {
            CreateSqlConnection(sqlConnecionString);
            if (_sqConnection.State == ConnectionState.Closed)
                _sqConnection.Open();

            _sqlTransaction = _sqConnection.BeginTransaction();
        }

        public static void TransactionAdd(string query)
        {
            ExecuteNonQuery(_sqlTransaction, CommandType.Text, query);
        }

        public static void TransactionCommit()
        {
            if (_sqlTransaction != null)
                _sqlTransaction.Commit();
            if (_sqConnection != null && _sqConnection.State == ConnectionState.Open)
                _sqConnection.Close();
        }

        public static void TransactionRollback()
        {
            if (_sqlTransaction != null)
                _sqlTransaction.Rollback();
            if (_sqConnection != null && _sqConnection.State == ConnectionState.Open)
                _sqConnection.Close();
        }
    }
}
