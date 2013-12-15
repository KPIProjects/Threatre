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

            WebRequest request = WebRequest.CreateHttp(movie.Url);
            request.Method = "GET";
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                ParseMovieHTMLPage(movie, responseContent);
            }, null);
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

        private static void ParseMovieHTMLPage(Movie movie, string html)
        {
            string clearHTML = html.Replace("\n\t\t\t\t", "").
                                    Replace("\t\t\t", "").
                                    Replace("<p><span class=\"_reachbanner_\">", "").
                                    Replace("<span class=\"hps\">", "").
                                    Replace("<span id=\"result_box\" lang=\"ru\">", "").
                                    Replace("&hellip;", "...").
                                    Replace("&ndash;", " - ").
                                    Replace("&mdash;", "-").
                                    Replace("&amp;", "&").
                                    Replace("&lt;;", "<").
                                    Replace("&gt;", ">").
                                    Replace("&laquo;", "«").
                                    Replace("&raquo;", "»").
                                    Replace("&amp;", "&").
                                    Replace("&nbsp;", " ");
            ParseDescriptionFromHTML(movie, clearHTML);
            ParseLengthFromHTML(movie, clearHTML);
            if (movie.Type == MovieType.InCinema)
            {
                ParseReleaseDateFromHTML(movie, clearHTML);
            }

        }

        private static void ParseDescriptionFromHTML(Movie movie, string html)
        {
            string parseDescription = html.Replace("<div class=\"description\" itemprop=\"description\">", "\0").
                                           Replace("</div> <!-- end description -->", "\0").
                                           Replace("<p>", "").
                                           Replace("</p>", "").
                                           Replace("<br />", "\n").
                                           Replace("</span>", "").
                                           Split('\0')[1];

            // Clear of span class tag //
            int idx = -1;
            do
            {
                idx = parseDescription.IndexOf("<span class=\"");
                if (idx != -1)
                {
                    int lastidx = parseDescription.IndexOf("\">");
                    parseDescription = parseDescription.Remove(idx, lastidx - idx + 2);
                }
            }
            while (idx != -1);

            // Clear of span title tag //
            idx = -1;
            do
            {
                idx = parseDescription.IndexOf("<span title=\"");
                if (idx != -1)
                {
                    int lastidx = parseDescription.IndexOf("\">");
                    parseDescription = parseDescription.Remove(idx, lastidx - idx + 2);
                }
            }
            while (idx != -1);
            movie.Description = parseDescription;
            movie.DescriptionDidLoad();
        }

        private static void ParseLengthFromHTML(Movie movie, string html)
        {
            if (html.IndexOf("<p>Продолжительность: <span>") == -1)
            {
                movie.Length = "N/A";
                return;
            }
            string parsedLength = html.Replace("<p>Продолжительность: <span>", "\0").
                                      Replace("</span><span itemprop=\"duration\"", "\0").Split('\0')[1];
            movie.Length = parsedLength;
            movie.LengthDidLoad();
        }

        private static void ParseReleaseDateFromHTML(Movie movie, string html)
        {
            if (html.IndexOf("<p>Премьера в Украине: <span>") == -1)
            {
                movie.ReleaseDate = "N/A";
                return;
            }
            string parsedDate = html.Replace("<p>Премьера в Украине: <span>", "\0").
                                     Split('\0')[1].
                                     Replace("</span></p>", "\0").Split('\0')[0];
            movie.ReleaseDate = parsedDate;
            movie.ReleaseDateDidLoad();
        }
    }
}
