using System.Security.Cryptography;
using System.Text;

namespace BaseWeb.Cores
{
    public class Encrypt
    {
        public static string Encrypted(string key)
        {
            string step1 = EncryptWithClass(key);
            string step2 = EncrptWithCharChoice(step1);
            return step2;
        }

        

        //step1
        public static string EncryptWithClass(string key)
        {
            byte[] b = System.Text.ASCIIEncoding.ASCII.GetBytes(key);
            string encrypted = Convert.ToBase64String(b);
            return encrypted;
        }

        //step2

        public static string EncrptWithCharChoice(string encryptString)
        {
            string EncryptionKey = "qwertyuiop";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
    }
}
