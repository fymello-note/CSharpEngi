using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Extensions.Database
{
    public static class SqlCommandExtensions
    {
        public static void AddParameterWithValue(this SqlCommand command, string parameterName, object value)
        {
            if(value == null)
            {
                command.Parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue(parameterName, value);
            }
        }
    }
}