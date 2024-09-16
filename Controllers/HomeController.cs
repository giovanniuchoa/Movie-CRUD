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

        #region Create

        public async Task<IActionResult> CreateConfirm(string movieName, string movieCategory, int movieYear, decimal movieDuration)
        {

            await CreateMovieAsync(movieName, movieCategory, movieYear, movieDuration);

            return RedirectToAction("Index");

        }

        public async Task<ReturnModel> CreateMovieAsync(string movieName, string movieCategory, int movieYear, decimal movieDuration)
        {

            var connString = "Server=127.0.0.1;Port=3306;User ID=root;Database=moviecrud";

            await using var connection = new MySqlConnection(connString);
            await connection.OpenAsync();

            var ret = new ReturnModel();

            try
            {

                using (var command = new MySqlCommand(
                    "INSERT INTO `moviecrud`.`movie` (`movieName`, `movieCategory`, `movieYear`, `movieDuration`) " +
                    "VALUES (@movieName, @movieCategory, @movieYear, @movieDuration);", connection))
                {

                    command.Parameters.AddWithValue("@movieName", movieName);
                    command.Parameters.AddWithValue("@movieCategory", movieCategory);
                    command.Parameters.AddWithValue("@movieYear", movieYear);
                    command.Parameters.AddWithValue("@movieDuration", movieDuration);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {

                        ret.Erro = false;
                        ret.Mensagem = "Movie created successfully.";

                    }

                }

            }
            catch (Exception ex)
            {

                ret.Erro = true;
                ret.Mensagem = $"Error creating movie: {ex.Message}";

            }

            return ret;

        }

        #endregion

        #region Read
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

        [HttpGet]
        public async Task<MovieModel> GetMovieByIdAsync(int idMovie)
        {
            var connString = "Server=127.0.0.1;Port=3306;User ID=root;Database=moviecrud";

            await using var connection = new MySqlConnection(connString);
            await connection.OpenAsync();

            var movieModel = new MovieModel();

            using (var command = new MySqlCommand("SELECT * FROM movie WHERE idMovie = @idMovie", connection))
            {

                command.Parameters.AddWithValue("@idMovie", idMovie);

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

                    movieModel = movie;

                }

            }

            return movieModel;
        }

        #endregion

        #region Update

        public async Task<IActionResult> EditConfirm(int idMovie, string movieName, string movieCategory, int movieYear, decimal movieDuration)
        {

            await EditMovieAsync(idMovie, movieName, movieCategory, movieYear, movieDuration);

            return RedirectToAction("Index");

        }

        public async Task<ReturnModel> EditMovieAsync(int idMovie, string movieName, string movieCategory, int movieYear, decimal movieDuration)
        {

            var connString = "Server=127.0.0.1;Port=3306;User ID=root;Database=moviecrud";

            await using var connection = new MySqlConnection(connString);
            await connection.OpenAsync();

            var ret = new ReturnModel();

            try
            {

                using (var command = new MySqlCommand("UPDATE movie SET movieName = @movieName, movieCategory = @movieCategory, " +
                    "movieYear = @movieYear, movieDuration = @movieDuration WHERE idMovie = @idMovie", connection))
                {

                    command.Parameters.AddWithValue("@idMovie", idMovie);
                    command.Parameters.AddWithValue("@movieName", movieName);
                    command.Parameters.AddWithValue("@movieCategory", movieCategory);
                    command.Parameters.AddWithValue("@movieYear", movieYear);
                    command.Parameters.AddWithValue("@movieDuration", movieDuration);

                    int rowsAffected = await command.ExecuteNonQueryAsync();

                    if (rowsAffected > 0)
                    {

                        ret.Erro = false;
                        ret.Mensagem = "Movie updated successfully.";

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
                ret.Mensagem = $"Error updating movie: {ex.Message}";

            }

            return ret;

        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm(int idMovie)
        {

            await DeleteMovieAsync(idMovie);

            return RedirectToAction("Index");

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

        #endregion

        



    }

}

