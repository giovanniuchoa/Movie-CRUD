namespace Movie_CRUD.Models
{
    public class MovieModel
    {

        public int idMovie { get; set; }
        public string movieName { get; set; }

        public string movieCategory { get; set; }
        public int movieYear { get; set; }
        public decimal movieDuration { get; set; }

    }
}
