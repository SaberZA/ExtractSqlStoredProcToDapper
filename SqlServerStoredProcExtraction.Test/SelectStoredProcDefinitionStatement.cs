using Kc.DbFactory;

namespace SqlServerStoredProcExtraction.Test
{
    public class SelectStoredProcDefinitionStatement : ISqlStatement
    {
        public string GetSqlStatement()
        {
            return @"
                    select p.[type] as 'ObjectType'
                          ,p.[name] as 'ObjectName'
                          ,c.[definition] as 'ObjectDefintion'
                      from sys.objects p
                      join sys.sql_modules c
                        on p.object_id = c.object_id
                     where p.[type] = 'P'
                    ";
        }
    }
}