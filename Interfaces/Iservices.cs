using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolverTakeHomeTest.Interfaces
{
    public interface Iservices
    {
        List<dynamic> GetTableData(string tableName, int offset, int count);
    }
}
