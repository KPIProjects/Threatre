using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Theatre.Storage;
using Theatre.Storage.Movies;

namespace Theatre.Storage
{
    public abstract class StorageDataSource
    {
        public abstract void GetNowPlaying(int onpage = 1, Action<List<Movie>> callback = null, DateTime ondate = new DateTime(), string cinema = "null");
        public abstract void GetUpcoming(int onpage = 1, Action<List<Movie>> callback = null);

        public List<Movie> NowMovies;
        public List<Movie> UpcomingMovies;
    }
}
