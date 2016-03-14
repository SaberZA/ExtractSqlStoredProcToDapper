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

        private static List<StoredProcDefinition> GetStoredProcDefinitions()
        {
            var selectStoredProcDefinitionStatement = new SelectStoredProcDefinitionStatement();
            var storedProcDefinitions = sqlDbFactory.Query<StoredProcDefinition>(selectStoredProcDefinitionStatement);
            return storedProcDefinitions;
        }
    }
}
