using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace P2PNetwork.Blockchain.Helpers
{
    public static class CryptographyHelper
    {
        public static byte[] HexToByte(string hex)
        {
            if (hex.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string!");
            }

            byte[] answ = new byte[hex.Length / 2];

            for (int i = 0; i < answ.Length; i++)
            {
                answ[i] = byte.Parse(hex.Substring(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return answ;
        }

        public static string ByteToHex(byte[] arr)
        {
            string hex = BitConverter.ToString(arr);

            return hex;
        }

        public static byte[] Sha256(byte[] arr)
        {
            SHA256Managed sha256 = new SHA256Managed();
            string hash = "";

            foreach (byte theByte in sha256.ComputeHash(arr))
            {
                hash += theByte.ToString("x2");
            }

            return Encoding.UTF8.GetBytes(hash);
        }
    }
}
