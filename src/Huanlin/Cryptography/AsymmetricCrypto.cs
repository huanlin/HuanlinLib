using System;
using System.Collections.Generic;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using Huanlin.Collections;
using Huanlin.Helpers;

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
    /// Asymmetric encryption uses a pair of keys to encrypt and decrypt. 
    /// There is a "public" key which is used to encrypt. Decrypting, on the other hand, 
    /// requires both the "public" key and an additional "private" key. The advantage is 
    /// that people can send you encrypted messages without being able to decrypt them. 
    /// </summary> 
    /// <remarks> 
    /// The only provider supported is the <see cref="RSACryptoServiceProvider"/> 
    /// </remarks> 
    public class AsymmetricCrypto
    {

        private RSACryptoServiceProvider m_Rsa;
        private string m_KeyContainerName = "Encryption.AsymmetricEncryption.DefaultContainerName";
        //private bool _UseMachineKeystore = true;
        private int m_KeySize = 1024;

        private const string ElementParent = "RSAKeyValue";
        private const string ElementModulus = "Modulus";
        private const string ElementExponent = "Exponent";
        private const string ElementPrimeP = "P";
        private const string ElementPrimeQ = "Q";
        private const string ElementPrimeExponentP = "DP";
        private const string ElementPrimeExponentQ = "DQ";
        private const string ElementCoefficient = "InverseQ";
        private const string ElementPrivateExponent = "D";

        //-- http://forum.java.sun.com/thread.jsp?forum=9&thread=552022&tstart=0&trange=15 
        private const string KeyModulus = "PublicKey.Modulus";
        private const string KeyExponent = "PublicKey.Exponent";
        private const string KeyPrimeP = "PrivateKey.P";
        private const string KeyPrimeQ = "PrivateKey.Q";
        private const string KeyPrimeExponentP = "PrivateKey.DP";
        private const string KeyPrimeExponentQ = "PrivateKey.DQ";
        private const string KeyCoefficient = "PrivateKey.InverseQ";
        private const string KeyPrivateExponent = "PrivateKey.D";

        #region "PublicKey 類別"

        /// <summary> 
        /// Represents a public encryption key. Intended to be shared, it 
        /// contains only the Modulus and Exponent. 
        /// </summary> 
        public class PublicKey
        {
            public string Modulus;
            public string Exponent;

            public PublicKey()
            {
            }

            public PublicKey(string KeyXml)
            {
                LoadFromXml(KeyXml);
            }

            /// <summary> 
            /// Load public key from App.config or Web.config file 
            /// </summary> 
            public void LoadFromConfig()
            {
                this.Modulus = CryptoUtils.GetConfigString(KeyModulus);
                this.Exponent = CryptoUtils.GetConfigString(KeyExponent);
            }

            /// <summary> 
            /// Returns *.config file XML section representing this public key 
            /// </summary> 
            public string ToConfigSection()
            {
                StringBuilder sb = new StringBuilder();
                {
                    sb.Append(CryptoUtils.WriteConfigKey(KeyModulus, this.Modulus));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyExponent, this.Exponent));
                }
                return sb.ToString();
            }

            /// <summary> 
            /// Writes the *.config file representation of this public key to a file 
            /// </summary> 
            public void ExportToConfigFile(string filePath)
            {
                StreamWriter sw = new StreamWriter(filePath, false);
                sw.Write(this.ToConfigSection());
                sw.Close();
            }

            /// <summary> 
            /// Loads the public key from its XML string 
            /// </summary> 
            public void LoadFromXml(string keyXml)
            {
                this.Modulus = StrHelper.GetXmlElement(keyXml, "Modulus");
                this.Exponent = StrHelper.GetXmlElement(keyXml, "Exponent");
            }

            /// <summary> 
            /// Converts this public key to an RSAParameters object 
            /// </summary> 
            public RSAParameters ToParameters()
            {
                RSAParameters r = new RSAParameters();
                r.Modulus = Convert.FromBase64String(this.Modulus);
                r.Exponent = Convert.FromBase64String(this.Exponent);
                return r;
            }

            /// <summary> 
            /// Converts this public key to its XML string representation 
            /// </summary> 
            public string ToXml()
            {
                StringBuilder sb = new StringBuilder();
                {
                    sb.Append(CryptoUtils.WriteXmlNode(ElementParent));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementModulus, this.Modulus));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementExponent, this.Exponent));
                    sb.Append(CryptoUtils.WriteXmlNode(ElementParent, true));
                }
                return sb.ToString();
            }

            /// <summary> 
            /// Writes the Xml representation of this public key to a file 
            /// </summary> 
            public void ExportToXmlFile(string filePath)
            {
                StreamWriter sw = new StreamWriter(filePath, false);
                sw.Write(this.ToXml());
                sw.Close();
            }

        }
        #endregion

        #region "PrivateKey 類別"

        /// <summary> 
        /// Represents a private encryption key. Not intended to be shared, as it 
        /// contains all the elements that make up the key. 
        /// </summary> 
        public sealed class PrivateKey
        {
            public string Modulus;
            public string Exponent;
            public string PrimeP;
            public string PrimeQ;
            public string PrimeExponentP;
            public string PrimeExponentQ;
            public string Coefficient;
            public string PrivateExponent;

            public PrivateKey()
            {
            }

            public PrivateKey(string keyXml)
            {
                LoadFromXml(keyXml);
            }

            /// <summary> 
            /// Load private key from App.config or Web.config file 
            /// </summary> 
            public void LoadFromConfig()
            {
                this.Modulus = CryptoUtils.GetConfigString(KeyModulus);
                this.Exponent = CryptoUtils.GetConfigString(KeyExponent);
                this.PrimeP = CryptoUtils.GetConfigString(KeyPrimeP);
                this.PrimeQ = CryptoUtils.GetConfigString(KeyPrimeQ);
                this.PrimeExponentP = CryptoUtils.GetConfigString(KeyPrimeExponentP);
                this.PrimeExponentQ = CryptoUtils.GetConfigString(KeyPrimeExponentQ);
                this.Coefficient = CryptoUtils.GetConfigString(KeyCoefficient);
                this.PrivateExponent = CryptoUtils.GetConfigString(KeyPrivateExponent);
            }

            /// <summary> 
            /// Converts this private key to an RSAParameters object 
            /// </summary> 
            public RSAParameters ToParameters()
            {
                RSAParameters r = new RSAParameters();
                r.Modulus = Convert.FromBase64String(this.Modulus);
                r.Exponent = Convert.FromBase64String(this.Exponent);
                r.P = Convert.FromBase64String(this.PrimeP);
                r.Q = Convert.FromBase64String(this.PrimeQ);
                r.DP = Convert.FromBase64String(this.PrimeExponentP);
                r.DQ = Convert.FromBase64String(this.PrimeExponentQ);
                r.InverseQ = Convert.FromBase64String(this.Coefficient);
                r.D = Convert.FromBase64String(this.PrivateExponent);
                return r;
            }

            /// <summary> 
            /// Returns *.config file XML section representing this private key 
            /// </summary> 
            public string ToConfigSection()
            {
                StringBuilder sb = new StringBuilder();
                {
                    sb.Append(CryptoUtils.WriteConfigKey(KeyModulus, this.Modulus));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyExponent, this.Exponent));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyPrimeP, this.PrimeP));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyPrimeQ, this.PrimeQ));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyPrimeExponentP, this.PrimeExponentP));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyPrimeExponentQ, this.PrimeExponentQ));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyCoefficient, this.Coefficient));
                    sb.Append(CryptoUtils.WriteConfigKey(KeyPrivateExponent, this.PrivateExponent));
                }
                return sb.ToString();
            }

            /// <summary> 
            /// Writes the *.config file representation of this private key to a file 
            /// </summary> 
            public void ExportToConfigFile(string strFilePath)
            {
                StreamWriter sw = new StreamWriter(strFilePath, false);
                sw.Write(this.ToConfigSection());
                sw.Close();
            }

            /// <summary> 
            /// Loads the private key from its XML string 
            /// </summary> 
            public void LoadFromXml(string keyXml)
            {
                this.Modulus = StrHelper.GetXmlElement(keyXml, "Modulus");
                this.Exponent = StrHelper.GetXmlElement(keyXml, "Exponent");
                this.PrimeP = StrHelper.GetXmlElement(keyXml, "P");
                this.PrimeQ = StrHelper.GetXmlElement(keyXml, "Q");
                this.PrimeExponentP = StrHelper.GetXmlElement(keyXml, "DP");
                this.PrimeExponentQ = StrHelper.GetXmlElement(keyXml, "DQ");
                this.Coefficient = StrHelper.GetXmlElement(keyXml, "InverseQ");
                this.PrivateExponent = StrHelper.GetXmlElement(keyXml, "D");
            }

            /// <summary> 
            /// Converts this private key to its XML string representation 
            /// </summary> 
            public string ToXml()
            {
                StringBuilder sb = new StringBuilder();
                {
                    sb.Append(CryptoUtils.WriteXmlNode(ElementParent));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementModulus, this.Modulus));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementExponent, this.Exponent));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementPrimeP, this.PrimeP));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementPrimeQ, this.PrimeQ));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementPrimeExponentP, this.PrimeExponentP));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementPrimeExponentQ, this.PrimeExponentQ));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementCoefficient, this.Coefficient));
                    sb.Append(CryptoUtils.WriteXmlElement(ElementPrivateExponent, this.PrivateExponent));
                    sb.Append(CryptoUtils.WriteXmlNode(ElementParent, true));
                }
                return sb.ToString();
            }

            /// <summary> 
            /// Writes the Xml representation of this private key to a file 
            /// </summary> 
            public void ExportToXmlFile(string filePath)
            {
                StreamWriter sw = new StreamWriter(filePath, false);
                sw.Write(this.ToXml());
                sw.Close();
            }

        }

        #endregion

        /// <summary> 
        /// Instantiates a new asymmetric encryption session using the default key size; 
        /// this is usally 1024 bits 
        /// </summary> 
        public AsymmetricCrypto()
        {
            m_Rsa = GetRSAProvider();
        }

        /// <summary> 
        /// Instantiates a new asymmetric encryption session using a specific key size 
        /// </summary> 
        public AsymmetricCrypto(int keySize)
        {
            m_KeySize = keySize;
            m_Rsa = GetRSAProvider();
        }

        /// <summary> 
        /// Sets the name of the key container used to store this key on disk; this is an 
        /// unavoidable side effect of the underlying Microsoft CryptoAPI. 
        /// </summary> 
        /// <remarks> 
        /// http://support.microsoft.com/default.aspx?scid=http://support.microsoft.com:80/support/kb/articles/q322/3/71.asp&amp;NoWebContent=1 
        /// </remarks> 
        public string KeyContainerName
        {
            get { return m_KeyContainerName; }
            set { m_KeyContainerName = value; }
        }

        /// <summary> 
        /// Returns the current key size, in bits 
        /// </summary> 
        public int KeySizeBits
        {
            get { return m_Rsa.KeySize; }
        }

        /// <summary> 
        /// Returns the maximum supported key size, in bits 
        /// </summary> 
        public int KeySizeMaxBits
        {
            get { return m_Rsa.LegalKeySizes[0].MaxSize; }
        }

        /// <summary> 
        /// Returns the minimum supported key size, in bits 
        /// </summary> 
        public int KeySizeMinBits
        {
            get { return m_Rsa.LegalKeySizes[0].MinSize; }
        }

        /// <summary> 
        /// Returns valid key step sizes, in bits 
        /// </summary> 
        public int KeySizeStepBits
        {
            get { return m_Rsa.LegalKeySizes[0].SkipSize; }
        }

        /// <summary> 
        /// Returns the default public key as stored in the *.config file 
        /// </summary> 
        public PublicKey DefaultPublicKey
        {
            get
            {
                PublicKey pubkey = new PublicKey();
                pubkey.LoadFromConfig();
                return pubkey;
            }
        }

        /// <summary> 
        /// Returns the default private key as stored in the *.config file 
        /// </summary> 
        public PrivateKey DefaultPrivateKey
        {
            get
            {
                PrivateKey privkey = new PrivateKey();
                privkey.LoadFromConfig();
                return privkey;
            }
        }

        /// <summary> 
        /// Generates a new public/private key pair as objects 
        /// </summary> 
        public void GenerateNewKeyset(ref PublicKey publicKey, ref PrivateKey privateKey)
        {
            string PublicKeyXML = null;
            string PrivateKeyXML = null;
            GenerateNewKeyset(ref PublicKeyXML, ref PrivateKeyXML);
            publicKey = new PublicKey(PublicKeyXML);
            privateKey = new PrivateKey(PrivateKeyXML);
        }

        /// <summary> 
        /// Generates a new public/private key pair as XML strings 
        /// </summary> 
        public void GenerateNewKeyset(ref string publicKeyXML, ref string privateKeyXML)
        {
            RSA rsa = RSACryptoServiceProvider.Create();
            publicKeyXML = rsa.ToXmlString(false);
            privateKeyXML = rsa.ToXmlString(true);
        }

        /// <summary>
        /// Encrypts data using the default public key.
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public ByteArray Encrypt(ByteArray inputData)
        {
            PublicKey PublicKey = DefaultPublicKey;
            return Encrypt(inputData, PublicKey);
        }

        /// <summary> 
        /// Encrypts data using the provided public key.
        /// </summary> 
        public ByteArray Encrypt(ByteArray inputData, PublicKey publicKey)
        {
            m_Rsa.ImportParameters(publicKey.ToParameters());
            return InternalEncrypt(inputData);
        }

        /// <summary> 
        /// Encrypts data using the provided public key as XML 
        /// </summary> 
        public ByteArray Encrypt(ByteArray inputData, string publicKeyXML)
        {
            LoadKeyXml(publicKeyXML, false);
            return InternalEncrypt(inputData);
        }

        private ByteArray InternalEncrypt(ByteArray inputData)
        {
            try
            {
                return new ByteArray(m_Rsa.Encrypt(inputData.Bytes, false));
            }
            catch (CryptographicException ex)
            {
                if (ex.Message.ToLower().IndexOf("bad length") > -1)
                {
                    throw new CryptographicException(
                        "欲加密的資料量太大; RSA 加密方法主要是用來加密小量資料，" +
                        "其實際的資料量限制取決於 key 的長度。如欲加密大量資料，" +
                        "請改用對稱式加密，並且用非對稱式加密方法將對稱式加密的 key 加密。", ex);
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary> 
        /// Decrypts data using the default private key 
        /// </summary> 
        public ByteArray Decrypt(ByteArray encryptedData)
        {
            PrivateKey PrivateKey = new PrivateKey();
            PrivateKey.LoadFromConfig();
            return Decrypt(encryptedData, PrivateKey);
        }

        /// <summary> 
        /// Decrypts data using the provided private key 
        /// </summary> 
        public ByteArray Decrypt(ByteArray encryptedData, PrivateKey privateKey)
        {
            m_Rsa.ImportParameters(privateKey.ToParameters());
            return InternalDecrypt(encryptedData);
        }

        /// <summary> 
        /// Decrypts data using the provided private key as XML 
        /// </summary> 
        public ByteArray Decrypt(ByteArray encryptedData, string privateKeyXML)
        {
            LoadKeyXml(privateKeyXML, true);
            return InternalDecrypt(encryptedData);
        }

        private void LoadKeyXml(string keyXml, bool isPrivate)
        {
            try
            {
                m_Rsa.FromXmlString(keyXml);
            }
            catch (XmlSyntaxException ex)
            {
                string s;
                if (isPrivate)
                {
                    s = "private";
                }
                else
                {
                    s = "public";
                }
                throw new XmlSyntaxException(string.Format("The provided {0} encryption key XML does not appear to be valid.", s), ex);
            }
        }

        private ByteArray InternalDecrypt(ByteArray encryptedData)
        {
            return new ByteArray(m_Rsa.Decrypt(encryptedData.Bytes, false));
        }

        /// <summary> 
        /// gets the default RSA provider using the specified key size; 
        /// note that Microsoft's CryptoAPI has an underlying file system dependency that is unavoidable 
        /// </summary> 
        /// <remarks> 
        /// http://support.microsoft.com/default.aspx?scid=http://support.microsoft.com:80/support/kb/articles/q322/3/71.asp&amp;NoWebContent=1 
        /// </remarks> 
        private RSACryptoServiceProvider GetRSAProvider()
        {
            RSACryptoServiceProvider rsa = null;
            CspParameters csp = null;
            try
            {
                csp = new CspParameters();
                csp.KeyContainerName = m_KeyContainerName;
                rsa = new RSACryptoServiceProvider(m_KeySize, csp);
                return rsa;
            }
            catch (System.Security.Cryptography.CryptographicException ex)
            {
                if (ex.Message.ToLower().IndexOf("csp for this implementation could not be acquired") > -1)
                {
                    throw new Exception("Unable to obtain Cryptographic Service Provider. " +
                        "Either the permissions are incorrect on the " +
                        "'C:\\Documents and Settings\\All Users\\Application Data\\Microsoft\\Crypto\\RSA\\MachineKeys' " +
                        "folder, or the current security context '" +
                        System.Security.Principal.WindowsIdentity.GetCurrent().Name + "'" +
                        " does not have access to this folder.", ex);
                }
                else
                {
                    throw;
                }
            }
            finally
            {
                if ((rsa != null))
                {
                    rsa = null;
                }
                if ((csp != null))
                {
                    csp = null;
                }
            }
        }
    }
}
