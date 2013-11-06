using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using System.Runtime.Serialization.Json;

namespace Theatre
{
    public class Storage
    {
        public string url = "http://api.themoviedb.org/3/";
        public string api_key = "api_key=094e4bb3d3ab45a67d695ba730de8393";

        private static Storage instance;

        private Storage() { }

        public static Storage Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Storage();
                }
                return instance;
            }
        }

        public static Dictionary ParsePage(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary));

            MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(toParse));

            Dictionary result = (Dictionary)ser.ReadObject(stream1);
            return result;
        }

        public static Movie ParseMovie(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Movie));

            MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(toParse));

            Movie result = (Movie)ser.ReadObject(stream1);
            return result;
        }


        /// <summary>
        /// Запрос популярных фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetTop(string onpage = "1", Action<Dictionary> callback = null)
        {
            var request = WebRequest.CreateHttp(url + "movie/top_rated?page=" + onpage + '&' + api_key + "&language=ru");
            request.Method = "GET";
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Dictionary Top = ParsePage(responseContent);
                callback(Top);
            }, null);
        }

        /// <summary>
        /// Запрос ожидаемых в ближайшее время фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetUpcoming(string onpage = "1", Action<Dictionary> callback = null)
        {
            var request = WebRequest.CreateHttp(url + "movie/upcoming?page=" + onpage + '&' + api_key + "&language=ru");
            request.Method = "GET";
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Dictionary Upcoming = ParsePage(responseContent);
                callback(Upcoming);
            }, null);
        }

        /// <summary>
        /// Запрос идущих в кинотеатрах фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetNowPlaying(string onpage = "1", Action<Dictionary> callback = null)
        {
            var request = WebRequest.CreateHttp(url + "movie/now_playing?page=" + onpage + '&' + api_key +"&language=ru");
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Dictionary NowPlaying = ParsePage(responseContent);
                callback(NowPlaying);
            }, null);
        }

        /// <summary>
        /// Осуществляет запрос по идентификатору фильма
        /// </summary>
        /// <param name="mov_id">Идентификатор фильма</param>
        /// <returns>Данные о фильме</returns>
        public void GetMovieById(string mov_id, Action<Movie> callback = null)
        {
            var request = WebRequest.CreateHttp(url + "movie/" + mov_id + '?' + api_key + "&language=ru");
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Movie MovieById = ParseMovie(responseContent);
                callback(MovieById);
            }, null);
        }
    }
}
