using Huanlin.TextServices.Chinese;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test.Huanlin.TextServices
{
    
    
    /// <summary>
    ///This is a test class for ZhuyinTest and is intended
    ///to contain all ZhuyinTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ZhuyinTest
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
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
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
        ///A test for ParseKeyString
        ///</summary>
        [TestMethod()]
        public void ParseKeyStringTest()
        {
            string zhuyinKeys = "wu0";
            Zhuyin expected = new Zhuyin("ㄊㄧㄢ");
            Zhuyin actual;
            actual = Zhuyin.ParseKeyString(zhuyinKeys);
            Assert.AreEqual(expected, actual);

            zhuyinKeys = "2k7";
            expected = new Zhuyin("ㄉㄜ" + Zhuyin.Tone0Char);  // 的
            actual = Zhuyin.ParseKeyString(zhuyinKeys);
            Assert.AreEqual(expected, actual);
        }


        /// <summary>
        ///A test for ToString
        ///</summary>
        [TestMethod()]
        public void ToStringTest()
        {
            Zhuyin target = new Zhuyin("ㄊㄧㄢ");
            string expected = "ㄊㄧㄢ" + Zhuyin.Tone1Char;
            string actual;
            actual = target.ToString();
            Assert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetTone
        ///</summary>
        [TestMethod()]
        public void GetToneTest()
        {
            string zhuyinKeys = "ql3";
            ZhuyinTone expected = ZhuyinTone.Tone3;
            ZhuyinTone actual;
            actual = Zhuyin.GetTone(zhuyinKeys);
            Assert.AreEqual(expected, actual);
        }
    }
}
