using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using Huanlin.Collections;

namespace Example
{
    internal class DigitalSignatureDemo
    {
        Alice alice;
        Bob bob;

        public DigitalSignatureDemo()
        {
            alice = new Alice();
            bob = new Bob();
        }

        public ByteArray Sign(string plainText)
        {
            ByteArray data = new ByteArray(plainText);
            return new ByteArray(alice.HashAnsSign(data.Bytes));
        }

        public bool Verify(ByteArray data, ByteArray signature)
        {
            return bob.VeridySignedData(data.Bytes, signature.Bytes, alice.PublicKey);
        }
    }

    public class Alice
    {
        RSACryptoServiceProvider m_Rsa;

        public Alice()
        {
            m_Rsa = new RSACryptoServiceProvider();
        }

        /// <summary>
        /// 傳回數位簽章。
        /// </summary>
        /// <param name="dataToSign"></param>
        /// <returns></returns>
        public byte[] HashAnsSign(byte[] dataToSign)
        {
            // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
            // to specify the use of SHA1 for hashing.
            return m_Rsa.SignData(dataToSign, new SHA1CryptoServiceProvider());
        }

        public string PublicKey
        {
            get
            {
                return m_Rsa.ToXmlString(false);
            }
        }
    }

    public class Bob
    {
        public bool VeridySignedData(byte[] data, byte[] signature, string publicKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);
            return rsa.VerifyData(data, new SHA1CryptoServiceProvider(), signature);
        }
    }
}
