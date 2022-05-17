using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using StrategyManager.Common;

namespace StrategyManager.DAL
{
    public class StrategyData
    {
        public static DataTable GetTestData()
        {
            string sql = "SELECT * FROM test.new_table";
            return MySQLHelper.GetDataTable(sql);
        }
    }
}
