using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using Huanlin.Collections;

// A simple, string-oriented wrapper class for encryption functions, including 
// Hashing, Symmetric Encryption, and Asymmetric Encryption. 
//
// http://www.codeproject.com/dotnet/SimpleEncryption.asp
// 
// Jeff Atwood 
// http://www.codinghorror.com/ 

namespace Huanlin.Common.Cryptography
{
    /// <summary> 
    /// Symmetric encryption uses a single key to encrypt and decrypt. 
    /// Both parties (encryptor and decryptor) must share the same secret key. 
    /// </summary> 
    public class SymmetricCrypto
    {
        private const string DefaultIntializationVector = "1Az=-@qT";
        private const int BufferSize = 2048;

        public enum Provider
        {
            DES,        // The Data Encryption Standard provider supports a 64 bit key only 
            RC2,        // The Rivest Cipher 2 provider supports keys ranging from 40 to 128 bits, default is 128 bits 
            Rijndael,   // The Rijndael (also known as AES) provider supports keys of 128, 192, or 256 bits with a default of 256 bits 
            TripleDES   // The TripleDES provider (also known as 3DES) supports keys of 128 or 192 bits with a default of 192 bits 
        }

        private ByteArray m_Key;
        private ByteArray m_InitVector;
        private SymmetricAlgorithm m_SymmAlgorithm;

        public SymmetricAlgorithm GetSymmetricAlgorithm() => m_SymmAlgorithm;

        /// <summary> 
        /// Instantiates a new symmetric encryption object using the specified provider. 
        /// </summary> 
        public SymmetricCrypto(Provider provider, bool useDefaultInitializationVector)
        {
            switch (provider)
            {
                case Provider.DES:
                    m_SymmAlgorithm = DES.Create();
                    break;
                case Provider.RC2:
                    m_SymmAlgorithm = RC2.Create();
                    break;
                case Provider.Rijndael:
                    m_SymmAlgorithm = Aes.Create();
                    break;
                case Provider.TripleDES:
                    m_SymmAlgorithm = TripleDES.Create();
                    break;
            }

            //m_SymmAlgorithm.Mode = CipherMode.CBC;
            //m_SymmAlgorithm.Padding = PaddingMode.PKCS7;

            //-- make sure key and IV are always set, no matter what 
            Key = RandomKey();
            if (useDefaultInitializationVector)
            {
                this.IntializationVector = new ByteArray(DefaultIntializationVector);
            }
            else
            {
                this.IntializationVector = RandomInitializationVector();
            }
        }

        /// <summary> 
        /// The key used to encrypt/decrypt data 
        /// </summary> 
        public ByteArray Key
        {
            get { return m_Key; }
            set
            {
                m_Key = value;

                m_Key.MaxBytes = m_SymmAlgorithm.LegalKeySizes[0].MaxSize / 8;
                m_Key.MinBytes = m_SymmAlgorithm.LegalKeySizes[0].MinSize / 8;
                //m_Key.StepBytes = m_SymmAlgorithm.LegalKeySizes[0].SkipSize / 8;
            }
        }

        /// <summary> 
        /// Using the default Cipher Block Chaining (CBC) mode, all data blocks are processed using 
        /// the value derived from the previous block; the first data block has no previous data block 
        /// to use, so it needs an InitializationVector to feed the first block 
        /// </summary> 
        public ByteArray IntializationVector
        {
            get { return m_InitVector; }
            set
            {
                m_InitVector = value;
                m_InitVector.MaxBytes = m_SymmAlgorithm.BlockSize / 8;
                m_InitVector.MinBytes = m_SymmAlgorithm.BlockSize / 8;
            }
        }

        /// <summary> 
        /// generates a random Initialization Vector, if one was not provided 
        /// </summary> 
        public ByteArray RandomInitializationVector()
        {
            m_SymmAlgorithm.GenerateIV();
            ByteArray iv = new ByteArray(m_SymmAlgorithm.IV);
            return iv;
        }

        /// <summary> 
        /// generates a random Key, if one was not provided 
        /// </summary> 
        public ByteArray RandomKey()
        {
            m_SymmAlgorithm.GenerateKey();
            ByteArray key = new ByteArray(m_SymmAlgorithm.Key);
            return key;
        }

        /// <summary> 
        /// Ensures that _crypto object has valid Key and IV 
        /// prior to any attempt to encrypt/decrypt anything 
        /// </summary> 
        private void ValidateKeyAndIV(bool isEncrypting)
        {
            if (m_Key.IsEmpty)
            {
                if (isEncrypting)
                {
                    m_Key = RandomKey();
                }
                else
                {
                    throw new CryptographicException("No key was provided for the decryption operation!");
                }
            }
            if (m_InitVector.IsEmpty)
            {
                if (isEncrypting)
                {
                    m_InitVector = RandomInitializationVector();
                }
                else
                {
                    throw new CryptographicException("No initialization vector was provided for the decryption operation!");
                }
            }
            m_SymmAlgorithm.Key = m_Key.Bytes;
            m_SymmAlgorithm.IV = m_InitVector.Bytes;
        }

        /// <summary> 
        /// Encrypts the specified Data using provided key 
        /// </summary> 
        public byte[] Encrypt(byte[] data, byte[] key)
        {
            this.Key.Bytes = key;
            return Encrypt(data);
        }

        public ByteArray Encrypt(ByteArray data, ByteArray key)
        {
            this.Key = key;
            byte[] encryptedData = Encrypt(data.Bytes);
            return new ByteArray(encryptedData);
        }

        /// <summary> 
        /// Encrypts the specified Data using preset key and preset initialization vector 
        /// </summary> 
        public byte[] Encrypt(byte[] data)
        {
            ValidateKeyAndIV(true);

            var encryptor = m_SymmAlgorithm.CreateEncryptor(Key.Bytes, IntializationVector.Bytes);

            using MemoryStream ms = new MemoryStream();
            using CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.FlushFinalBlock();

            return ms.ToArray();
        }

        /// <summary> 
        /// Encrypts the stream to memory using provided key and provided initialization vector 
        /// </summary> 
        public byte[] Encrypt(Stream s, ByteArray key, ByteArray iv)
        {
            this.IntializationVector = iv;
            this.Key = key;
            return Encrypt(s);
        }

        /// <summary> 
        /// Encrypts the stream to memory using specified key 
        /// </summary> 
        public byte[] Encrypt(Stream s, byte[] key)
        {
            this.Key.Bytes = key;
            return Encrypt(s);
        }

        /// <summary> 
        /// Encrypts the specified stream to memory using preset key and preset initialization vector 
        /// </summary> 
        public byte[] Encrypt(Stream s)
        {
            MemoryStream ms = new MemoryStream();
            byte[] buf = new byte[BufferSize + 1];

            ValidateKeyAndIV(true);

            CryptoStream cs = new CryptoStream(ms, m_SymmAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);

            int i = s.Read(buf, 0, BufferSize);
            while (i > 0)
            {
                cs.Write(buf, 0, i);
                i = s.Read(buf, 0, BufferSize);
            }

            cs.Close();
            ms.Close();

            return ms.ToArray();
        }

        public ByteArray Decrypt(ByteArray encryptedData, ByteArray key)
        {
            this.Key = key;
            byte[] decryptedData = Decrypt(encryptedData.Bytes);
            return new ByteArray(decryptedData);
        }


        /// <summary> 
        /// Decrypts the specified data using provided key and preset initialization vector 
        /// </summary> 
        public byte[] Decrypt(byte[] encryptedData, byte[] key)
        {
            this.Key.Bytes = key;
            return Decrypt(encryptedData);
        }

        /// <summary> 
        /// Decrypts the specified stream using provided key and preset initialization vector 
        /// </summary> 
        public byte[] Decrypt(Stream encryptedStream, ByteArray key)
        {
            this.Key = key;
            return Decrypt(encryptedStream);
        }

        /// <summary> 
        /// Decrypts the specified stream using preset key and preset initialization vector 
        /// </summary> 
        public byte[] Decrypt(Stream encryptedStream)
        {
            MemoryStream ms = new MemoryStream();
            byte[] decryptedData = new byte[BufferSize + 1];

            ValidateKeyAndIV(false);
            CryptoStream cs = new CryptoStream(encryptedStream, m_SymmAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);

            int i;
            i = cs.Read(decryptedData, 0, BufferSize);
            while (i > 0)
            {
                ms.Write(decryptedData, 0, i);
                i = cs.Read(decryptedData, 0, BufferSize);
            }
            cs.Close();
            ms.Close();
          
            return ms.ToArray();
        }

        /// <summary> 
        /// Decrypts the specified data using preset key and preset initialization vector 
        /// </summary> 
        public byte[] Decrypt(byte[] encryptedData)
        {           
            ValidateKeyAndIV(false);

            ICryptoTransform decryptor = m_SymmAlgorithm.CreateDecryptor(Key.Bytes, IntializationVector.Bytes);
            using MemoryStream ms = new MemoryStream(encryptedData);
            using CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);

            using MemoryStream output = new MemoryStream();
            cs.CopyTo(output); // 將解密後的資料複製到 output stream
            return output.ToArray();
        }
    }
}
