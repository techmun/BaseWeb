using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using BaseWeb.ViewModels;

namespace BaseWeb.Cores
{

    public enum EnumSession
    {
        UserName,
        ImportPath,
        ExportPath
    }
    public class WebSession : ControllerBase
    {
        private static HttpContext _httpContext => new HttpContextAccessor().HttpContext;


        public static string GetSession(EnumSession sessionName)
        {
            string sessionVal = string.Empty;
            sessionVal = _httpContext.Session.GetString(sessionName.ToString());
            return sessionVal;
        }

        public static void storeSession(EnumSession sessionName, string val)
        {
            _httpContext.Session.SetString(sessionName.ToString(), val);
        }



    }
}
