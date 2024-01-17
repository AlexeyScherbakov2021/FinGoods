using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace FinGoods.Repository
{
    internal class RepositoryFP
    {
        private SqlConnection conn;

        public RepositoryFP()
        {
            //System.Configuration.ConfigurationSettings;

            ConnectionStringSettings settings =
                   ConfigurationManager.ConnectionStrings["ConnectFP"];

            SecureString theSecureString = new NetworkCredential("", "ctcnhjt,s").SecurePassword;
            theSecureString.MakeReadOnly();

            SqlCredential credential = new SqlCredential("fpLoginName", theSecureString);

            conn = new SqlConnection(settings.ConnectionString, credential);
        }

        public void Load()
        {
            try
            {
                conn.Open();
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = "SELECT cl.*,e.*,c.*,cli.* FROM [Нефтегазкомплекс].[dbo].[ContractList] cl " +
                    "join Elements e on e.ElementID=cl.ELID " +
                    "join Contracts c on cl.ContractID=c.ContractID " +
                    "join Clients cli on cli.cli_code=c.cli_code " +
                    "where lvl=1 and e.ElementName not like '%допоставк%'";
                SqlDataReader read = cmd.ExecuteReader();

                while (read.Read())
                {
                    string name = read.GetString(13);
                }
            }

            catch 
            {
            }

            finally 
            { 
                conn.Close(); 
            }
        }

    }
}
