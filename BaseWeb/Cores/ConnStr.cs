using Microsoft.Extensions.Configuration;

namespace BaseWeb.Cores
{
    public class ConnStr
    {
        public static string connection()
        {
            var MyConfig = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
            var constr = MyConfig.GetConnectionString("Default");


            return Decrypt.Decrypted(constr);
        }
    }
}
