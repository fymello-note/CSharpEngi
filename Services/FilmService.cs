using ProgettoProva.web.Models.Film;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProgettoProva.web.Services
{
    public class FilmService
    {
        // HELPER
        private void AddFilm(FilmViewModel film)
        {

            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"INSERT INTO film (Title, ReleaseDate, Director, Producer, GenreId) 
                                            OUTPUT inserted.FilmId
                                            VALUES (@Title, @ReleaseDate, @Director, @Producer, @GenreId);";

                    // command.Parameters.AddWithValue("Title", film.Title);
                    // command.Parameters.AddWithValue("ReleaseDate", film.ReleaseDate);
                    // command.Parameters.AddWithValue("Director", film.Director);
                    //command.Parameters.AddWithValue("Producer", film.Producer == null ? DBNull.Value : (object)film.Producer);
                    // command.Parameters.AddWithValueOrDBNull("Producer", film.Producer);

                    /*
                    command.Parameters.BindParameters(new Dictionary<string, object>()
                    {
                        //{ "Director", film.Director }, <- altro modo per scrivere
                        ["Title"] = film.Title,
                        ["ReleaseDate"] = film.ReleaseDate,
                        ["Director"] = film.Director,
                        ["Producer"] = film.Producer
                    });
                    */

                    var record = new FilmRecord()
                    {
                        Title = film.Title,
                        Director = film.Director,
                        Producer = film.Producer,
                        ReleaseDate = film.ReleaseDate,
                    };

                    command.Parameters.BindParameters(film, new string[] { "Cast" });

                    int filmId = (int)command.ExecuteScalar();

                    for(int i = 0; i < film.Cast.Count; i++) {
                        command.CommandText = @"
                                                INSERT INTO dbo.Actor (FilmId, [Name])
                                                VALUES (@FilmId, @Name);";

                        command.Parameters.Clear();

                        command.Parameters.AddWithValue("FilmId", filmId);
                        command.Parameters.AddWithValue("Name", film.Cast[i]);

                        command.ExecuteNonQuery();
                    }

                    //command.ExecuteNonQuery();
                }
            }
        }

        // HELPER
        private List<FilmDataGridItemViewModel> GetFilms()
        {
            Dictionary<int, FilmDataGridItemViewModel> filmsDic = new Dictionary<int, FilmDataGridItemViewModel>();
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                
                // Recupera informazioni per tutti i film
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = @"SELECT film.FilmId, film.Title, film.ReleaseDate, film.Director, film.Producer, genre.[Name] as Genre 
                                            From dbo.Film as film inner join dbo.Genre as genre 
                                            on Film.GenreId = Genre.GenreId;";

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int filmIdPos = reader.GetOrdinal("FilmId");
                            int titlePos = reader.GetOrdinal("Title");
                            int releaseDatePos = reader.GetOrdinal("ReleaseDate");
                            int directorPos = reader.GetOrdinal("Director");
                            int producerPos = reader.GetOrdinal("Producer");
                            int genrePos = reader.GetOrdinal("Genre");
                            
                            do
                            {
                                    var film = new FilmDataGridItemViewModel
                                    {
                                        FilmId = reader.GetInt32(filmIdPos),
                                        Title = reader.GetString(titlePos),
                                        ReleaseDate = reader.GetDateTime(releaseDatePos),
                                        Director = reader.GetString(directorPos),
                                        Producer = !reader.IsDBNull(producerPos) ? reader.GetString(producerPos) : null,
                                        Genre = reader.GetString(genrePos),
                                        /*Cast = new List<string>() { "mike" }*/
                                    };
                                filmsDic.Add(film.FilmId, film);
                            } while (reader.Read());
                        }
                    }
                }

                // Recupera informazioni di tutti gli attori per i film selezionati
                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT FilmId, [Name] FROM dbo.Actor;";
                    command.CommandType = CommandType.Text;

                    using(SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int filmIdPos = reader.GetOrdinal("FilmId");
                            int namePos = reader.GetOrdinal("Name");

                            do
                            {
                                int filmId = reader.GetInt32(filmIdPos);
                                string name = reader.GetString(namePos);

                                if (filmsDic.TryGetValue(filmId, out FilmDataGridItemViewModel film)) {
                                    film.Cast.Add(name);
                                }

                                //FilmDataGridItemViewModel film = films.FirstOrDefault(f => f.FilmId == filmId);
                                //film.Cast.Add(name);

                                /*foreach (FilmDataGridItemViewModel film in films)
                                {
                                    if(film.FilmId == filmId)
                                    {
                                        film.Cast.Add(name);
                                        break;
                                    }
                                }*/

                            } while (reader.Read());

                            /*List<String> names = new List<string>();
                            int LastFilmId = reader.GetInt32(filmIdPos);
                            do
                            {
                                int filmId = reader.GetInt32(filmIdPos);

                                if (LastFilmId != filmId)
                                {
                                    if (filmsDic.TryGetValue(filmId, out FilmDataGridItemViewModel film)) {
                                        film.Cast = names;
                                    }
                                    //FilmDataGridItemViewModel film = films.FirstOrDefault(f => f.FilmId == filmId);
                                    //film?.Cast = names;
                                    names.Clear();
                                    LastFilmId = filmId;
                                }

                                 names.Add(reader.GetString(namePos));

                            } while (reader.Read());*/
                        }
                    }
                }
            }

            return filmsDic.Values.ToList();
        }

        // HELPER
        public void UpdateFilm(int id, FilmViewModel film)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlTransaction transaction = connection.BeginTransaction())
                {

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"UPDATE Film 
                                    SET Title = @Title, ReleaseDate = @ReleaseDate, Director = @Director, Producer = @Producer, GenreId = @GenreId 
                                            WHERE FilmId = @FilmId;";

                        command.Transaction = transaction;

                        command.Parameters.BindParameters(film, new string[] { nameof(film.Cast) });
                        command.Parameters.AddWithValue("FilmId", id);

                        int affectedRows = command.ExecuteNonQuery();
                        if (affectedRows == 0)
                            throw new ResourceNotFoundException("Film non trovato.");
                    }

                    using (SqlCommand command = connection.CreateCommand())
                    {
                        command.CommandText = @"Delete FROM Actor 
                                            WHERE FilmId = @FilmId;";
                        command.Transaction = transaction;

                        command.Parameters.AddWithValue("FilmId", id);
                        command.ExecuteNonQuery();
                    }

                    if (film.Cast.Count > 0)
                    {

                        using (SqlCommand command = connection.CreateCommand())
                        {

                            command.CommandText = @"
                                                INSERT INTO dbo.Actor (FilmId, [Name])
                                                VALUES (@FilmId, @Name);";
                            
                            command.Transaction = transaction;

                            for (int i = 0; i < film.Cast.Count; i++)
                            {

                                command.Parameters.Clear();

                                command.Parameters.AddWithValue("FilmId", id);
                                command.Parameters.AddWithValue("Name", film.Cast[i]);

                                command.ExecuteNonQuery();
                            }
                        }
                    }

                    transaction.Commit(); // Da mettere sempre quando accettiamo le modifiche
                    // transaction.Rollback(); // Da usare per il debug o per provare se le query funzionano;
                }
            }
        }

        // HELPER
        public FilmViewModel GetFilmById(int id)
        {
            FilmViewModel filmToReturn;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Title, ReleaseDate, Director, Producer, GenreId FROM Film WHERE FilmId = @FilmId";

                    command.Parameters.AddWithValue("FilmId", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ResourceNotFoundException("Film by FilmId not found");

                        int titlePos = reader.GetOrdinal("Title");
                        int releaseDatePos = reader.GetOrdinal("ReleaseDate");
                        int directorPos = reader.GetOrdinal("Director");
                        int producerPos = reader.GetOrdinal("Producer");
                        int genreIdPos = reader.GetOrdinal("GenreId");

                        filmToReturn = new FilmViewModel()
                        {
                            Title = reader.GetString(titlePos),
                            Director = reader.GetString(directorPos),
                            Producer = reader.GetString(producerPos) ?? null,
                            ReleaseDate = reader.GetDateTime(releaseDatePos),
                            // Cast = new List<string>(),
                            GenreId = reader.GetInt16(genreIdPos)
                        };


                    }
                }
                using (SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT [Name] FROM Actor WHERE FilmId = @FilmId;";
                    command.CommandType = CommandType.Text;

                    command.Parameters.AddWithValue("FilmId", id);

                    using(SqlDataReader reader = command.ExecuteReader()) {
                        while (reader.Read())
                        {
                            filmToReturn.Cast.Add(reader.GetString(0)); 
                        }
                    }

                }
            }
            return filmToReturn;
        }
    }
}