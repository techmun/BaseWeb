using MySql.Data.MySqlClient;
using System.Data;

namespace BaseWeb.Cores
{
    public class DataAccessLayerBase
    {
        private readonly string constr;
        public DataAccessLayerBase(string _constr) {
            constr = _constr;
        }

        public DataTable toDataTabe(MySqlCommand cmd)
        {
            var dataTable = new DataTable();
            using (var connection = new MySqlConnection(constr))
            {
                connection.Open();
                cmd.Connection = connection;
                var reader = cmd.ExecuteReader();
                dataTable.Load(reader);


            }
            return dataTable;

        }

    }
}
