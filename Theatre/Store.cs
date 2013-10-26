using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Collections;
using System.Net;
using System.IO;
//using System.Net;
using System.Xml.Linq;
using System.Xml;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;

namespace Theatre
{
    /// <summary>
    /// Класс, который содержит в себе список фильмов на странице, номер страницы, 
    /// количество страниц и количество фильмов.
    /// </summary>
    [DataContract]
    public class Dictionary
    {
        [DataMember]
        public string page;

        [DataMember]
        public string total_pages;

        [DataMember]
        public string total_results;

        [DataMember]
        public List<Movie> results;
    }

    [DataContract]
    public class Item
    {
        [DataMember]
        string id;

        [DataMember]
        string name;

        public string Id
        {
            set { id = value; }
            get { return id; }
        }
        public string Name
        {
            set { name = value; }
            get { return name; }
        }

        public void Show()
        {
            Console.Write(Name);
        }
    }

    [DataContract]
    public class Movie
    {
        [DataMember]
        public string adult;

        [DataMember]
        public string backdrop_path;

        [DataMember]
        public string collection;

        [DataMember]
        public string budget;

        [DataMember]
        public List<Item> genres;

        [DataMember]
        public string homepage;

        [DataMember]
        public string id;

        [DataMember]
        public string imdb_id;

        [DataMember]
        public string original_title;

        [DataMember]
        public string overview;

        [DataMember]
        public string popularity;

        [DataMember]
        public string poster_path;

        [DataMember]
        public List<Item> production_companies;

        [DataMember]
        public List<Item> production_countries;

        [DataMember]
        public string release_date;

        [DataMember]
        public string revenue;

        [DataMember]
        public string runtime;

        [DataMember]
        public List<Item> spoken_languages;

        [DataMember]
        public string status;

        [DataMember]
        public string tagline;

        [DataMember]
        public string title;

        [DataMember]
        public string vote_average;

        [DataMember]
        public string vote_count;

        public void Show()
        {
            if (title != null) Console.WriteLine("Title = {0}", title);
            if (backdrop_path != null) Console.WriteLine("Backdrop_path = {0}", backdrop_path);
            if (collection != null) Console.WriteLine("Collection = {0}", collection);
            if (budget != null) Console.WriteLine("Budget = {0}", budget);
            if (genres != null)
            {
                Console.Write("Genres: ");
                for (int i = 0; i < genres.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)genres[i]).Show();
                }
                Console.WriteLine();
            }
            if (homepage != null) Console.WriteLine("Homepage = {0}", homepage);
            if (id != null) Console.WriteLine("Id = {0}", id);
            if (imdb_id != null) Console.WriteLine("Id_imdb = {0}", imdb_id);
            if (original_title != null) Console.WriteLine("Original_title = {0}", original_title);
            if (overview != null) Console.WriteLine("Overview = {0}", overview);
            if (popularity != null) Console.WriteLine("Popularity = {0}", popularity);
            if (poster_path != null) Console.WriteLine("Poster_path = {0}", poster_path);
            if (production_companies != null)
            {
                Console.Write("Production_companies: ");
                for (int i = 0; i < production_companies.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)production_companies[i]).Show();
                }
                Console.WriteLine();
            }
            if (production_countries != null)
            {
                Console.Write("Production_countries: ");
                for (int i = 0; i < production_countries.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)production_countries[i]).Show();
                }
                Console.WriteLine();
            }
            if (release_date != null) Console.WriteLine("Release_date = {0}", release_date);
            if (revenue != null) Console.WriteLine("Revenue = {0}", revenue);
            if (runtime != null) Console.WriteLine("Runtime = {0}", runtime);
            if (spoken_languages != null)
            {
                Console.Write("Spoken_languages: ");
                for (int i = 0; i < spoken_languages.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)spoken_languages[i]).Show();
                }
                Console.WriteLine();
            }
            if (status != null) Console.WriteLine("Status = {0}", status);
            if (tagline != null) Console.WriteLine("Tagline = {0}", tagline);
            if (title != null) Console.WriteLine("Title = {0}", title);
            if (vote_average != null) Console.WriteLine("Vote_average = {0}", vote_average);
            if (vote_count != null) Console.WriteLine("Vote_count = {0}", vote_count);
        }
    }

    public class Storage
    {
        public string url = "http://api.themoviedb.org/3/";
        public string api_key = "&api_key=094e4bb3d3ab45a67d695ba730de8393";

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

            MemoryStream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(toParse));

            Dictionary result = (Dictionary)ser.ReadObject(stream1);
            return result;
        }

        public static Movie ParseMovie(String toParse)
        {
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Movie));

            MemoryStream stream1 = new MemoryStream(ASCIIEncoding.UTF8.GetBytes(toParse));

            Movie result = (Movie)ser.ReadObject(stream1);
            return result;
        }


        public Dictionary Top;
        public event EventHandler TopChanged;
        /// <summary>
        /// Запрос популярных фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetTop(string onpage = "1")
        {
            var request = WebRequest.CreateHttp(url + "movie/top_rated?page=" + onpage + api_key);
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Top = ParsePage(responseContent);
                TopChanged.Invoke(Top, null);
            }, null);
        }
        
        //private void GenerateTop(string response, string onpage)
        //{
        //    List<Movie> Movies = new List<Movie>();
        //    string str = response;

        //    string page = GetData(ref str, "{\"page\":", ",\"results\":");
        //    while (str[1] != ']')
        //    {
        //        string adult = GetData(ref str, "{\"adult\":", ",");
        //        string backdrop_path = GetData(ref str, "\"backdrop_path\":", ",");
        //        string id = GetData(ref str, "\"id\":", ",");
        //        string original_title = GetData(ref str, "\"original_title\":", ",");
        //        string release_date = GetData(ref str, "\"release_date\":", ",");
        //        string poster_path = GetData(ref str, "\"poster_path\":", ",");
        //        string popularity = GetData(ref str, "\"popularity\":", ",");
        //        string title = GetData(ref str, "\"title\":", ",");
        //        string vote_average = GetData(ref str, "\"vote_average\":", ",");
        //        string vote_count = GetData(ref str, "\"vote_count\":", "}");
        //        if (str[1] == ',') str = str.Remove(1, 1);
        //        Movies.Add(new Movie(adult, backdrop_path, null, null, null, null, id, null, original_title, null,
        //            popularity, poster_path, null, null, release_date, null, null, null, null, null, title,
        //            vote_average, vote_count));
        //    }
        //    GetData(ref str, "[", "],");
        //    string total_pages = GetData(ref str, "\"total_pages\":", ",");
        //    string total_results = GetData(ref str, "\"total_results\":", "}");

        //    Top = new Dictionary(Movies, page, total_pages, total_results);
        //    TopChanged.Invoke(null, null);
        //}


        public Dictionary Upcoming;
        public event EventHandler UpcomingChanged;
        /// <summary>
        /// Запрос ожидаемых в ближайшее время фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetUpcoming(string onpage = "1")
        {
            var request = WebRequest.CreateHttp(url + "movie/top_rated?page=" + onpage + api_key);
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                Upcoming = ParsePage(responseContent);
                UpcomingChanged.Invoke(Upcoming, null);
            }, null);
        }
        
        //private void GenerateUpcoming(string response, string onpage)
        //{
        //    string str = response;
        //    List<Movie> Movies = new List<Movie>();

        //    GetData(ref str, "{\"dates\"", ":");
        //    string min_date = GetData(ref str, "{\"minimum\":", ",");
        //    string max_date = GetData(ref str, "\"maximum\":", "},");

        //    string page = GetData(ref str, "\"page\":", ",\"results\":");
        //    while (str[1] != ']')
        //    {
        //        string adult = GetData(ref str, "{\"adult\":", ",");
        //        string backdrop_path = GetData(ref str, "\"backdrop_path\":", ",");
        //        string id = GetData(ref str, "\"id\":", ",");
        //        string original_title = GetData(ref str, "\"original_title\":", ",");
        //        string release_date = GetData(ref str, "\"release_date\":", ",");
        //        string poster_path = GetData(ref str, "\"poster_path\":", ",");
        //        string popularity = GetData(ref str, "\"popularity\":", ",");
        //        string title = GetData(ref str, "\"title\":", ",");
        //        string vote_average = GetData(ref str, "\"vote_average\":", ",");
        //        string vote_count = GetData(ref str, "\"vote_count\":", "}");
        //        if (str[1] == ',') str = str.Remove(1, 1);
        //        Movies.Add(new Movie(adult, backdrop_path, null, null, null, null, id, null, original_title, null,
        //            popularity, poster_path, null, null, release_date, null, null, null, null, null, title,
        //            vote_average, vote_count));
        //    }
        //    GetData(ref str, "[", "],");
        //    string total_pages = GetData(ref str, "\"total_pages\":", ",");
        //    string total_results = GetData(ref str, "\"total_results\":", "}");

        //    Upcoming = new Dictionary(Movies, page, total_pages, total_results);
        //    UpcomingChanged.Invoke(null, null);

        //}


        public Dictionary NowPlaying;
        public event EventHandler NowPlayingChanged;
        /// <summary>
        /// Запрос идущих в кинотеатрах фильмов, разделеных по страницам
        /// </summary>
        /// <param name="onpage">Номер страницы</param>
        /// <returns></returns>
        public void GetNowPlaying(string onpage = "1")
        {
            var request = WebRequest.CreateHttp(url + "movie/top_rated?page=" + onpage + api_key);
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                NowPlaying = ParsePage(responseContent);
                NowPlayingChanged.Invoke(NowPlaying, null);
            }, null);
        }
        
        //private void GenerateNowPlaying(string response, string onpage)
        //{
        //    string str = response;

        //    List<Movie> Movies = new List<Movie>();

        //    GetData(ref str, "{\"dates\"", ":");
        //    string min_date = GetData(ref str, "{\"minimum\":", ",");
        //    string max_date = GetData(ref str, "\"maximum\":", "},");

        //    string page = GetData(ref str, "\"page\":", ",\"results\":");
        //    while (str[1] != ']')
        //    {
        //        string adult = GetData(ref str, "{\"adult\":", ",");
        //        string backdrop_path = GetData(ref str, "\"backdrop_path\":", ",");
        //        string id = GetData(ref str, "\"id\":", ",");
        //        string original_title = GetData(ref str, "\"original_title\":", ",");
        //        string release_date = GetData(ref str, "\"release_date\":", ",");
        //        string poster_path = GetData(ref str, "\"poster_path\":", ",");
        //        string popularity = GetData(ref str, "\"popularity\":", ",");
        //        string title = GetData(ref str, "\"title\":", ",");
        //        string vote_average = GetData(ref str, "\"vote_average\":", ",");
        //        string vote_count = GetData(ref str, "\"vote_count\":", "}");
        //        if (str[1] == ',') str = str.Remove(1, 1);
        //        Movies.Add(new Movie(adult, backdrop_path, null, null, null, null, id, null, original_title, null,
        //            popularity, poster_path, null, null, release_date, null, null, null, null, null, title,
        //            vote_average, vote_count));
        //    }
        //    GetData(ref str, "[", "],");
        //    string total_pages = GetData(ref str, "\"total_pages\":", ",");
        //    string total_results = GetData(ref str, "\"total_results\":", "}");

        //    NowPlaying = new Dictionary(Movies, page, total_pages, total_results);
        //    NowPlayingChanged.Invoke(null, null);
        //}


        public Movie MovieById;
        public event EventHandler MovieByIdChanged;
        /// <summary>
        /// Осуществляет запрос по идентификатору фильма
        /// </summary>
        /// <param name="mov_id">Идентификатор фильма</param>
        /// <returns>Данные о фильме</returns>
        public void GetMovieById(string mov_id)
        {
            var request = WebRequest.CreateHttp(url + "movie/" + mov_id + api_key);
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                StreamReader streamRead = new StreamReader(streamResponse);
                String responseContent = streamRead.ReadToEnd();
                MovieById = ParseMovie(responseContent);
                MovieByIdChanged.Invoke(MovieById, null);
            }, null);
        }
        
        //private void GenerateMovieById(string responseContent, string mov_id)
        //{
        //    string film = responseContent;
        //    string adult = GetData(ref film, "{\"adult\":", ",");
        //    string backdrop_path = GetData(ref film, "\"backdrop_path\":", ",");
        //    string collection = GetData(ref film, "\"belongs_to_collection\":", ",");
        //    string budget = GetData(ref film, "\"budget\":", ",");
        //    List<Item> genres = new List<Item>();
        //    GetData(ref film, "\"genres\"", ":");
        //    while (film[1] != ']')
        //    {
        //        genres.Add(new Item(GetData(ref film, "{\"id\":", ","), GetData(ref film, "\"name\":", "}")));
        //        if (film[1] == ',') film = film.Remove(1, 1);
        //    }
        //    GetData(ref film, "[", "],");
        //    string homepage = GetData(ref film, "\"homepage\":", ",");
        //    string id = GetData(ref film, "\"id\":", ",");
        //    string id_imdb = GetData(ref film, "\"imdb_id\":", ",");
        //    string original_title = GetData(ref film, "\"original_title\":", ",");
        //    string overview = GetData(ref film, "\"overview\":", ",");
        //    string popularity = GetData(ref film, "\"popularity\":", ",");
        //    string poster_path = GetData(ref film, "\"poster_path\":", ",");
        //    List<Item> production_companies = new List<Item>();
        //    GetData(ref film, "\"production_companies\"", ":");
        //    while (film[1] != ']')
        //    {
        //        production_companies.Add(new Item(GetData(ref film, "\"id\":", "}"), GetData(ref film, "{\"name\":", ",")));
        //        if (film[1] == ',') film = film.Remove(1, 1);
        //    }
        //    GetData(ref film, "[", "],");
        //    List<Item> production_countries = new List<Item>();
        //    GetData(ref film, "\"production_countries\"", ":");
        //    while (film[1] != ']')
        //    {
        //        production_countries.Add(new Item(GetData(ref film, "{\"iso_3166_1\":\"", "\","), GetData(ref film, "\"name\":", "}")));
        //        if (film[1] == ',') film = film.Remove(1, 1);
        //    }
        //    GetData(ref film, "[", "],");
        //    string release_date = GetData(ref film, "\"release_date\":", ",");
        //    string revenue = GetData(ref film, "\"revenue\":", ",");
        //    string runtime = GetData(ref film, "\"runtime\":", ",");
        //    List<Item> spoken_languages = new List<Item>();
        //    GetData(ref film, "\"spoken_languages\"", ":");
        //    while (film[1] != ']')
        //    {
        //        spoken_languages.Add(new Item(GetData(ref film, "{\"iso_639_1\":", ","), GetData(ref film, "\"name\":", "}")));
        //        if (film[1] == ',') film = film.Remove(1, 1);
        //    }
        //    GetData(ref film, "[", "],");
        //    string status = GetData(ref film, "\"status\":", ",");
        //    string tagline = GetData(ref film, "\"tagline\":", ",");
        //    string title = GetData(ref film, "\"title\":", ",");
        //    string vote_average = GetData(ref film, "\"vote_average\":", ",");
        //    string vote_count = GetData(ref film, "\"vote_count\":", "}");

        //    MovieById = new Movie(adult, backdrop_path, collection, budget, genres, homepage, id, id_imdb,
        //        original_title, overview, popularity, poster_path, production_companies, production_countries,
        //        release_date, revenue, runtime, spoken_languages, status, tagline, title, vote_average, vote_count);
        //    MovieByIdChanged.Invoke(null, null);
        //}

        /// <summary>
        /// Умный в гору не пойдёт, умный гору обойдёт.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        //string GetData(ref string doc, string begin, string end)
        //{
        //    if (doc[doc.IndexOf(begin) + begin.Length] == '\"')
        //    {
        //        //Console.WriteLine(doc[doc.IndexOf(begin) + begin.Length]);
        //        begin += '\"';
        //        end = '\"' + end;
        //    }
        //    char[] ch = new char[doc.IndexOf(end) - (doc.IndexOf(begin) + begin.Length)];
        //    doc.CopyTo(doc.IndexOf(begin) + begin.Length, ch, 0,
        //        doc.IndexOf(end) - (doc.IndexOf(begin) + begin.Length));

        //    string str = "";
        //    foreach (char c in ch)
        //    {
        //        if ((int)c != 0) str += c;
        //    }
        //    doc = doc.Remove(doc.IndexOf(begin), begin.Length + str.Length + end.Length);

        //    return str;
        //}
    }
}
