using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProgettoProva.web.Services
{
    public class GenreService
    {
        private const string connectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=testDatabase;";

        // HELPER
        public List<SelectListItem> GetGenreListItem()
        {
            List<SelectListItem> items = new List<SelectListItem>() { };
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT GenreId, [Name] FROM dbo.Genre";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            int genreIdPos = reader.GetOrdinal("GenreId");
                            int namePos = reader.GetOrdinal("Name");

                            do
                            {
                                SelectListItem item = new SelectListItem()
                                {
                                    Value = reader.GetInt16(genreIdPos).ToString(),
                                    Text = reader.GetString(namePos)

                                };

                                items.Add(item);
                            } while (reader.Read());
                        }
                    }

                }
            }
            return items;
        }
    }
}