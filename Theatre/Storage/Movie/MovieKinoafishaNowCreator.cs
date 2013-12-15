using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            movie.Sessions = new List<Session>();
            foreach (SessionResponse Item in SomeMovie.sessions)
            {
                if (Item.k_name != null)
                {
                    movie.Sessions.Add(new Session(Item));
                }
                else
                {
                    movie.Sessions[movie.Sessions.Count - 1].AppendResponce(Item);
                }
            }
            movie.ShortDescription = "Рейтинг: " + movie.Rating;
            return movie;
        }
    }
}
