using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections;
using Theatre.Storage.Movies;

namespace Theatre.Storage
{
    public class DataStorage
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

        private static DataStorage instance;
        public StorageDataSource DataSource;

        private DataStorage() { }

        public static DataStorage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new DataStorage();
                    instance.DataSource = new KinoafishaDataSource();
                }
                return instance;
            }
        }

        
    }
}
