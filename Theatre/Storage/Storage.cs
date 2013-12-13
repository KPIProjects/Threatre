﻿using System;
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
        public string url = "http://kinoafisha.ua";
        //public string api_key = "api_key=094e4bb3d3ab45a67d695ba730de8393";

        public List<Movie> NowMovies;
        public List<Movie> UpcomingMovies;

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

        public static Dictionary<T> ParsePage<T>(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Dictionary<T>));

            MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(toParse));

            Dictionary<T> result = (Dictionary<T>)ser.ReadObject(stream1);
            return result;
        }

        /*public static Movie ParseMovie(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Movie));

            MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(toParse));

            Movie result = (Movie)ser.ReadObject(stream1);
            return result;
        }*/

        /// <summary>
        /// Запрос идущих в кинотеатрах фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы с фильмами</param>
        /// <param name="callback"></param>
        /// <param name="city">Город, в котором пользователь собирается смотреть фильм</param>
        /// <param name="ondate">Дата просмотра</param>
        /// <param name="kinoteatr">Кинотеатр, в котором пользователь собирается смотреть фильм</param>
        public void GetNowPlaying(int onpage = 1, Action<List<Movie>> callback = null,
            string city = "kiev", DateTime ondate = new DateTime(), string kinoteatr = "null")
        {
            if (onpage == 1) { NowMovies = new List<Movie>(); }

            DateTime StartDate = new DateTime(1970, 1, 1, 22, 0, 0);
            if (ondate.Year == 1) { ondate = DateTime.Now; }
            int date = (ondate - StartDate).Days * 60 * 60 * 24 + StartDate.Hour * 60 * 60;

            var request = WebRequest.CreateHttp(url + "/ajax/kinoafisha_load");
            request.Method = "POST";
            //request.Credentials = CredentialCache.DefaultCredentials;
            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes("city=" + city + "&date=" + date.ToString() + "&kinoteatr=" + kinoteatr + 
                "&offset=" + (onpage * 10 - 10).ToString() + "&limit=" + (onpage * 10).ToString());

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }

            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Dictionary<NowMovie> NowPlaying = ParsePage<NowMovie>(responseContent);

                foreach (NowMovie Item in NowPlaying.result)
                {
                    NowMovies.Add(new Movie(Item));
                }

                callback(NowMovies);
            }, null);
        }
        /*
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
        }*/

        /// <summary>
        /// Запрос ожидаемых в ближайшее время фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetUpcoming(int onpage = 1, Action<List<Movie>> callback = null)
        {
            if (onpage == 1) { UpcomingMovies = new List<Movie>(); }

            var request = WebRequest.CreateHttp(url + "/ajax/skoro_load");
            request.Method = "POST";
            //request.Credentials = CredentialCache.DefaultCredentials;
            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes("offset="+(onpage*10-10).ToString()+"&limit="+(onpage*10).ToString());

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = bytes.Length;

            using (var newStream = request.GetRequestStream())
            {
                newStream.Write(bytes, 0, bytes.Length);
                newStream.Close();
            }

            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Dictionary<UpcomingMovie> Upcoming = ParsePage<UpcomingMovie>(responseContent);

                foreach (UpcomingMovie Item in Upcoming.result)
                {
                    NowMovies.Add(new Movie(Item));
                }

                callback(UpcomingMovies);
            }, null);
        }
        
        /// <summary>
        /// Осуществляет запрос по идентификатору фильма
        /// </summary>
        /// <param name="mov_url">Ссылка на фильм</param>
        /// <returns>Данные о фильме</returns>
        /*public void GetMovieByURL(string mov_url, Action<Movie> callback = null)
        {
            var request = WebRequest.CreateHttp(url + mov_url);
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
        }*/
    }
}
