using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
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
        private readonly IServices _services;

        public DataController(ILogger<DataController> logger,IServices services)
        {
            _logger = logger;
            _services = services;
        }

        //https://localhost:44360/data/SetConnectionString?connectionString=Server=MANI-DELL\SQLEXPRESS;Trusted_Connection=True;MultipleActiveResultSets=true
        [HttpGet("SetConnectionString")]
        public IActionResult SetConnectionString([FromQuery]string connectionString)
        {
            try
            {
                if (!string.IsNullOrEmpty(connectionString))
                {
                    _services.SetConnectionString(connectionString);
                }

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //https://localhost:44360/data/ListOfDBByConnectionString?connectionString=Server=MANI-DELL\SQLEXPRESS;Trusted_Connection=True;MultipleActiveResultSets=true
        [HttpGet("listOfDBbyConnectionString")]
        public IActionResult ListOfDBByConnectionString(string connectionString)
        {
            try
            {
                List<string> tablesName = new List<string>();
                if (!string.IsNullOrEmpty(connectionString)) {
                    tablesName = _services.ListOfTablesByConnectionString(connectionString);
                }

                if(tablesName.Count == 0)
                {
                    throw new Exception("No Tables Found for this connection string");
                }

                return Ok(tablesName);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //https://localhost:44360/data/DimOrganization?offset=10&count=1
        [Route("{tableName}")]
        [HttpGet()]
        public IActionResult GetTableData(string tableName, int offset = 0, int count=1)
        {
            try
            {

                List<dynamic> returnData = new List<dynamic>();

                if (!string.IsNullOrEmpty(tableName))
                {
                    returnData = _services.GetTableData(tableName, offset, count);
                }
                return Ok(returnData);

            }
            catch (SqlException e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}
