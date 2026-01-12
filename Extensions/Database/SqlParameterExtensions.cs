using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;

namespace ProgettoProva.web.Extensions.Database
{
    public static class SqlParameterExtensions
    {
        public static void AddWithValueOrDBNull(this SqlParameterCollection parameters, string parameterName, object value)
        {
            if (value == null)
            {
                parameters.AddWithValue(parameterName, DBNull.Value);
            }
            else
            {
                parameters.AddWithValue(parameterName, value);
            }

            // Uguale all'operazione ternaria
            // parameters.AddWithValue(parameterName, value ?? DBNull.Value)
        }
        private static void BindParameters(this SqlParameterCollection parameterCollection, Dictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                parameterCollection.AddWithValueOrDBNull(parameter.Key, parameter.Value);
            } 
        }
    }
}