using Huanlin.Common.Cryptography;
using Huanlin.Collections;
using NUnit.Framework;
using System.Text;

namespace Huanlin.Common.Tests.Cryptography;

[TestFixture]
public class SymmetricCryptoTest
{
    [Test]
    public void EncryptDecryptTest_Rijndael()
    {
        SymmetricCrypto crypto = new SymmetricCrypto(SymmetricCrypto.Provider.Rijndael, false);
        string originalText = "Hello, this is a secret message.";
        byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

        // Encrypt
        byte[] encryptedBytes = crypto.Encrypt(originalBytes);

        // Decrypt
        byte[] decryptedBytes = crypto.Decrypt(encryptedBytes);
        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        Assert.That(decryptedText, Is.EqualTo(originalText));
    }

    [Test]
    public void EncryptDecryptTest_TripleDES()
    {
        SymmetricCrypto crypto = new SymmetricCrypto(SymmetricCrypto.Provider.TripleDES, false);
        string originalText = "Another secret message.";
        byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

        // Encrypt
        byte[] encryptedBytes = crypto.Encrypt(originalBytes, crypto.Key.Bytes);

        // Decrypt
        byte[] decryptedBytes = crypto.Decrypt(encryptedBytes, crypto.Key.Bytes);
        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        Assert.That(decryptedText, Is.EqualTo(originalText));
    }

    [Test]
    public void EncryptDecryptTest_DES()
    {
        SymmetricCrypto crypto = new SymmetricCrypto(SymmetricCrypto.Provider.DES, false);
        string originalText = "Short message.";
        byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

        // Encrypt
        byte[] encryptedBytes = crypto.Encrypt(originalBytes, crypto.Key.Bytes);

        // Decrypt
        byte[] decryptedBytes = crypto.Decrypt(encryptedBytes, crypto.Key.Bytes);
        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        Assert.That(decryptedText, Is.EqualTo(originalText));
    }

    [Test]
    public void EncryptDecryptTest_RC2()
    {
        SymmetricCrypto crypto = new SymmetricCrypto(SymmetricCrypto.Provider.RC2, false);
        string originalText = "RC2 test string.";
        byte[] originalBytes = Encoding.UTF8.GetBytes(originalText);

        // Encrypt
        byte[] encryptedBytes = crypto.Encrypt(originalBytes, crypto.Key.Bytes);

        // Decrypt
        byte[] decryptedBytes = crypto.Decrypt(encryptedBytes, crypto.Key.Bytes);
        string decryptedText = Encoding.UTF8.GetString(decryptedBytes);

        Assert.That(decryptedText, Is.EqualTo(originalText));
    }
}