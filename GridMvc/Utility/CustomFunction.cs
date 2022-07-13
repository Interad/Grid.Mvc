using System;
using System.Data.Entity;
using System.Linq.Expressions;

namespace GridMvc.Utility
{
    public static class CustomFunction
    {
        [DbFunction("CodeFirstDatabaseSchema", "JSON_VALUE")]
        public static string JsonValue(string expression, string path)
        {
            // todo: implement filter for in memory data
            throw new NotSupportedException();
        }

        [DbFunction("CodeFirstDatabaseSchema", "ToDateTime2")]
        public static DateTime ToDateTime2(string value, string format)
        {
            // todo: implement filter for in memory data
            throw new NotSupportedException();
        }

        [DbFunction("CodeFirstDatabaseSchema", "ToDecimal")]
        public static decimal ToDecimal(string value)
        {
            // todo: implement filter for in memory data
            throw new NotSupportedException();
        }
    }

}
