using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;

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

        public static void BindParameters(this SqlParameterCollection parameterCollection, Dictionary<string, object> parameters)
        {
            foreach (KeyValuePair<string, object> parameter in parameters)
            {
                parameterCollection.AddWithValueOrDBNull(parameter.Key, parameter.Value);
            } 
        }

        public static void BindParameters(this SqlParameterCollection parameterCollection, object obj)
        {
            BindParameters(parameterCollection, obj, new string[0]);
            /*
            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach(PropertyInfo prop in properties)
            {
                string paramName = prop.Name;
                object paramValue = prop.GetValue(obj);

                parameterCollection.AddWithValueOrDBNull(paramName, paramValue);
            }
            */
        }
        public static void BindParameters(this SqlParameterCollection parameterCollection, object obj, string[] propertiesToExclude)
        {

            if (parameterCollection == null)
                throw new ArgumentException(nameof(parameterCollection));

            // if (obj == null)
                // throw new ArgumentException("obj");

            if (propertiesToExclude == null)
                throw new ArgumentException(nameof(propertiesToExclude));

            if (obj == null)
                return;

            PropertyInfo[] properties = obj.GetType().GetProperties();

            foreach(PropertyInfo prop in properties)
            {
                string paramName = prop.Name;

                if (propertiesToExclude.Contains(paramName))
                    continue;

                object paramValue = prop.GetValue(obj);

                parameterCollection.AddWithValueOrDBNull(paramName, paramValue);
            }
        }
    }
}