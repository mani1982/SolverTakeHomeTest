using Microsoft.Extensions.Configuration;
using SolverTakeHomeTest.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SolverTakeHomeTest.Services
{
    public class Services : IServices
    {

        private readonly IConfiguration _configuration;

        public Services(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private static string ConnectionString { get; set; }


        public void SetConnectionString(string connectionString)
        {
            using SqlConnection connection = new SqlConnection(connectionString.ToString());
            try
            {
                connection.Open();
                ConnectionString = connectionString;
            }
            catch (Exception e)
            {
                throw new Exception("Connection string is not correct or cannot connect to DB server");
            }
        }

        public List<string> ListOfTablesByConnectionString(string connectionString)
        {
            var tablesName = new List<string>();

            using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
            {
                try
                {
                    connection.Open();
                    ConnectionString = connectionString;

                }catch(Exception e)
                {
                    throw new Exception("Connection string is not correct or cannot connect to DB server");
                }

                SqlCommand comm = new SqlCommand("select name from sys.databases", connection);

                IDataReader reader = comm.ExecuteReader();

                while (reader.Read())
                {
                    tablesName.Add(reader[0].ToString());
                }

                reader.Close();

            }

            return tablesName;
        }

        public List<dynamic> GetTableData(string tableName, int offset, int count)
        {
            List<dynamic> returnData = null;

            try
            {
                var connectionString = _configuration.GetSection("someConnectionString").Value;

                if (string.IsNullOrEmpty(connectionString) && string.IsNullOrEmpty(ConnectionString))
                {
                    throw new Exception("Either inject the connection string to appSettings or use SetConnectionString endPoint to Set connectionString");
                }
                else
                {
                    connectionString = ConnectionString;
                }

                // Connect to SQL
                using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Can not connect to database");
                    }

                    //try to get row count on table to compare it with offset, if offset is greater than rowcount no rows would be return so better notify the user
                    SqlCommand com = new SqlCommand($"select count(*) as count from dbo.{tableName}", connection);

                    var reader = com.ExecuteReader();
                    DataTable dt = new DataTable();

                    dt.Load(reader);

                    var rowsCount = (int)dt.Rows[0]["count"];

                    if (rowsCount <= offset)
                    {
                        throw new Exception("The offset is more than row counts.");
                    }

                    com = new SqlCommand($"select * from dbo.{tableName} order by 1 offset {offset} rows fetch next {count} rows only", connection);

                    returnData = new List<dynamic>();


                    reader = com.ExecuteReader();

                    dt = new DataTable();
                    dt.Load(reader);

                    foreach (DataRow dr in dt.Rows)
                    {
                        var dic = new Dictionary<string, dynamic>();
                        foreach (DataColumn col in dr.Table.Columns)
                        {
                            string name = col.ColumnName.ToLower();

                            dic.Add(name, dr[name] != DBNull.Value ? Convert.ChangeType(dr[name], col.DataType) : null);
                        }

                        returnData.Add(dic);
                    }

                    reader.Close();
                }
            }catch(Exception e)
            {
                throw new Exception(e.Message, e.InnerException);
            }

            return returnData;
        }
    }
}
