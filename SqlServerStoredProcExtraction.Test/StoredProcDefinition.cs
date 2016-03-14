using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SqlServerStoredProcExtraction.Test
{
    public class StoredProcDefinition
    {
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public string ObjectDefintion { get; set; }

        public List<SqlSpParameter> Parameters
        {
            get
            {
                var startIndex = ObjectDefintion.IndexOf("CREATE", System.StringComparison.InvariantCultureIgnoreCase);
                var endIndexOfParameterBlock = ObjectDefintion.IndexOf("BEGIN", System.StringComparison.InvariantCultureIgnoreCase);
                if (endIndexOfParameterBlock == -1)
                {
                    endIndexOfParameterBlock = ObjectDefintion.IndexOf("AS", System.StringComparison.InvariantCultureIgnoreCase);
                }
                var parameterBlock = ObjectDefintion.Substring(startIndex, endIndexOfParameterBlock - startIndex);
                var firstParamIndex = parameterBlock.IndexOf("@", StringComparison.InvariantCultureIgnoreCase);

                if (firstParamIndex == -1)
                {
                    return new List<SqlSpParameter>();
                }

                var lastIndexOfAsStatement = parameterBlock.LastIndexOf("AS",
                    StringComparison.InvariantCultureIgnoreCase);

                if (lastIndexOfAsStatement == -1)
                {
                    lastIndexOfAsStatement = parameterBlock.Length - 1;
                }


                var eachParamBlock = parameterBlock.Substring(firstParamIndex, lastIndexOfAsStatement - firstParamIndex);


                var parameters = eachParamBlock.Split(new string[] { ",", "\r\n"}, StringSplitOptions.RemoveEmptyEntries).ToList();

                var filteredParams = new List<string>();
                foreach (var parameter in parameters)
                {
                    var trim = parameter.Trim();
                    if (trim.Contains("--"))
                    {
                        var commentIndex = trim.IndexOf("--", StringComparison.InvariantCultureIgnoreCase);
                        var trimmedOffComment = trim.Substring(0, commentIndex);
                        trim = trimmedOffComment;
                    }

                    if (trim.Length < 5)
                    {
                        continue;
                    }

                    if (!trim.Contains("@"))
                    {
                        continue;
                    }


                    filteredParams.Add(trim);
                }

                var sqlSpParameters = new List<SqlSpParameter>();

                foreach (var parameter in filteredParams)
                {
                    var paramAndType = parameter.Split(new string[] {" ", "\t"}, StringSplitOptions.RemoveEmptyEntries).ToList();
                    var sqlSpParameter = new SqlSpParameter() {ParameterName = paramAndType[0], ParameterType = paramAndType[1]};
                    sqlSpParameters.Add(sqlSpParameter);
                }
                
                return sqlSpParameters;
            }
        }
    }

    public class SqlSpParameter
    {
        public string ParameterName { get; set; }
        public string ParameterType { get; set; }
    }
}