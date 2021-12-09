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
    }

}
