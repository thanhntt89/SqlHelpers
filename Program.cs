using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlHelpers
{
    static class Program
    {
        public static string connectionString = string.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};Connect Timeout={4};", "thanhnt", "Wii", "sa", "@dmin123", 30);


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
            

            var dt = SqlHelpers.ExecuteDataset(connectionString, System.Data.CommandType.Text, sqlString, parameter);
        }
    }
}
