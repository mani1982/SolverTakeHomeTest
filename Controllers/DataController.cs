using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolverTakeHomeTest.Interfaces;

namespace SolverTakeHomeTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DataController : ControllerBase
    {

        private readonly ILogger<DataController> _logger;
        private readonly Iservices _services;

        public DataController(ILogger<DataController> logger,Iservices services)
        {
            _logger = logger;
            _services = services;
        }

       
        [Route("{tableName}")]
        [HttpGet()]
        public IActionResult GetTableData(string tableName, int offset = 0, int count=1)
        {
            try
            {

                List<dynamic> returnData = new List<dynamic>();

                //// Build connection string
                //SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                //builder.DataSource = "localhost";   // update me
                //builder.UserID = "MANI-DELL\\manij";              // update me
                ////builder.Password = "your_password";      // update me
                ////builder.InitialCatalog = "AdventureWorksDW";
                //builder.IntegratedSecurity = true;

                //var connectionString = "Server=MANI-DELL\\SQLEXPRESS;Initial Catalog=AdventureWorksDW;Trusted_Connection=True;MultipleActiveResultSets=true";//"Server=(localdb)\v11.0;Integrated Security=true";

                //// Connect to SQL
                //Console.Write("Connecting to SQL Server ... ");
                ////using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
                //using (SqlConnection connection = new SqlConnection(connectionString))
                //{
                //    SqlCommand com = new SqlCommand($"select count(*) as count from dbo.{tableName}", connection);
                //    connection.Open();

                //    var reader = com.ExecuteReader();
                //    DataTable dt = new DataTable();

                //    dt.Load(reader);

                //    var rowsCount = (int)dt.Rows[0]["count"];

                //    if(rowsCount <= offset)
                //    {
                //        return BadRequest("The offset is more than row counts.");
                //    }

                //    com = new SqlCommand($"select * from dbo.{tableName} order by 1 offset {offset} rows fetch next {count} rows only", connection);

                //    returnData = new List<dynamic>();


                //    //reader = new SqlDataReader();
                //    reader = com.ExecuteReader();

                //    dt = new DataTable();
                //    dt.Load(reader);

                //    foreach (DataRow dr in dt.Rows)
                //    {
                //        var dic = new Dictionary<string, dynamic>();
                //        foreach (DataColumn col in dr.Table.Columns)
                //        {
                //            string name = col.ColumnName.ToLower();

                //            dic.Add(name, dr[name] != DBNull.Value ? Convert.ChangeType(dr[name], col.DataType): null);
                //        }

                //        returnData.Add(dic);
                //    }
                //}
                returnData = _services.GetTableData(tableName,offset, count);
                return Ok(returnData);

            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
