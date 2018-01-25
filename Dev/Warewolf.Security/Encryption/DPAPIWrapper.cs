﻿using System;
using System.Security.Cryptography;
using System.Text;

namespace Warewolf.Security.Encryption
{
    public static class DpapiWrapper
    {
        const DataProtectionScope DataProtectionScope = System.Security.Cryptography.DataProtectionScope.LocalMachine;

        public static string DecryptIfEncrypted(string input)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input) || !input.IsBase64())
            {
                return input;
            }

            return Decrypt(input);
        }

        public static string EncryptIfDecrypted(string input)
        {
            if(string.IsNullOrEmpty(input) || string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            if (input.IsBase64() && input.CanBeDecrypted())
            {
                return input;
            }

            return Encrypt(input);
        }

        public static string Encrypt(string plainText)
        {
            if (plainText == null)
            {
                throw new ArgumentNullException(nameof(plainText));
            }

            //encrypt data
            var data = Encoding.Unicode.GetBytes(plainText);
            var encrypted = ProtectedData.Protect(data, null, DataProtectionScope);

            //return as base64 string
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipher)
        {
            if (cipher == null)
            {
                throw new ArgumentNullException(nameof(cipher));
            }

            if (!cipher.IsBase64())
            {
                throw new ArgumentException("cipher must be base64 encoded");
            }

            //parse base64 string
            var data = Convert.FromBase64String(cipher);

            //decrypt data
            var decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope);
            return Encoding.Unicode.GetString(decrypted);
        }

        public static bool CanBeDecrypted(this string cipher)
        {
            if(string.IsNullOrEmpty(cipher))
            {
                return false;
            }

            if (!cipher.IsBase64())
            {
                return false;
            }

            //parse base64 string
            var data = Convert.FromBase64String(cipher);

            //decrypt data
            try
            {
                ProtectedData.Unprotect(data, null, DataProtectionScope);
            }
            catch(Exception)
            {
                return false;
            }
            return true;
        }
        
        public static bool IsBase64(this string base64String)
        {
            if (base64String.Contains(" ") || base64String.Contains("\t") || base64String.Contains("\r") || base64String.Contains("\n"))
            {
                return false;
            }
            if (string.IsNullOrEmpty(base64String) || base64String.Length % 4 != 0)
            {
                return false;
            }

            try
            {                
                Convert.FromBase64String(base64String);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}