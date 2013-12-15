using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;

namespace Theatre
{
    public class Storage
    {

        public List<Movie> NowMovies { get { return DataSource.NowMovies; } }
        public List<Movie> UpcomingMovies { get { return DataSource.UpcomingMovies; } }

        public void GetNowPlaying(int onpage = 1, Action<List<Movie>> callback = null,
            string city = "kiev", DateTime ondate = new DateTime(), string cinema = "null")
        {
            DataSource.GetNowPlaying(onpage, callback, city, ondate, cinema);
        }
        public void GetUpcoming(int onpage = 1, Action<List<Movie>> callback = null)
        {
            DataSource.GetUpcoming(onpage, callback);
        }

        private static Storage instance;
        public StorageDataSource DataSource;

        private Storage() { }

        public static Storage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Storage();
                    instance.DataSource = new KinoafishaDataSource();
                }
                return instance;
            }
        }

        
    }
}
