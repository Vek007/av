using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AV
{
    public static class DBExecutor
    {
        private static readonly string connection_string = "";

        static DBExecutor()
        {
            connection_string = System.Configuration.ConfigurationManager.ConnectionStrings["MSSQLConnection"].ConnectionString;
        }

        public static int ExecuteCommand(string sqlCommand)
        {
            int numRows = 0;
            try
            {
                using (SqlConnection con = new SqlConnection(connection_string))
                {
                    //
                    // Open the SqlConnection.
                    //
                    con.Open();
                    //
                    // The following code uses an SqlCommand based on the SqlConnection.
                    //
                    using (SqlCommand command = new SqlCommand(sqlCommand, con))
                    {
                        Debug.WriteLine(">> " + sqlCommand);
                        numRows = command.ExecuteNonQuery();
                    }
                }

                return numRows;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error >> " + ex.ToString());
                return 0;
            }
        }

        public static List<string> GetAllSymFromDB()
        {
            string sql = "SELECT root_ticker FROM [st].[dbo].[t_x_v_main] union SELECT root_ticker FROM [st].[dbo].[t_v_main]";

            List<string> ts = new List<string>();

            try
            {
                using (SqlConnection myConnection = new SqlConnection(connection_string))
                {
                    myConnection.Open();
                    using (SqlCommand command = new SqlCommand(sql, myConnection))
                    using (SqlDataReader oReader = command.ExecuteReader())
                    {
                        while (oReader.Read())
                        {
                            string t = oReader["root_ticker"].ToString();
                            if (!string.IsNullOrEmpty(t))
                                ts.Add(t);
                        }
                    }
                    myConnection.Close();
                }

                return ts;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error >> " + ex.ToString());
                return null;
            }
        }
    }
}
