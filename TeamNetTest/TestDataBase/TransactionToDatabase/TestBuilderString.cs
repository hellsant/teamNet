using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TeamNetDB.TransactionToDatabase;
using System.Collections.Generic;

namespace TeamNetDBTest.TestDataBase.TransactionToDatabase
{
    [TestClass]
    public class TestBuilderString
    {
        /// <summary>
        /// Verifies if sending a simple string is correct.
        /// </summary>
        [TestMethod]
        public void TestStructureStringSingle()
        {
            BuilderString builderString = new BuilderString();
            string actual = builderString.StructureString("He", this.GetConditionsValue());
            string expected = "He Hi = 'Ho' ";
            Assert.AreEqual(expected, actual);
        }

        
        /// <summary>
        /// Verifies if sending a composite string is correct.
        /// </summary>
        [TestMethod]
        public void TestStructureStringComposed()
        {
            BuilderString builderString = new BuilderString();
            List<ConditionValue> values = new List<ConditionValue>();
            values.Add(this.GetConditionsValue()[0]);
            values.Add(this.GetConditionsValue()[0]);
            string actual = builderString.StructureString("He", values);
            string expected = "He Hi = 'Ho'  and  Hi = 'Ho' ";
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        /// Private method that is responsible for creating the conditions database.
        /// </summary>
        /// <returns></returns>
        private List<ConditionValue> GetConditionsValue()
        {
            List<ConditionValue> values = new List<ConditionValue>();
            values.Add(new ConditionValue(){ ConditionColumn="Hi", ValueCondition="Ho"});
            return values;
        }
    }
}
