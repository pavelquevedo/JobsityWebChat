using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WebChat.Utils.Tools
{
    public class Encrypt
    {
        /// <summary>
        /// Static method to encrypt a string using SHA256
        /// </summary>
        /// <param name="text">String to encrypt</param>
        /// <returns></returns>
        public static string GetSHA256(string text)
        {
            SHA256 sha256 = SHA256Managed.Create();
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] stream = null;
            StringBuilder sb = new StringBuilder();
            stream = sha256.ComputeHash(encoding.GetBytes(text));
            for (int i = 0; i < stream.Length; i++) sb.AppendFormat("{0:x2}", stream[i]);
            return sb.ToString();
        }
    }
}
