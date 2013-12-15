using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theatre.Storage.Movies
{
    class MovieKinoafishaUpcomingCreator : MovieKinoafishaCreator
    {
        public static Movie CreateMovie(UpcomingMovie SomeMovie)
        {
            Movie movie = MovieKinoafishaCreator.CreateMovie(SomeMovie);

            movie.Type = MovieType.Anounce;
            movie.DaysForPremier = SomeMovie.before;
            movie.ReleaseDate = SomeMovie.entered.Replace("<b>", "").Replace("</b>", "");
            movie.Budget = SomeMovie.worldwide;
            movie.ShortDescription = "Релиз: " + movie.ReleaseDate;

            return movie;
        }
    }
}
