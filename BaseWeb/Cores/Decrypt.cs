using System.Security.Cryptography;
using System.Text;

namespace BaseWeb.Cores
{
    public class Decrypt
    {
        public static string Decrypted(string key)
        {
            string step1 = DecrptWithCharChoice(key);
            string step2 = DecryptWithClass(step1);
            return step2;
        }

        public static string DecryptWithClass(string key)
        {

            byte[] b;
            string decrypted;
            try
            {
                b = Convert.FromBase64String(key);
                decrypted = System.Text.ASCIIEncoding.ASCII.GetString(b);
            }
            catch (FormatException fe)
            {
                decrypted = "";
            }
            return decrypted;
        }

        public static string DecrptWithCharChoice(string cipherText)
        {
            string EncryptionKey = "qwertyuiop";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}
