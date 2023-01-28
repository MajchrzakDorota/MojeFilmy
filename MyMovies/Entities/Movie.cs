namespace MyMovies.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } // check nullable
        public string Director { get; set; } // same as above
        public int Year { get; set; }
        public float Rate { get; set; }

    }
}
