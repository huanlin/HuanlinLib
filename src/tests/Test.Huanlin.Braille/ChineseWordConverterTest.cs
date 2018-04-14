﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using System.Collections.Generic;
using Huanlin.Braille.Converters;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NChinese.Phonetic;

namespace Huanlin.Braille.UnitTest
{
    /// <summary>
    ///This is a test class for Huanlin.Braille.ChineseWordConverter and is intended
    ///to contain all Huanlin.Braille.ChineseWordConverter Unit Tests
    ///</summary>
    [TestClass()]
	public class ChineseWordConverterTest
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
		//
		//[ClassInitialize()]
		//public static void MyClassInitialize(TestContext testContext)
		//{
		//}
		//
		//Use ClassCleanup to run code after all tests in a class have run
		//
		//[ClassCleanup()]
		//public static void MyClassCleanup()
		//{
		//}
		//
		//Use TestInitialize to run code before running each test
		//
		//[TestInitialize()]
		//public void MyTestInitialize()
		//{
		//}
		//
		//Use TestCleanup to run code after each test has run
		//
		//[TestCleanup()]
		//public void MyTestCleanup()
		//{
		//}
		//
		#endregion


		/// <summary>
		///A test for Convert (Stack&lt;char&gt;, BrailleWord)
		///</summary>
		[TestMethod()]
		public void ConvertTest()
		{
            string msg = "ChineseWordConverter.Convert 測試失敗: ";

			var target = new ChineseWordConverter(
                new ZhuyinReverseConverter(new ZhuyinReverseConversionProvider()));

			ContextTagManager context = new ContextTagManager();

			// 測試結合韻。
			string text = "我";
			Stack<char> charStack = new Stack<char>(text);
			List<BrailleWord> expected = new List<BrailleWord>();
            BrailleWord brWord = new BrailleWord(text, "　ㄨㄛˇ", "1208");
            expected.Add(brWord);
			List<BrailleWord> actual = target.Convert(charStack, context);
            
			CollectionAssert.AreEqual(expected, actual, msg + text);
			charStack.Clear();

			// 測試單音字：要在音調記號前加一個空方。
			text = "智";
			charStack = new Stack<char>(text);
            brWord = new BrailleWord(text, "ㄓ　　ˋ", "013110");
			expected.Clear();
            expected.Add(brWord);
			actual = target.Convert(charStack, context);
            CollectionAssert.AreEqual(expected, actual, msg + text);
			charStack.Clear();

			// 測試無特殊規則的注音。
			text = "你";
			charStack = new Stack<char>(text);
			brWord = new BrailleWord(text, "ㄋㄧ　ˇ", "1D2108");
            expected.Clear();
            expected.Add(brWord);
			actual = target.Convert(charStack, context);
            CollectionAssert.AreEqual(expected, actual, msg + text);
			charStack.Clear();

			// 測試標點符號。
			text = "：";
			charStack = new Stack<char>(text);
            brWord = new BrailleWord(text, "　　　ˉ", "1212");
            expected.Clear();
            expected.Add(brWord);
			actual = target.Convert(charStack, context);
            CollectionAssert.AreEqual(expected, actual, msg + text);
			charStack.Clear();

            // 測試全形空白
            text = "　";
            charStack = new Stack<char>(text);
            brWord = new BrailleWord(text, "　　　ˉ", "00");
            expected.Clear();
            expected.Add(brWord);
            actual = target.Convert(charStack, context);
            CollectionAssert.AreEqual(expected, actual, msg + text);
            charStack.Clear();

            // 測試簡體字。
            text = "实";
            charStack = new Stack<char>(text);
            brWord = new BrailleWord(text, "ㄕ　　ˊ", "0A3102");
            expected.Clear();
            expected.Add(brWord);
            actual = target.Convert(charStack, context);
            CollectionAssert.AreEqual(expected, actual, msg + text);
            charStack.Clear();

            // 測試無法轉換的字元：／
            text = "／";
            charStack = new Stack<char>(text);
            expected = null;
            actual = target.Convert(charStack, context);
            CollectionAssert.AreEqual(expected, actual, msg + text);
            charStack.Clear();
        }

	}


}
