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

namespace Huanlin.Cryptography
{
    /// <summary> 
    /// Symmetric encryption uses a single key to encrypt and decrypt. 
    /// Both parties (encryptor and decryptor) must share the same secret key. 
    /// </summary> 
    public class SymmetricCrypto
    {
        private const string DefaultIntializationVector = "%1Az=-@qT";
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


        /// <summary> 
        /// Instantiates a new symmetric encryption object using the specified provider. 
        /// </summary> 
        public SymmetricCrypto(Provider provider, bool useDefaultInitializationVector)
        {
            switch (provider)
            {
                case Provider.DES:
                    m_SymmAlgorithm = new DESCryptoServiceProvider();
                    break;
                case Provider.RC2:
                    m_SymmAlgorithm = new RC2CryptoServiceProvider();
                    break;
                case Provider.Rijndael:
                    m_SymmAlgorithm = new RijndaelManaged();
                    break;
                case Provider.TripleDES:
                    m_SymmAlgorithm = new TripleDESCryptoServiceProvider();
                    break;
            }

            //-- make sure key and IV are always set, no matter what 
            this.Key = RandomKey();
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
        /// Key size in bytes. We use the default key size for any given provider; if you 
        /// want to force a specific key size, set this property 
        /// </summary> 
        public int KeySizeBytes
        {
            get { return m_SymmAlgorithm.KeySize / 8; }
            set
            {
                m_SymmAlgorithm.KeySize = value * 8;
                m_Key.MaxBytes = value;
            }
        }

        /// <summary> 
        /// Key size in bits. We use the default key size for any given provider; if you 
        /// want to force a specific key size, set this property 
        /// </summary> 
        public int KeySizeBits
        {
            get { return m_SymmAlgorithm.KeySize; }
            set
            {
                m_SymmAlgorithm.KeySize = value;
                m_Key.MaxBits = value;
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
            MemoryStream ms = new MemoryStream();

            ValidateKeyAndIV(true);

            CryptoStream cs = new CryptoStream(ms, m_SymmAlgorithm.CreateEncryptor(), CryptoStreamMode.Write);
            cs.Write(data, 0, data.Length);
            cs.Close();
            ms.Close();

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
            MemoryStream ms = new MemoryStream(encryptedData, 0, encryptedData.Length);
            byte[] buf = new byte[encryptedData.Length];

            ValidateKeyAndIV(false);
            CryptoStream cs = new CryptoStream(ms, m_SymmAlgorithm.CreateDecryptor(), CryptoStreamMode.Read);

            try
            {
                int cnt = cs.Read(buf, 0, encryptedData.Length - 1);
                byte[] result = new byte[cnt];
                Array.Copy(buf, result, cnt);

                return result;
            }
            catch (CryptographicException ex)
            {
                throw new CryptographicException("資料解密失敗! 可能是因為提供了不正確的金鑰。", ex);
            }
            finally
            {
                cs.Close();
            }
        }


        /// <summary>
        /// 傳回指定之對稱式加密演算法的合法金鑰。
        /// </summary>
        /// <param name="symmAlgo">對稱式加密演算法物件。</param>
        /// <param name="key">愈指定的金鑰（字串）。</param>
        /// <returns>合法的金鑰（byte 陣列）。</returns>
        public static byte[] GetLegalKey(SymmetricAlgorithm symmAlgo, string key)
        {
            byte[] keyData = Encoding.Default.GetBytes(key);

            if (symmAlgo.LegalKeySizes.Length > 0)
            {
                int maxBytes = symmAlgo.LegalKeySizes[0].MaxSize / 8;
                int minBytes = symmAlgo.LegalKeySizes[0].MinSize / 8;

                if (keyData.Length > maxBytes)
                {
                    byte[] buf = new byte[maxBytes];
                    Array.Copy(keyData, buf, buf.Length);
                    return buf;
                }
                else if (keyData.Length < minBytes)
                {
                    byte[] buf = new byte[minBytes];
                    Array.Copy(keyData, buf, keyData.Length);
                    return buf;
                }
                else
                {
                    // key 的長度介於 minBytes 和 maxBytes 之間。
                    int length = keyData.Length + 8 - (keyData.Length % 8);
                    byte[] buf = new byte[length];
                    Array.Copy(keyData, buf, keyData.Length);
                    return buf;
                }
            }
            return keyData;
        }

    }
}
