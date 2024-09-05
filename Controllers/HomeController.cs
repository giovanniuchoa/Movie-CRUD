using Microsoft.AspNetCore.Mvc;
using Movie_CRUD.Models;
using MySqlConnector;

namespace Movie_CRUD.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            List<MovieModel> temp = await GetMovieAsync();

            return View(temp);
        }

        public async Task<List<MovieModel>> GetMovieAsync()
        {
            var connString = "Server=127.0.0.1;Port=3306;User ID=root;Database=moviecrud";

            await using var connection = new MySqlConnection(connString);
            await connection.OpenAsync();

            var moviesList = new List<MovieModel>();

            using (var command = new MySqlCommand("SELECT * FROM movie", connection))
            {
                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {

                    var movie = new MovieModel
                    {
                        idMovie = reader.GetInt32(0),
                        movieName = reader.GetString(1),
                        movieCategory = reader.GetString(2),
                        movieYear = reader.GetInt32(3),
                        movieDuration = reader.GetDecimal(4)
                    };

                    moviesList.Add(movie);
                }

            }

            return moviesList;

        }

        public async Task<ReturnModel> DeleteMovieAsync(int idMovie)
        {

            var connString = "Server=127.0.0.1;Port=3306;User ID=root;Database=moviecrud";

            await using var connection = new MySqlConnection(connString);
            await connection.OpenAsync();

            var ret = new ReturnModel();

            try
            {

                using (var command = new MySqlCommand("DELETE FROM movie WHERE idMovie = @idMovie", connection))
                {

                    command.Parameters.AddWithValue("@idMovie", idMovie);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {

                        ret.Erro = false;
                        ret.Mensagem = "Movie deleted successfully.";

                    }
                    else
                    {

                        ret.Erro = true;
                        ret.Mensagem = "No movie was found with the provided id.";

                    }

                }

            }
            catch (Exception ex)
            {

                ret.Erro = true;
                ret.Mensagem = $"Error deleting movie: {ex.Message}";

            }

            return ret;

        }

    }

}

