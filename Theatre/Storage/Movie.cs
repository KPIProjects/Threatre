using System.Collections.Generic;
using System.Runtime.Serialization;
//using System.Windows.Media.Imaging;

namespace Theatre
{
    class Movie
    {
        public string id;

        public string title;

        public string url;

        public string image_url;

        public string countries;

        public string actors;

        public string director;

        public string comments_count;

        public string reviews_count;

        public string trailers_count;

        public string photos_count;

        public string is3d;

        public string cinema;

        public string cinema_link;

        /////// for NowMovie ///////
        public string vote;

        public string count_vote;

        public string imdb;

        public List<Session> sessions;
        ////////////////////////////

        ///// for UpcomingMovie ////
        public string days_left;

        public string release;

        public string budget;
        ////////////////////////////

        //public BitmapImage Thumbnail { get; set; }


        public Movie(NowMovie SomeMovie)
        {
            id = SomeMovie.id;

            title = SomeMovie.name;

            url = SomeMovie.url;

            image_url = SomeMovie.image;

            countries = SomeMovie.countries;

            actors = SomeMovie.actors;

            director = SomeMovie.rejisser;

            comments_count = SomeMovie.comments_count;

            reviews_count = SomeMovie.reviews_count;

            trailers_count = SomeMovie.trailers_count;

            photos_count = SomeMovie.photos_count;

            is3d = SomeMovie.is3d;

            cinema = SomeMovie.is_b;

            cinema_link = SomeMovie.b_link;

            vote = SomeMovie.vote;

            count_vote = SomeMovie.count_vote;

            imdb = SomeMovie.imdb;

            sessions = new List<Session>();

            foreach (Session Item in SomeMovie.sessions)
            {
                sessions.Add(Item);
            }
        }

        public Movie(UpcomingMovie SomeMovie)
        {
            id = SomeMovie.id;

            title = SomeMovie.name;

            url = SomeMovie.url;

            image_url = SomeMovie.image;

            countries = SomeMovie.countries;

            actors = SomeMovie.actors;

            director = SomeMovie.rejisser;

            comments_count = SomeMovie.comments_count;

            reviews_count = SomeMovie.reviews_count;

            trailers_count = SomeMovie.trailers_count;

            photos_count = SomeMovie.photos_count;

            is3d = SomeMovie.is3d;

            cinema = SomeMovie.is_b;

            cinema_link = SomeMovie.b_link;

            days_left = SomeMovie.before;

            release = SomeMovie.entered;

            budget = SomeMovie.worldwide;
        }
    }
}
