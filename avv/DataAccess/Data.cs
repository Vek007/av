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
using System.Diagnostics;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

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

        internal static void AddPhAsDup(ph p)
        {
            if (alDb.phs.Where(p1 => p1.path.Trim() == p.path.Trim()).Count() <= 0)
            {
                p.is_dup = true;
                Data.alDb.phs.Add(p);
                Data.alDb.SaveChanges();
                RefreshDatabase(p);
            }
        }

        public static bool ExistsAsRecord(ph p)
        {
            Debug.WriteLine(p.id + p.time_stamp.ToString());
            List<ph> pDbs = alDb.phs.Where(p1 => p1.id.Trim() == p.id.Trim()).ToList();

            foreach (ph pd in pDbs)
            {
                {
                    if (pd.time_stamp.Value.Date.Day == p.time_stamp.Value.Date.Day &&
                        pd.time_stamp.Value.Date.Month == p.time_stamp.Value.Date.Month &&
                        pd.time_stamp.Value.Date.Year == p.time_stamp.Value.Date.Year &&
                        pd.time_stamp.Value.Date.Hour == p.time_stamp.Value.Date.Hour &&
                        pd.time_stamp.Value.Date.Minute == p.time_stamp.Value.Date.Minute)
                        return true;
                }
            }
            return false;
        }

        public static List<ph> GetPhByMonthsAndYear(int year, int month)
        {
            List<ph> phList = alDb.phs.Where(p => p.time_stamp.Value.Year == year && p.time_stamp.Value.Month == month).OrderBy(p => p.time_stamp.Value).ToList();

            return phList;
        }

        public static List<ph> GetPhByStartAndEndDate(DateTime stDate, DateTime endDate)
        {
            List<ph> phList = alDb.phs.Where(p => p.time_stamp.Value >= stDate && p.time_stamp.Value <= endDate).OrderBy(p => p.time_stamp.Value).ToList();

            return phList;
        }

        public static List<ph> GetPhByDate(DateTime date)
        {
            List<ph> phList = alDb.phs.Where(p => p.time_stamp.Value.Year == date.Year && p.time_stamp.Value.Month == date.Month && p.time_stamp.Value.Day== date.Day).OrderBy(p => p.time_stamp.Value).ToList();

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
        /// <param name="p">Ph to add</param>
        public static void AddPh(ph p)
        {
            if (!Data.ExistsAsRecord(p))
            {
                Data.alDb.phs.Add(p);
                Data.alDb.SaveChanges();
                RefreshDatabase(p);
            }
            else
            {
                Debug.WriteLine("Duplicate:" + p.id + " - " + p.path);
            }

            //string sqlInsert = "Insert into ph (id, name, description,path,time_stamp) values ('"+ p.Id + "','" +p.Name + "','" + p.Description + "','" + p.FilePath + "','" + p.CreationDate+"')";

            //DBExecutor.ExecuteCommand(sqlInsert);
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
