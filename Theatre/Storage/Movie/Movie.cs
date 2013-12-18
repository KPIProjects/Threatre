using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using Theatre.Storage.Session;

namespace Theatre.Storage.Movies
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

        ///////// Extended ////////
        public List<string> MovieImageURLs { get; set; }
        public List<System.Windows.Media.Imaging.BitmapImage> MovieImages { get; set; }
        public string TrailerURL { get; set; }
        public string Description { get; set; }
        public string Length { get; set; }
        public event EventHandler LengthDidLoadEvent;
        public event EventHandler DescriptionDidLoadEvent;
        public event EventHandler ReleaseDateDidLoadEvent;
        ////////////////////////////

        /////// for NowMovie ///////
        public string Rating { get; set; }
        public string VoteCount { get; set; }
        public string IMDB { get; set; }
        public List<SessionInCinema> Sessions;
        ////////////////////////////

        ///// for UpcomingMovie ////
        public string DaysForPremier  { get; set; }
        public string ReleaseDate { get; set; }
        public string Budget  { get; set; }
        ////////////////////////////

        public string ShortDescription { get; set; }

        public void LengthDidLoad()
        {
            if (LengthDidLoadEvent != null)
            {
                LengthDidLoadEvent.Invoke(this, null);
            }
        }

        public void DescriptionDidLoad()
        {
            if (DescriptionDidLoadEvent != null)
            {
                DescriptionDidLoadEvent.Invoke(this, null);
            }
        }

        public void ReleaseDateDidLoad()
        {
            if (ReleaseDateDidLoadEvent != null)
            {
                ReleaseDateDidLoadEvent.Invoke(this, null);
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
