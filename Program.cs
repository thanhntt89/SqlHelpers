using System;

namespace SqlHelpers
{
    static class Program
    {  
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {            

            Select();
        }

        private static void Select()
        {
            ConnectionInfo connectionInfo = new ConnectionInfo()
            {
                ServerName ="THANHPC",
                UserName="sa",
                Password= "@dmin123",
                DatabaseName= "Wii",
                TimeOutCommand = 300,
                TimeOutConnection = 20
            };

            SqlHelpers.CreateConnectionString(connectionInfo);

            string sqlString = "select [プロジェクトID] , [機能ID], [機能名], [タイムスタンプ] FROM [Wii].[dbo].[Fes機能ID] where [プロジェクトID] = @プロジェクトID AND [機能ID] = @機能ID";

            Parameters parameter = new Parameters();
            parameter.Add(new Parameter()
            {
                Name = "@機能ID",
                Values = 110
            });
            parameter.Add(new Parameter()
            {
                Name = "@プロジェクトID",
                Values =1
            });
            

            var dt = SqlHelpers.ExecuteDataset(SqlHelpers.ConnectionString, System.Data.CommandType.Text, sqlString, parameter);
        }
    }
}
