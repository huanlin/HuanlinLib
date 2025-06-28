using Huanlin.Common.Cryptography;
using Huanlin.Collections;
using NUnit.Framework;
using System.Text;

namespace Huanlin.Common.Tests.Cryptography;

[TestFixture]
public class HashCryptoTest
{
    [Test]
    public void MD5HashTest()
    {
        HashCrypto hashCrypto = new HashCrypto(HashCrypto.Provider.MD5);
        string input = "Hello World!";
        ByteArray data = new ByteArray(input);
        ByteArray hash = hashCrypto.Calculate(data);
        
        // Expected MD5 hash for "Hello World!" is "ed076287532e863e120c509aab564436"
        // You can verify this using online MD5 hash generators.
        string expectedHash = "ED076287532E86365E841E92BFC50D8C";

        Assert.That(hash.HexString, Is.EqualTo(expectedHash));
    }

    [Test]
    public void SHA1HashTest()
    {
        HashCrypto hashCrypto = new HashCrypto(HashCrypto.Provider.SHA1);
        string input = "Hello World!";
        ByteArray data = new ByteArray(input);
        ByteArray hash = hashCrypto.Calculate(data);

        string expectedHash = "2EF7BDE608CE5404E97D5F042F95F89F1C232871";

        Assert.That(hash.HexString, Is.EqualTo(expectedHash));
    }

    [Test]
    public void SHA256HashTest()
    {
        HashCrypto hashCrypto = new HashCrypto(HashCrypto.Provider.SHA256);
        string input = "Hello World!";
        ByteArray data = new ByteArray(input);
        ByteArray hash = hashCrypto.Calculate(data);

        string expectedHash = "7F83B1657FF1FC53B92DC18148A1D65DFC2D4B1FA3D677284ADDD200126D9069";

        Assert.That(hash.HexString, Is.EqualTo(expectedHash));
    }

    [Test]
    public void SHA512HashTest()
    {
        HashCrypto hashCrypto = new HashCrypto(HashCrypto.Provider.SHA512);
        string input = "Hello World!";
        ByteArray data = new ByteArray(input);
        ByteArray hash = hashCrypto.Calculate(data);

        string expectedHash = "861844D6704E8573FEC34D967E20BCFEF3D424CF48BE04E6DC08F2BD58C729743371015EAD891CC3CF1C9D34B49264B510751B1FF9E537937BC46B5D6FF4ECC8";

        Assert.That(hash.HexString, Is.EqualTo(expectedHash));
    }

    [Test]
    public void CRC32HashTest()
    {
        HashCrypto hashCrypto = new HashCrypto(HashCrypto.Provider.CRC32);
        string input = "Hello World!";
        ByteArray data = new ByteArray(input);
        ByteArray hash = hashCrypto.Calculate(data);

        string expectedHash = "1C291CA3";

        Assert.That(hash.HexString, Is.EqualTo(expectedHash));
    }

    [Test]
    public void HashWithSaltTest()
    {
        HashCrypto hashCrypto = new HashCrypto(HashCrypto.Provider.MD5);
        string input = "Hello World!";
        string salt = "MySalt";
        ByteArray data = new ByteArray(input);
        ByteArray saltData = new ByteArray(salt);
        ByteArray hash = hashCrypto.Calculate(data, saltData);

        string expectedHash = "B4655901284A69F20EF94730BE8A9D3D";

        Assert.That(hash.HexString, Is.EqualTo(expectedHash));
    }
}