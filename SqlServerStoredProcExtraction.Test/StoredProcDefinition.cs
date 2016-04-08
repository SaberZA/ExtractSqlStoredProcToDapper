using System;
using System.Collections.Generic;
using System.Linq;
using Kc.DbFactory;
using NUnit.Framework;

namespace SqlServerStoredProcExtraction.Test
{
    public class StoredProcDefinition
    {
        public string ObjectType { get; set; }
        public string ObjectName { get; set; }
        public List<SqlSpParameter> Parameters { get; set; }
        public string ObjectCore { get; set; }

        public string ObjectDefintion { get; set; }

        public void BootstrapStoredProc()
        {
            //Parameters = CreateParametersFromDefinition();
            ObjectCore = CreateObjectCore();
        }
        
        private string CreateObjectCore()
        {
            var endIndexOfParameterBlock = ObjectDefintion.IndexOf("BEGIN", System.StringComparison.InvariantCultureIgnoreCase);
            if (endIndexOfParameterBlock == -1)
            {
                endIndexOfParameterBlock = ObjectDefintion.IndexOf("AS", System.StringComparison.InvariantCultureIgnoreCase);
            }


            var objectCore = ObjectDefintion.Substring(endIndexOfParameterBlock,
                ObjectDefintion.Length - endIndexOfParameterBlock);

            var beginWord = "begin";
            if (objectCore.ToLower().StartsWith(beginWord))
            {
                objectCore = objectCore.Substring(beginWord.Length, objectCore.Length - beginWord.Length);
            }

            var endWord = "end";
            if (objectCore.ToLower().EndsWith(endWord))
            {
                objectCore = objectCore.Substring(0, objectCore.Length - endWord.Length);
            }
            return objectCore;
        }
    }

    public class SelectStoredProcParametersStatement : ISqlStatement
    {
        public string StoredProcedureName { get; set; }

        public SelectStoredProcParametersStatement(string storedProcedureName)
        {
            StoredProcedureName = storedProcedureName;
        }
        
        public string GetSqlStatement()
        {
            return @"select * from information_schema.parameters
            where specific_name = @StoredProcedureName";
        }
    }

    public class SqlSpParameter
    {
        public string Parameter_Name { get; set; }
        public string Data_Type { get; set; }
        public string Parameter_Mode { get; set; }

        public Type ParameterClrType
        {
            get { return SqlServerDataTypeHelper.GetClrType(Data_Type); }
        }

        public string ParameterPropertyName
        {
            get { return Parameter_Name.Replace("@", ""); }
        }
    }
}