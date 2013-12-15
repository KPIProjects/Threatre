using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theatre.Storage.Session;

namespace Theatre.Storage.Movies
{
    class MovieKinoafishaNowCreator : MovieKinoafishaCreator
    {
        public static Movie CreateMovie(NowMovie SomeMovie)
        {
            Movie movie = MovieKinoafishaCreator.CreateMovie(SomeMovie);

            movie.Type = MovieType.InCinema;
            movie.Rating = SomeMovie.vote;
            movie.VoteCount = SomeMovie.count_vote;
            movie.IMDB = SomeMovie.imdb;
            movie.Sessions = new List<SessionInCinema>();
            foreach (SessionResponse Item in SomeMovie.sessions)
            {
                if (Item.k_name != null)
                {
                    movie.Sessions.Add(SessionKinoafishaCreator.CreateSession(Item));
                }
                else
                {
                    SessionKinoafishaCreator.AppendResponceToSession(movie.Sessions[movie.Sessions.Count - 1], Item);
                }
            }
            movie.ShortDescription = "Рейтинг: " + movie.Rating;
            return movie;
        }
    }
}
