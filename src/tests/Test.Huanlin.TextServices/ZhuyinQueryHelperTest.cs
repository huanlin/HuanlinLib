using Huanlin.TextServices.Chinese;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test.Huanlin.TextServices
{
    
    
    /// <summary>
    ///This is a test class for ZhuyinQueryHelperTest and is intended
    ///to contain all ZhuyinQueryHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZhuyinQueryHelperTest
    {


        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            ZhuyinQueryHelper.Initialize();
        }
        
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for IsPolyphonic
        ///</summary>
        [TestMethod()]
        public void IsPolyphonicTest()
        {
            string aChar = "的";
            bool expected = true;
            bool actual;
            actual = ZhuyinQueryHelper.IsPolyphonic(aChar);
            Assert.AreEqual(expected, actual);

            aChar = "料";
            expected = false;
            actual = ZhuyinQueryHelper.IsPolyphonic(aChar);
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetZhuyinSymbols
        ///</summary>
        [TestMethod()]
        public void GetZhuyinSymbolsTest()
        {
            string aChar = "料";
            string[] expected = { "ㄌㄧㄠˋ" };
            string[] actual;
            actual = ZhuyinQueryHelper.GetZhuyinSymbols(aChar);
            CollectionAssert.AreEqual(expected, actual);

            aChar = "們";
            expected = new string[] { "ㄇㄣˊ", "ㄇㄣ˙" };
            actual = ZhuyinQueryHelper.GetZhuyinSymbols(aChar);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetZhuyinKeys
        ///</summary>
        [TestMethod()]
        public void GetZhuyinKeysTest()
        {
            string aChar = "料";
            string[] expected = { "xul4" };
            string[] actual;
            actual = ZhuyinQueryHelper.GetZhuyinKeys(aChar);
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
