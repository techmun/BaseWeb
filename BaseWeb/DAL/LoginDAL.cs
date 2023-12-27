using BaseWeb.Cores;
using Microsoft.Data.SqlClient;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BaseWeb.DAL
{
    public class LoginDAL: DataAccessLayerBase
    {
        private readonly string constr;

        public LoginDAL(string _constr) : base(_constr)
        {

            constr = _constr;
        }

        public DataTable getUserPwById(string id)
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Login_GetAllUserById";
            cmd.Parameters.AddWithValue("id", id);
            return toDataTabe(cmd);
        }
    }
}
