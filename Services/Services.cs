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
    public class Services : Iservices
    {

        private readonly IConfiguration _configuration;

        public Services(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<dynamic> GetTableData(string tableName, int offset, int count)
        {
            List<dynamic> returnData = null;

            try
            {
                var connectionString = _configuration.GetSection("someConnectionString").Value;

                //"Server=MANI-DELL\\SQLEXPRESS;Initial Catalog=AdventureWorksDW;Trusted_Connection=True;MultipleActiveResultSets=true";//"Server=(localdb)\v11.0;Integrated Security=true";

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
