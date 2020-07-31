using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolverTakeHomeTest.Interfaces
{
    public interface IServices
    {
        void SetConnectionString(string connectionString);

        List<string> ListOfTablesByConnectionString(string connectionString);

        List<dynamic> GetTableData(string tableName, int offset, int count);
    }
}
