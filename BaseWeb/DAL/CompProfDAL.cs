using BaseWeb.Cores;
using BaseWeb.ViewModels;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web;
using Microsoft.AspNetCore.Mvc;

namespace BaseWeb.DAL
{
    public class CustProfDAL : DataAccessLayerBase
    {
        private readonly string constr;
        public CustProfDAL(string _constr) : base(_constr)
        {

            constr = _constr;
        }

        public DataTable getCustProfByFilter(string filter = "")
        {
            var cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "CustProf_ReadCustProfSearch";
            cmd.Parameters.AddWithValue("qfilter", filter);
            return toDataTabe(cmd);
        }

        public DataTable addCustProf(AddEditCustProfViewModel model, string queryType,string userId)
        {
            var cmd = new MySqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = queryType == "I"?"CustProf_Craete":"CustProf_Update";
            cmd.Parameters.AddWithValue("CustCode", model.CustCode);
            cmd.Parameters.AddWithValue("CompName", model.CustName);
            cmd.Parameters.AddWithValue("ContactPS", model.ContactPS);
            cmd.Parameters.AddWithValue("ContactNo", model.ContactNo);
            cmd.Parameters.AddWithValue("Email", model.Email);
            cmd.Parameters.AddWithValue("LicenseNo", model.LicenseNo);
            cmd.Parameters.AddWithValue("UserId", userId);
            return toDataTabe(cmd);
        }
    }
}
