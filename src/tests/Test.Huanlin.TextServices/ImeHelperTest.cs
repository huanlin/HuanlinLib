using Huanlin.TextServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace Test.Huanlin.TextServices
{
    
    
    /// <summary>
    ///This is a test class for ImeHelperTest and is intended
    ///to contain all ImeHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class ImeHelperTest
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
        ///A test for GetBopomofo
        ///</summary>
        [TestMethod()]
        public void GetBopomofoTest()
        {
            string aChineseText = "甚麼";
            string[] expected = { "ㄕ　ㄜˊ", "ㄇ　ㄛ˙" };
            string[] actual;
            actual = ImeHelper.GetBopomofo(aChineseText);            
            CollectionAssert.AreEqual(expected, actual);

            // 測試一串中文字中夾雜非中文字的結果。
            aChineseText = "我是 M，我35歲";
            expected = new string[] {"　ㄨㄛˇ", "ㄕ　　ˋ", "　　　　", "　　　　", "　　　　", "　ㄨㄛˇ", "　　　　", "　　　　", "ㄙㄨㄟˋ"};
            actual = ImeHelper.GetBopomofo(aChineseText);
            CollectionAssert.AreEqual(expected, actual);


            // 測試傳入注音符號，一樣也會傳回注音符號。
            aChineseText = "ㄅ";
            expected = new string[] {"ㄅ　　ˉ"};
            actual = ImeHelper.GetBopomofo(aChineseText);
            CollectionAssert.AreEqual(expected, actual);

            // 測試簡體字。
            aChineseText = "实";
            expected = new string[] {"ㄕ　　ˊ"};
            actual = ImeHelper.GetBopomofo(aChineseText);
            CollectionAssert.AreEqual(expected, actual);

            // 測試私名號
            aChineseText = "＿＿";
            expected = new string[] {};
            actual = ImeHelper.GetBopomofo(aChineseText);
            CollectionAssert.AreEqual(expected, actual);
        }

        /// <summary>
        ///A test for GetBopomofoWithPhraseTable
        ///</summary>
        [TestMethod()]
        public void GetBopomofoWithPhraseTableTest()
        {
            // 手動加一個片語，以便觀察是否查詢字根時是否有用到片語資料。
            ZhuyinPhraseTable.GetInstance().AddPhrase("甚麼 ㄕㄚˊ ㄇㄚ˙");

            string aChineseText = "甚麼";
            string[] expected = { "ㄕ　ㄚˊ", "ㄇ　ㄚ˙" }; 
            string[] actual;
            actual = ImeHelper.GetBopomofoWithPhraseTable(aChineseText);
            CollectionAssert.AreEqual(expected, actual);            
        }

    }
}
