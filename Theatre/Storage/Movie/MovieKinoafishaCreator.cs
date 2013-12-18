using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Theatre.Storage.Movies
{
    abstract class MovieKinoafishaCreator
    {
        public static Movie CreateMovie(MovieKinoafishaResponse SomeMovie)
        {
            Movie movie = new Movie();
            movie.ID = SomeMovie.id;
            movie.Title = SomeMovie.name;
            movie.Url = "http://kinoafisha.ua" + SomeMovie.url;
            movie.PosterThumbnailURL = "http://kinoafisha.ua" + SomeMovie.image;
            movie.PosterFullsizeURL = movie.PosterThumbnailURL.Replace("/sm_", "/bp_");

            string[] countriesYearAndGenres = SomeMovie.countries.Split('(');
            string[] countriesAndYear = countriesYearAndGenres[0].Split(',');
            string[] genresParts = countriesYearAndGenres[1].Replace(")", "").Split(',');
            movie.Countries = new List<string>();
            for (int i = 0; i < countriesAndYear.Length - 1; i++)
            {
                string countryString = countriesAndYear[i];
                if (i != 0)
                {
                    movie.Countries.Add(countryString.Remove(0, 1));
                }
                else
                {
                    movie.Countries.Add(countryString);
                }
            }
            movie.Genres = new List<string>();
            for (int i = 0; i < genresParts.Length; i++)
            {
                string genreString = genresParts[i];
                if (i != 0)
                {
                    movie.Genres.Add(genreString.Remove(0, 1));
                }
                else
                {
                    movie.Genres.Add(genreString);
                }
            }


            movie.Actors = ParsePeopleString(SomeMovie.actors);
            movie.Directors = ParsePeopleString(SomeMovie.rejisser);
            movie.CommentsCount = SomeMovie.comments_count;
            movie.ReviewsCount = SomeMovie.reviews_count;
            movie.TrailersCount = SomeMovie.trailers_count;
            movie.PhotosCount = SomeMovie.photos_count;
            movie.Is3d = SomeMovie.is3d;
            movie.Cinema = SomeMovie.is_b;
            movie.CinemaLink = SomeMovie.b_link;
            return movie;
        }

        private static Dictionary<string, string> ParsePeopleString(string ToParse)
        {
            string[] actors = ToParse.Split(',');
            Dictionary<string, string> dict = new Dictionary<string, string>();
            foreach (string actor in actors)
            {
                string actorstring = actor;
                if (actor == "") continue;
                if (actorstring[0] == ' ')
                {
                    actorstring = actor.Remove(0, 1);
                }
                if (actorstring[0] == '<')
                {
                    actorstring = actorstring.Replace("<a href=\"", "");
                    actorstring = actorstring.Replace("</a>", "");
                    int closeLinkSymbolPosition = actorstring.IndexOf(">");
                    string name = actorstring.Substring(closeLinkSymbolPosition + 1, actorstring.Length - closeLinkSymbolPosition - 1);
                    string link = "http://kinoafisha.ua" + actorstring.Substring(0, closeLinkSymbolPosition - 1);
                    dict.Add(name, link);
                }
                else
                {
                    dict.Add(actorstring, "");
                }
            }
            return dict;
        }
    }
}
