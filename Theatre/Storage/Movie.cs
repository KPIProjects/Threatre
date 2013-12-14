using System.Collections.Generic;
using System.Runtime.Serialization;
//using System.Windows.Media.Imaging;

namespace Theatre
{
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
            Rating = SomeMovie.vote;
            VoteCount = SomeMovie.count_vote;
            IMDB = SomeMovie.imdb;
            Sessions = new List<Session>();
            foreach (Session Item in SomeMovie.sessions)
            {
                Sessions.Add(Item);
            }
            ShortDescription = "Рейтинг: " + Rating;
        }

        public Movie(UpcomingMovie SomeMovie) : this ((MovieForList)SomeMovie)
        {
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
    }
}
