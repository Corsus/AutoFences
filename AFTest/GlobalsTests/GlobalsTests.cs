using NUnit.Framework;
using System;
using AFLib;



namespace AFTest
{
    [TestFixture ()]
    /*
    * <summary>Tests simple case of global instance passing </summary>
    */
    public class GlobalsTests
    {
        Globals instance;

        [SetUp]
        public void GlobalsTestSetup(){

        }

        //Test that instance is null
        [Test ()]
        public void TestInstanceNull ()
        {
            Assert.IsNotNull(this, instance); 

        }

        //Test that instance is correct
        [Test ()]
        public void TestInstance ()
        {
            Assert.IsInstanceOfType (this, instance);

        }
            

    }
}

