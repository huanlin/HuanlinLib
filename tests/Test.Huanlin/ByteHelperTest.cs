using NUnit.Framework;

namespace Test.Huanlin
{
    /// <summary>
    ///This is a test class for Huanlin.Helpers.ByteHelper and is intended
    ///to contain all Huanlin.Helpers.ByteHelper Unit Tests
    ///</summary>
    [TestFixture]
	public class ByteHelperTest
	{
		/// <summary>
		///A test for Reverse (byte)
		///</summary>
		[Test]
		public void ReverseTest()
		{
			byte aByte = 0xb8;
			byte expected = 0x1d;
			byte actual = global::Huanlin.Helpers.ByteHelper.Reverse(aByte);
			Assert.AreEqual(expected, actual, "Huanlin.Helpers.ByteHelper.Reverse 測試失敗!");

			aByte = 0x93;
			expected = 0xc9;
			actual = global::Huanlin.Helpers.ByteHelper.Reverse(aByte);
			Assert.AreEqual(expected, actual, "Huanlin.Helpers.ByteHelper.Reverse 測試失敗!");
		}

	}


}
