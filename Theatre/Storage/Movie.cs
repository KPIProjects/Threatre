using System.Collections.Generic;
using System.Runtime.Serialization;
//using System.Windows.Media.Imaging;

namespace Theatre
{
    public class Movie
    {
        public string ID { get; set; }

        public string Title {get; set;}

        public string Url { get; set; }

        public System.Windows.Media.Imaging.BitmapImage PosterFullsize {get; set;}

        public System.Windows.Media.Imaging.BitmapImage PosterThumbnail { get { return posterThumbnail; } set { posterThumbnail = value; } }
        private System.Windows.Media.Imaging.BitmapImage posterThumbnail; 

        public string PosterFullsizeURL { get; set; }

        public string PosterThumbnailURL { get; set; }

        public string Countries { get; set; }

        public string Actors { get; set; }

        public string Director { get; set; }

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


        public Movie(NowMovie SomeMovie)
        {
            ID = SomeMovie.id;

            Title = SomeMovie.name;

            Url = SomeMovie.url;

            PosterThumbnailURL = "http://kinoafisha.ua/" +  SomeMovie.image;

            PosterFullsizeURL = PosterThumbnailURL.Replace("/sm_", "/bp_");

            Countries = SomeMovie.countries;

            Actors = SomeMovie.actors;

            Director = SomeMovie.rejisser;

            CommentsCount = SomeMovie.comments_count;

            ReviewsCount = SomeMovie.reviews_count;

            TrailersCount = SomeMovie.trailers_count;

            PhotosCount = SomeMovie.photos_count;

            Is3d = SomeMovie.is3d;

            Cinema = SomeMovie.is_b;

            CinemaLink = SomeMovie.b_link;

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

        public Movie(UpcomingMovie SomeMovie)
        {
            ID = SomeMovie.id;

            Title = SomeMovie.name;

            Url = SomeMovie.url;

            PosterThumbnailURL = "http://kinoafisha.ua/" + SomeMovie.image;

            PosterFullsizeURL = PosterThumbnailURL.Replace("/sm_", "/bp_");

            Countries = SomeMovie.countries;

            Actors = SomeMovie.actors;

            Director = SomeMovie.rejisser;

            CommentsCount = SomeMovie.comments_count;

            ReviewsCount = SomeMovie.reviews_count;

            TrailersCount = SomeMovie.trailers_count;

            PhotosCount = SomeMovie.photos_count;

            Is3d = SomeMovie.is3d;

            Cinema = SomeMovie.is_b;

            CinemaLink = SomeMovie.b_link;

            DaysForPremier = SomeMovie.before;

            ReleaseDate = SomeMovie.entered;

            Budget = SomeMovie.worldwide;

            ShortDescription = "Релиз: " + ReleaseDate;
        }
    }
}
