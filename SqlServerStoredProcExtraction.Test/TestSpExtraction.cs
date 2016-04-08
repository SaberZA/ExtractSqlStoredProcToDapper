using System;
using System.Collections.Generic;
using System.Linq;
using Kc.DbFactory;

namespace SqlServerStoredProcExtraction.Test
{
    using NUnit.Framework;

    [TestFixture]
    public class TestSpExtraction
    {
        private static string _connectionString = @"Data Source=toronto;Initial Catalog=BWSubsWeb;Integrated Security=True";
        private static SqlDbFactory sqlDbFactory = new SqlDbFactory(_connectionString);

        [Test]
        public void TestGetSpDefinition()
        {
            //---------------Set up test pack-------------------
            
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var storedProcDefinitions = GetStoredProcDefinitions();
            //---------------Test Result -----------------------
            Assert.IsTrue(storedProcDefinitions.Any());
        }
        
        [Test]
        public void ExtractParameters_GivenSpDefinitions_ShouldReturnOneParameter()
        {
            //---------------Set up test pack-------------------
            var storedProcDefinitions = GetStoredProcDefinitions();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------

            //---------------Test Result -----------------------
            Assert.IsTrue(storedProcDefinitions[0].Parameters.Any());
            Assert.IsTrue(storedProcDefinitions[5].Parameters.Any());
            Assert.IsTrue(storedProcDefinitions[10].Parameters.Any());
            Assert.IsFalse(storedProcDefinitions[62].Parameters.Any());
            Assert.IsTrue(storedProcDefinitions[100].Parameters.Any());
            Assert.IsTrue(storedProcDefinitions[120].Parameters.Any());
            Assert.IsTrue(storedProcDefinitions[150].Parameters.Any());
            Assert.IsTrue(storedProcDefinitions[500].Parameters.Any());
        }

        [Test]
        public void ExtractCore_GivenSpDefinitions_ShouldReturnStoredProcCore()
        {
            //---------------Set up test pack-------------------
            var storedProcDefinitions = GetStoredProcDefinitions();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var storedProcCore = storedProcDefinitions[5].ObjectCore;
            //---------------Test Result -----------------------
            Assert.IsTrue(!storedProcCore.ToLower().StartsWith("begin") && !storedProcCore.ToLower().StartsWith("as"));
            Assert.IsTrue(!storedProcCore.ToLower().EndsWith("end"));
        }

        [Test]
        public void GetParameterClrType_GivenSpDefinitions_ShouldReturnMatchingClrTypeFromDbType()
        {
            //---------------Set up test pack-------------------
            var storedProcDefinitions = GetStoredProcDefinitions();
            //---------------Assert Precondition----------------

            //---------------Execute Test ----------------------
            var storedProcsWithDatetimeParams = storedProcDefinitions.Where(p => p.Parameters.Any(x => x.Data_Type.ToLower() == "datetime")).ToList();
            var storedProcsWithIntParams = storedProcDefinitions.Where(p => p.Parameters.Any(x => x.Data_Type.ToLower() == "int")).ToList();
            var storedProcsWithStringParams = storedProcDefinitions.Where(p => p.Parameters.Any(x => x.Data_Type.ToLower().Contains("varchar"))).ToList();

            var firstDateTimeParameter = storedProcsWithDatetimeParams[0].Parameters.FirstOrDefault(p => p.Data_Type.ToLower() == "datetime");
            var firstIntParameter = storedProcsWithIntParams[0].Parameters.FirstOrDefault(p => p.Data_Type.ToLower() == "int");
            var firstStringParameter = storedProcsWithStringParams[0].Parameters.FirstOrDefault(p => p.Data_Type.ToLower().Contains("varchar"));
            //---------------Test Result -----------------------
            Assert.AreEqual(typeof(int?), firstIntParameter.ParameterClrType);
            Assert.AreEqual(typeof(string), firstStringParameter.ParameterClrType);
            Assert.AreEqual(typeof(DateTime?), firstDateTimeParameter.ParameterClrType);
        }

        private static List<StoredProcDefinition> GetStoredProcDefinitions()
        {
            var selectStoredProcDefinitionStatement = new SelectStoredProcDefinitionStatement();
            var storedProcDefinitions = sqlDbFactory.Query<StoredProcDefinition>(selectStoredProcDefinitionStatement);

            foreach (var storedProcDefinition in storedProcDefinitions)
            {
                var selectStoredProcParametersStatement = new SelectStoredProcParametersStatement(storedProcDefinition.ObjectName);
                var sqlSpParameters = sqlDbFactory.Query<SqlSpParameter>(selectStoredProcParametersStatement);
                storedProcDefinition.Parameters = sqlSpParameters;
                storedProcDefinition.BootstrapStoredProc();
            }
            return storedProcDefinitions;
        }
    }
}
