using System;
using System.Configuration;

// A simple, string-oriented wrapper class for encryption functions, including 
// Hashing, Symmetric Encryption, and Asymmetric Encryption. 
// 
// Jeff Atwood 
// http://www.codinghorror.com/ 

namespace Huanlin.Cryptography
{
    /// <summary> 
    /// Friend class for shared utility methods used by multiple Encryption classes 
    /// </summary> 
    internal sealed class CryptoUtils
    {

        /// <summary> 
        /// Returns the specified string value from the application .config file 
        /// </summary> 
        internal static string GetConfigString(string key, bool isRequired)
        {

            string s = (string)ConfigurationManager.AppSettings.Get(key);
            if (s == null)
            {
                if (isRequired)
                {
                    throw new ConfigurationErrorsException("組態檔（.config）中沒有 <" + key + "> 標籤。");
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return s;
            }
        }

        internal static string GetConfigString(string key)
        {
            return GetConfigString(key, true);
        }

        internal static string WriteConfigKey(string key, string value)
        {
            string s = "<add key=\"{0}\" value=\"{1}\" />" + Environment.NewLine;
            return string.Format(s, key, value);
        }

        internal static string WriteXmlElement(string element, string value)
        {
            string s = "<{0}>{1}</{0}>" + Environment.NewLine;
            return string.Format(s, element, value);
        }

        internal static string WriteXmlNode(string element, bool isClosing)
        {
            string s;
            if (isClosing)
            {
                s = "</{0}>" + Environment.NewLine;
            }
            else
            {
                s = "<{0}>" + Environment.NewLine;
            }
            return string.Format(s, element);
        }

        internal static string WriteXmlNode(string element)
        {
            return WriteXmlNode(element, false);
        }

    }
}
