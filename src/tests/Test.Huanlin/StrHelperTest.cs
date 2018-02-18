﻿// The following code was generated by Microsoft Visual Studio 2005.
// The test owner should check each test for validity.
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Text;
using System.Collections.Generic;

namespace Test.Huanlin
{
	/// <summary>
	///This is a test class for Huanlin.Helpers.StrHelper and is intended
	///to contain all Huanlin.Helpers.StrHelper Unit Tests
	///</summary>
	[TestClass()]
	public class StrHelperTest
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
		///A test for IsEmpty (string)
		///</summary>
		[TestMethod()]
		public void IsEmptyTest()
		{
			string input = " \t　\r\n";

			bool expected = true;
            bool actual = global::Huanlin.Text.StrHelper.IsEmpty(input);

            Assert.AreEqual(expected, actual, "global::Huanlin.Text.StrHelper.IsEmpty did not return the expected value.");
		}

		/// <summary>
		///A test for RemoveSpaces (string, bool)
		///</summary>
		[TestMethod()]
		public void RemoveSpacesTest()
		{
			string input = "a b　c"; 
			bool fullShapeSpaces = true;

			string expected = "abc";
			string actual;

            actual = global::Huanlin.Text.StrHelper.RemoveSpaces(input, fullShapeSpaces);

            Assert.AreEqual(expected, actual, "global::Huanlin.Text.StrHelper.RemoveSpaces did not return the expected value.");
		}

		/// <summary>
		///A test for FullShapeSpaceToSpace (string)
		///</summary>
		[TestMethod()]
		public void FullShapeSpaceToSpaceTest()
		{
			string input = "a　b"; 

			string expected = "a b";
			string actual;

            actual = global::Huanlin.Text.StrHelper.FullShapeSpaceToSpace(input);

            Assert.AreEqual(expected, actual, "global::Huanlin.Text.StrHelper.FullShapeSpaceToSpace did not return the expected value" +
					".");
		}
	}


}
