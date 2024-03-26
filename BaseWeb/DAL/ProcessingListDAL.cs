using BaseWeb.Cores;
using Microsoft.Data.SqlClient;
using System.Data;

namespace BaseWeb.DAL
{
    public class ProcessingListDAL : DataAccessLayerBase
    {
        private readonly string constr;

        public ProcessingListDAL(string _constr) : base(_constr)
        {

            constr = _constr;
        }

        public DataSet getProcessingList(string CustCode)
        {
            var cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "Job_GetProcessingList";
            cmd.Parameters.AddWithValue("CustCode", CustCode);
            return toDataSet(cmd);
        }
    }
}
