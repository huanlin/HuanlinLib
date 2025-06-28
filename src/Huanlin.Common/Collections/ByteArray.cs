using System;
using System.Collections.Generic;
using System.Text;
using Huanlin.Common.Helpers;

namespace Huanlin.Collections
{
    /// <summary> 
    /// Represents Hex, Byte, Base64, or String data to encrypt/decrypt; 
    /// use the .Text property to set/get a string representation 
    /// use the .Hex property to set/get a string-based Hexadecimal representation 
    /// use the .Base64 to set/get a string-based Base64 representation 
    /// </summary> 
    public class ByteArray
    {
        private byte[] m_Data;
        private int m_MaxBytes = 0;
        private int m_MinBytes = 0;

        /// <summary> 
        /// Determines the default text encoding across ALL Data instances 
        /// </summary> 
        public static Encoding DefaultEncoding = Encoding.Default;

        /// <summary> 
        /// Determines the default text encoding for this Data instance 
        /// </summary> 
        public Encoding Encoding = DefaultEncoding;

        /// <summary> 
        /// Creates new, empty encryption data 
        /// </summary> 
        public ByteArray()
        {
        }

        /// <summary> 
        /// Creates new encryption data with the specified byte array 
        /// </summary> 
        public ByteArray(byte[] buf)
        {
            m_Data = buf;
        }

        /// <summary> 
        /// Creates new encryption data with the specified string; 
        /// will be converted to byte array using default encoding 
        /// </summary> 
        public ByteArray(string s)
        {
            this.Text = s;
        }

        /// <summary> 
        /// Creates new encryption data using the specified string and the 
        /// specified encoding to convert the string to a byte array. 
        /// </summary> 
        public ByteArray(string s, System.Text.Encoding encoding)
        {
            this.Encoding = encoding;
            this.Text = s;
        }

        /// <summary> 
        /// returns true if no data is present 
        /// </summary> 
        public bool IsEmpty
        {
            get
            {
                if (m_Data == null)
                {
                    return true;
                }
                if (m_Data.Length == 0)
                {
                    return true;
                }
                return false;
            }
        }

        /// <summary> 
        /// minimum number of bytes allowed for this data; if 0, no limit 
        /// </summary> 
        public int MinBytes
        {
            get { return m_MinBytes; }
            set { m_MinBytes = value; }
        }

        /// <summary> 
        /// minimum number of bits allowed for this data; if 0, no limit 
        /// </summary> 
        public int MinBits
        {
            get { return m_MinBytes * 8; }
            set { m_MinBytes = value / 8; }
        }

        /// <summary> 
        /// maximum number of bytes allowed for this data; if 0, no limit 
        /// </summary> 
        public int MaxBytes
        {
            get { return m_MaxBytes; }
            set { m_MaxBytes = value; }
        }

        /// <summary> 
        /// maximum number of bits allowed for this data; if 0, no limit 
        /// </summary> 
        public int MaxBits
        {
            get { return m_MaxBytes * 8; }
            set { m_MaxBytes = value / 8; }
        }

        /// <summary> 
        /// Returns the byte representation of the data; 
        /// This will be padded to MinBytes and trimmed to MaxBytes as necessary! 
        /// </summary> 
        public byte[] Bytes
        {
            get
            {
                if (m_MaxBytes == 0 && m_MinBytes == 0)
                {
                    return m_Data;
                }

                if (m_MaxBytes > 0)
                {
                    if (m_Data.Length > m_MaxBytes)
                    {
                        byte[] buf = new byte[m_MaxBytes];
                        Array.Copy(m_Data, buf, buf.Length);
                        m_Data = buf;
                    }
                }
                if (m_MinBytes > 0)
                {
                    if (m_Data.Length < m_MinBytes)
                    {
                        byte[] buf = new byte[m_MinBytes];
                        Array.Copy(m_Data, buf, m_Data.Length);
                        m_Data = buf;
                    }
                }
                return m_Data;
            }
            set 
            { 
                m_Data = value; 
            }
        }

        /// <summary> 
        /// Sets or returns text representation of bytes using the default text encoding 
        /// </summary> 
        public string Text
        {
            get
            {
                if (m_Data == null)
                {
                    return "";
                }
                else
                {
                    //-- need to handle nulls here; oddly, C# will happily convert 
                    //-- nulls into the string whereas VB stops converting at the 
                    //-- first null! 
                    int i = Array.IndexOf(m_Data, (byte)0);
                    if (i >= 0)
                    {
                        return this.Encoding.GetString(m_Data, 0, i);
                    }
                    else
                    {
                        return this.Encoding.GetString(m_Data);
                    }
                }
            }
            set 
            { 
                m_Data = this.Encoding.GetBytes(value); 
            }
        }

        /// <summary> 
        /// Sets or returns Hex string representation of this data 
        /// </summary> 
        public string HexString
        {
            get { return ConvertHelper.BytesToHexString(m_Data); }
            set { m_Data = ConvertHelper.HexStringToBytes(value); }
        }

        /// <summary> 
        /// Sets or returns Base64 string representation of this data 
        /// </summary> 
        public string Base64String
        {
            get { return ConvertHelper.BytesToBase64String(m_Data); }
            set { m_Data = ConvertHelper.Base64StringToBytes(value); }
        }

        /// <summary> 
        /// Returns text representation of bytes using the default text encoding 
        /// </summary> 
        public override string ToString()
        {
            return this.Text;
        }

        /// <summary> 
        /// returns Base64 string representation of this data 
        /// </summary> 
        public string ToBase64String()
        {
            return this.Base64String;
        }

        /// <summary> 
        /// returns Hex string representation of this data 
        /// </summary> 
        public string ToHexString()
        {
            return this.HexString;
        }

    }
}
