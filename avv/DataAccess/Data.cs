using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Core.Objects;
using System.Globalization;

namespace AV
{
    public class Data
    {
        public static alvEntities alDb = new alvEntities();

        /// <summary>
        /// Get all albums and associated Ph values
        /// </summary>
        /// <returns>Collection of Albums</returns>
        public static ReadOnlyCollection<Al> GetPhAl()
        {
            List<Al> albums = new List<Al>();
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("GetAlbums", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();

                    // Use using here so SqlDataReader will be closed automatically
                    using(SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            albums.Add(new Al()
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Description = reader.GetString(2)
                            }
                            );
                        }
                    }
                }

                // Now get all the Phs for each each album
                // This could be obtained by a single query with multiple
                // result sets but for illustrative purposes it is broken
                // into two processes
                using(SqlCommand cmd = new SqlCommand("GetPhsByAlbum", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@albumId", SqlDbType.Int);
                    for(int x = 0; x < albums.Count; x++)
                    {
                        cmd.Parameters["@albumId"].Value = albums[x].Id;

                        List<Ph> Phs = new List<Ph>();
                        // Use using here so SqlDataReader will be closed automatically
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while(reader.Read())
                            {
                                Phs.Add(new Ph()
                                    {
                                        Id = reader.GetString(0),
                                        Name = reader.GetString(1),
                                        Description = reader.GetString(2),
                                        Image = (byte[])reader.GetValue(3)
                                    }
                                );
                            }
                        }

                        // Annoying because
                        // albums[x].Phs = Phs.AsReadOnly();
                        // produces the error, Cannot modify the return value of xxx because it is not a variable
                        // The error could be avoided by using class rather than struct
                        Al temp = albums[x];
                        temp.Phs = Phs.AsReadOnly();
                        albums[x] = temp;
                    }
                }
            }

            return albums.AsReadOnly();
        }

        public static List<ph> GetPhByMonthsAndYear(int year, int month)
        {
            List<ph> phList = alDb.phs.Where(p => p.time_stamp.Value.Year == year && p.time_stamp.Value.Month == month).OrderBy(p => p.time_stamp.Value).ToList();

            return phList;
        }


        public static List<string> GetDistinctPhYears()
        {
            List<string> years = alDb.phs.
                Select(p => p.time_stamp.Value.Year.ToString()).Distinct().OrderByDescending(a=>a).ToList();


            return years;
        }

        public static List<string> GetDistinctPhMonths(int year)
        {
            List<string> monthInNames = alDb.phs.OrderBy(p => p.time_stamp.Value).Where(p=>p.time_stamp.Value.Year==year).
                Select(p => p.time_stamp.Value.Month.ToString()).Distinct().ToList().
                Select(a => CultureInfo.CurrentCulture.DateTimeFormat.
                GetMonthName(Convert.ToInt32(a))).ToList();

            return monthInNames;
        }

        /// <summary>
        /// Add new al to database
        /// </summary>
        /// <returns>Newly created Album</returns>
        public static Al AddAl()
        {
            Al album = new Al()
            {
                Name = "New Album",
                Description = "Enter Description"
            };

            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("InsertAlbum", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();

                    // Add the return value parameter
                    SqlParameter param = cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                    param.Direction = ParameterDirection.ReturnValue;

                    // Add the name parameter and set the value
                    cmd.Parameters.AddWithValue("@name", album.Name);
                    // Add the description parameter and set the value
                    cmd.Parameters.AddWithValue("@desc", album.Description);

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // The return value is the index of the newly added album
                    album.Id = (int)cmd.Parameters["RETURN_VALUE"].Value;
                }
            }

            return album;
        }

        /// <summary>
        /// Add Ph to Album
        /// </summary>
        /// <param name="alId">Id of Album to add Ph to</param>
        /// <param name="Ph">Ph to add</param>
        public static void AddPh(int alId, Ph Ph)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("InsertPh", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    conn.Open();

                     // Add the name parameter and set the value
                    cmd.Parameters.AddWithValue("@name", Ph.Name);
                    // Add the description parameter and set the value
                    cmd.Parameters.AddWithValue("@desc", Ph.Description);
                    // Add the image parameter and set the value
                    cmd.Parameters.AddWithValue("@Ph", Ph.Image);
                    // Add the album parameter and set the value
                    cmd.Parameters.AddWithValue("@albumId", alId);

                    // Add the return value parameter
                    SqlParameter param = cmd.Parameters.Add("RETURN_VALUE", SqlDbType.Int);
                    param.Direction = ParameterDirection.ReturnValue;

                    // Execute the insert
                    cmd.ExecuteNonQuery();

                    // Return value will be the index of the newly added Ph
                    Ph.Id = cmd.Parameters["RETURN_VALUE"].Value.ToString();
                }
            }
        }

        /// <summary>
        /// Add Ph to Al
        /// </summary>
        /// <param name="albumId">Id of Album to add Ph to</param>
        /// <param name="Ph">Ph to add</param>
        public static void AddPh(Ph Ph)
        {
            string sqlInsert = "Insert into ph (id, name, description,path,time_stamp) values ('"+ Ph.Id + "','" +Ph.Name + "','" + Ph.Description + "','" + Ph.FilePath + "','" + Ph.CreationDate+"')";

            DBExecutor.ExecuteCommand(sqlInsert);
        }

        public static void AddAl(string id)
        {
            string sqlInsert = "Insert into al (id, name) values ('" + id + "','" + id + "')";

            DBExecutor.ExecuteCommand(sqlInsert);
        }

        public static void AddAP(string alId, string phId)
        {
            string sqlInsert = "Insert into ph_al (ph_id, al_id) values ('" + phId + "','" + alId + "')";

            DBExecutor.ExecuteCommand(sqlInsert);
        }

        public static List<al> GetAls()
        {
            return alDb.als.ToList();
        }

        public static List<ph> GetPhs()
        {
            return alDb.phs.ToList();
        }

        /// <summary>
        /// Get Ph matching Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Ph GetPh(string id)
        {
            Ph Ph = new Ph()
            {
                Id = id
            };

            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("GetPh", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    DataTable dt = new DataTable();
                    dt.Load(cmd.ExecuteReader());

                    if(dt.Rows.Count != 0)
                    {
                        Ph.Name = dt.Rows[0]["name"].ToString();
                        Ph.Description = dt.Rows[0]["description"].ToString();
                        Ph.Image = (byte[])dt.Rows[0]["Ph"];
                    }

                    // Could also use a SqlDataReader
                    //SqlDataReader reader = cmd.ExecuteReader();
                    //while(reader.Read())
                    //{
                    //    Ph.Name = reader.GetString(0);
                    //    Ph.Description = reader.GetString(1);
                    //    //Ph.Image = (byte[])reader.GetValue(2);
                    //}
                }
            }

            return Ph;
        }

        /// <summary>
        /// Delete Al and all Phs associated with it
        /// </summary>
        /// <param name="id">Id of Album to delete</param>
        public static void DeleteAl(int id)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("DeleteAlbum", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Delete Ph
        /// </summary>
        /// <param name="id">Id of Ph to delete</param>
        public static void DeletePh(string id)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("DeletePh", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update Al
        /// </summary>
        /// <param name="album">Album info to update</param>
        public static void UpdateAl(Al album)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("UpdateAlbum", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", album.Id);
                    cmd.Parameters.AddWithValue("@name", album.Name);
                    cmd.Parameters.AddWithValue("@desc", album.Description);

                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Update Ph
        /// </summary>
        /// <param name="Ph">Ph info to update</param>
        public static void UpdatePh(Ph Ph)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using(SqlCommand cmd = new SqlCommand("UpdatePh", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id", Ph.Id);
                    cmd.Parameters.AddWithValue("@name", Ph.Name);
                    cmd.Parameters.AddWithValue("@desc", Ph.Description);
                    
                    conn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
        }


        public static void RefreshDatabase(Object entity)
        {
            ((IObjectContextAdapter)Data.alDb)
                .ObjectContext
                .Refresh(RefreshMode.StoreWins, entity);
        }

        #region Properties

        public static string ConnectionString
        {
            get { return ConfigurationManager.ConnectionStrings["Data"].ConnectionString; }
        }

        #endregion
    }
}
