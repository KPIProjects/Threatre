using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Theatre
{
    class KinoafishaDataSource : StorageDataSource
    {
        public string url = "http://kinoafisha.ua";

        public static KinoafishaResponse<T> ParsePage<T>(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(KinoafishaResponse<T>));

            MemoryStream stream1 = new MemoryStream(Encoding.UTF8.GetBytes(toParse));

            KinoafishaResponse<T> result = (KinoafishaResponse<T>)ser.ReadObject(stream1);
            return result;
        }


        /// <summary>
        /// Запрос идущих в кинотеатрах фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы с фильмами</param>
        /// <param name="callback"></param>
        /// <param name="city">Город, в котором пользователь собирается смотреть фильм</param>
        /// <param name="ondate">Дата просмотра</param>
        /// <param name="kinoteatr">Кинотеатр, в котором пользователь собирается смотреть фильм</param>
        public override void GetNowPlaying(int onpage = 1, Action<List<Movie>> callback = null,
            string city = "kiev", DateTime ondate = new DateTime(), string cinema = "null")
        {
            if (onpage == 1) { NowMovies = new List<Movie>(); }

            DateTime StartDate = new DateTime(1970, 1, 1, 22, 0, 0);
            if (ondate.Year == 1) { ondate = DateTime.Now; }
            int date = (ondate - StartDate).Days * 60 * 60 * 24 + StartDate.Hour * 60 * 60;

            var request = WebRequest.CreateHttp(url + "/ajax/kinoafisha_load");
            request.Method = "POST";
            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes("city=" + city + "&date=" + date.ToString() + "&kinoteatr=" + cinema +
                "&offset=" + (onpage * 10 - 10).ToString() + "&limit=10");

            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream((IAsyncResult asynchronousResult) =>
            {
                using (var postStream = request.EndGetRequestStream(asynchronousResult))
                {
                    postStream.Write(bytes, 0, bytes.Length);
                }
                request.BeginGetResponse(result =>
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                    Stream streamResponse = response.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    String responseContent = streamRead.ReadToEnd();
                    KinoafishaResponse<NowMovie> NowPlaying = ParsePage<NowMovie>(responseContent);

                    foreach (NowMovie Item in NowPlaying.result)
                    {
                        NowMovies.Add(new Movie(Item));
                    }

                    callback(NowMovies);
                }, null);
            }, null);

        }

        /// <summary>
        /// Запрос ожидаемых в ближайшее время фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public override void GetUpcoming(int onpage = 1, Action<List<Movie>> callback = null)
        {
            if (onpage == 1) { UpcomingMovies = new List<Movie>(); }

            var request = WebRequest.CreateHttp(url + "/ajax/skoro_load");
            request.Method = "POST";
            //request.Credentials = CredentialCache.DefaultCredentials;
            UTF8Encoding encoding = new UTF8Encoding();
            var bytes = encoding.GetBytes("offset=" + (onpage * 10 - 10).ToString() + "&limit=10");

            request.ContentType = "application/x-www-form-urlencoded";
            request.BeginGetRequestStream((IAsyncResult asynchronousResult) =>
            {
                using (var postStream = request.EndGetRequestStream(asynchronousResult))
                {
                    postStream.Write(bytes, 0, bytes.Length);
                }
                request.BeginGetResponse(result =>
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                    Stream streamResponse = response.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    String responseContent = streamRead.ReadToEnd();
                    KinoafishaResponse<UpcomingMovie> Anounces = ParsePage<UpcomingMovie>(responseContent);

                    foreach (UpcomingMovie Item in Anounces.result)
                    {
                        UpcomingMovies.Add(new Movie(Item));
                    }

                    callback(UpcomingMovies);
                }, null);
            }, null);
        }
    }
}
