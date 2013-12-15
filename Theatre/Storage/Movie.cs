using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
//using System.Windows.Media.Imaging;

namespace Theatre
{
    public enum MovieType { InCinema, Anounce };
    public class Movie
    {
        public string ID { get; set; }
        public string Title { get; set;}
        public string Url { get; set; }
        public System.Windows.Media.Imaging.BitmapImage PosterFullsize { get; set; }
        public System.Windows.Media.Imaging.BitmapImage PosterThumbnail { get; set; }
        public string PosterFullsizeURL { get; set; }
        public string PosterThumbnailURL { get; set; }
        public List<string> Countries { get; set; }
        public List<string> Genres { get; set; }
        public Dictionary<string,string> Actors { get; set; }
        public Dictionary<string,string> Directors { get; set; }
        public string CommentsCount { get; set; }
        public string ReviewsCount { get; set; }
        public string TrailersCount { get; set; }
        public string PhotosCount { get; set; }
        public string Is3d { get; set; }
        public string Cinema { get; set; }
        public string CinemaLink { get; set; }
        public MovieType Type { get; set; }

        ///////// from HTML ////////
        public string Description { get; set; }
        public string Length { get; set; }
        public event EventHandler LengthDidUpdated;
        public event EventHandler DescriptionDidUpdated;
        public event EventHandler ReleaseDateDidUpdated;
        ////////////////////////////

        /////// for NowMovie ///////
        public string Rating { get; set; }
        public string VoteCount { get; set; }
        public string IMDB { get; set; }
        public List<Session> Sessions;
        ////////////////////////////

        ///// for UpcomingMovie ////
        public string DaysForPremier  { get; set; }
        public string ReleaseDate { get; set; }
        public string Budget  { get; set; }
        ////////////////////////////


        public string ShortDescription { get; set; }

        public Movie(NowMovie SomeMovie) : this((MovieForList)SomeMovie)
        {
            Type = MovieType.InCinema;
            Rating = SomeMovie.vote;
            VoteCount = SomeMovie.count_vote;
            IMDB = SomeMovie.imdb;
            Sessions = new List<Session>();
            //Sessions2ParseTickets(
            foreach (SessionResponse Item in SomeMovie.sessions)
            {
                if (Item.k_name != null)
                {
                    Sessions.Add(new Session(Item));
                }
                else
                {
                    Sessions[Sessions.Count - 1].AppendResponce(Item);
                }
            }
            ShortDescription = "Рейтинг: " + Rating;
        }

        public Movie(UpcomingMovie SomeMovie) : this ((MovieForList)SomeMovie)
        {
            Type = MovieType.Anounce;
            DaysForPremier = SomeMovie.before;
            ReleaseDate = SomeMovie.entered.Replace("<b>", "").Replace("</b>","");
            Budget = SomeMovie.worldwide;
            ShortDescription = "Релиз: " + ReleaseDate;
        }

        public Movie(MovieForList SomeMovie)
        {
            ID = SomeMovie.id;
            Title = SomeMovie.name;
            Url = "http://kinoafisha.ua" + SomeMovie.url;
            PosterThumbnailURL = "http://kinoafisha.ua" + SomeMovie.image;
            PosterFullsizeURL = PosterThumbnailURL.Replace("/sm_", "/bp_");

            string[] countriesYearAndGenres = SomeMovie.countries.Split('(');
            string[] countriesAndYear = countriesYearAndGenres[0].Split(',');
            string[] genresParts = countriesYearAndGenres[1].Replace(")","").Split(',');
            Countries = new List<string>();
            for (int i=0; i < countriesAndYear.Length - 1; i++)
            {
                string countryString = countriesAndYear[i];
                if (i != 0)
                {
                    Countries.Add(countryString.Remove(0,1));
                }
                else
                {
                    Countries.Add(countryString);
                }
            }
            Genres = new List<string>();
            for (int i = 0; i < genresParts.Length; i++)
            {
                string genreString = genresParts[i];
                if (i != 0)
                {
                    Genres.Add(genreString.Remove(0, 1));
                }
                else
                {
                    Genres.Add(genreString);
                }
            }
            
            
            Actors = ParsePeopleString(SomeMovie.actors);
            Directors = ParsePeopleString(SomeMovie.rejisser);
            CommentsCount = SomeMovie.comments_count;
            ReviewsCount = SomeMovie.reviews_count;
            TrailersCount = SomeMovie.trailers_count;
            PhotosCount = SomeMovie.photos_count;
            Is3d = SomeMovie.is3d;
            Cinema = SomeMovie.is_b;
            CinemaLink = SomeMovie.b_link;

            var request = WebRequest.CreateHttp(Url);
            request.Method = "GET";
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                ParseMovieHTMLPage(responseContent);
            }, null);
        }

        Dictionary<string, string> ParsePeopleString(string ToParse)
        {
            string[] actors = ToParse.Split(',');
            Dictionary<string, string> dict =  new Dictionary<string, string>();
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

        void ParseMovieHTMLPage(string html)
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
            ParseDescriptionFromHTML(clearHTML);
            ParseLengthFromHTML(clearHTML);
            if (Type == MovieType.InCinema)
            {
                ParseReleaseDateFromHTML(clearHTML);
            }

        }

        void ParseDescriptionFromHTML(string html)
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
            Description = parseDescription;
            if (DescriptionDidUpdated != null)
            {
                DescriptionDidUpdated.Invoke(this, null);
            }
        }

        void ParseLengthFromHTML(string html)
        {
            if (html.IndexOf("<p>Продолжительность: <span>") == -1)
            {
                Length = "N/A";
                return;
            }
            string parsedLength = html.Replace("<p>Продолжительность: <span>", "\0").
                                      Replace("</span><span itemprop=\"duration\"", "\0").Split('\0')[1];
            Length = parsedLength;
            if (LengthDidUpdated != null)
            {
                LengthDidUpdated.Invoke(this, null);
            }
        }

        void ParseReleaseDateFromHTML(string html)
        {
            if (html.IndexOf("<p>Премьера в Украине: <span>") == -1)
            {
                ReleaseDate = "N/A";
                return;
            }
            string parsedDate = html.Replace("<p>Премьера в Украине: <span>", "\0").
                                     Split('\0')[1].
                                     Replace("</span></p>", "\0").Split('\0')[0];
            ReleaseDate = parsedDate;
            if (ReleaseDateDidUpdated != null)
            {
                ReleaseDateDidUpdated.Invoke(this, null);
            }
        }
    }

    public class ComparatorByRating : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            return y.Rating.CompareTo(x.Rating);
        }
    }

    public class ComparatorByReleaseDate : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            return x.ReleaseDate.CompareTo(y.ReleaseDate);
        }
    }
}
