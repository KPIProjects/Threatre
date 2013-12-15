using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Theatre
{
    public abstract class StorageDataSource
    {
        public abstract void GetNowPlaying(int onpage = 1, Action<List<Movie>> callback = null,
            string city = "kiev", DateTime ondate = new DateTime(), string cinema = "null");
        public abstract void GetUpcoming(int onpage = 1, Action<List<Movie>> callback = null);

        public List<Movie> NowMovies;
        public List<Movie> UpcomingMovies;
    }
}
