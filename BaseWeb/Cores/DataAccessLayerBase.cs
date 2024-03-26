
using Microsoft.Data.SqlClient;
using System.Data;
using System.Reflection;

namespace BaseWeb.Cores
{
    public class DataAccessLayerBase
    {
        private readonly string constr;
        public DataAccessLayerBase(string _constr) {
            constr = _constr;
        }
        
        public DataTable toDataTabe(SqlCommand cmd)
        {
            var dataTable = new DataTable();
            using (var connection = new SqlConnection(constr))
            {
                connection.Open();
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();
                dataTable.Load(reader);


            }
            return dataTable;

        }

        public DataSet toDataSet(SqlCommand cmd)
        {
            var ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter();
            using (var connection = new SqlConnection(constr))
            {
                connection.Open();
                cmd.Connection = connection;
                da = new SqlDataAdapter(cmd);
                da.Fill(ds);


            }
            return ds;

        }

    }
}
