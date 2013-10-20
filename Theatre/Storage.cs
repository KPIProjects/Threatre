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

namespace Theatre
{
    /// <summary>
    /// Класс, который содержит в себе список фильмов на странице, номер страницы, 
    /// количество страниц и количество фильмов.
    /// </summary>
    public class Dictionary
    {
        public string Page;
        public string Total_pages;
        public string Total_results;
        public List<Movie> Movies;

        public Dictionary() { }
        public Dictionary(List<Movie> movies, string page, string total_pages, string total_results)
        {
            Movies = movies;
            Page = page;
            Total_pages = total_pages;
            Total_results = total_results;
        }
    }

    public class Item
    {
        string _id;
        string _name;

        public string Id
        {
            set { _id = value; }
            get { return _id; }
        }
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        public Item() { }
        public Item(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Show()
        {
            Console.Write(Name);
        }
    }

    public class Movie
    {
        public string Adult;
        public string Backdrop_path;
        public string Collection;
        public string Budget;
        public List<Item> Genres;
        public string Homepage;
        public string Id;
        public string Id_imdb;
        public string Original_title;
        public string Overview;
        public string Popularity;
        public string Poster_path;
        public List<Item> Production_companies;
        public List<Item> Production_countries;
        public string Release_date;
        public string Revenue;
        public string Runtime;
        public List<Item> Spoken_languages;
        public string Status;
        public string Tagline;
        public string Title;
        public string Vote_average;
        public string Vote_count;

        public Movie() { }

        public Movie(string adult = null, string backdrop_path = null, string collection = null,
            string budget = null, List<Item> genres = null, string homepage = null, string id = null,
            string id_imdb = null, string original_title = null, string overview = null, string popularity = null,
            string poster_path = null, List<Item> production_companies = null, List<Item> production_countries = null,
            string release_date = null, string revenue = null, string runtime = null,
            List<Item> spoken_languages = null, string status = null, string tagline = null, string title = null,
            string vote_average = null, string vote_count = null)
        {
            Adult = adult;
            Backdrop_path = backdrop_path;
            Collection = collection;
            Budget = budget;
            Genres = genres;
            Homepage = homepage;
            Id = id;
            Id_imdb = id_imdb;
            Original_title = original_title;
            Overview = overview;
            Popularity = popularity;
            Poster_path = poster_path;
            Production_companies = production_companies;
            Production_countries = production_countries;
            Release_date = release_date;
            Revenue = revenue;
            Runtime = runtime;
            Spoken_languages = spoken_languages;
            Status = status;
            Tagline = tagline;
            Title = title;
            Vote_average = vote_average;
            Vote_count = vote_count;
        }

        public void Show()
        {
            if (Title != null) Console.WriteLine("Title = {0}", Title);
            if (Backdrop_path != null) Console.WriteLine("Backdrop_path = {0}", Backdrop_path);
            if (Collection != null) Console.WriteLine("Collection = {0}", Collection);
            if (Budget != null) Console.WriteLine("Budget = {0}", Budget);
            if (Genres != null)
            {
                Console.Write("Genres: ");
                for(int i=0; i<Genres.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)Genres[i]).Show();
                }
                Console.WriteLine();
            }
            if (Homepage != null) Console.WriteLine("Homepage = {0}", Homepage);
            if (Id != null) Console.WriteLine("Id = {0}", Id);
            if (Id_imdb != null) Console.WriteLine("Id_imdb = {0}", Id_imdb);
            if (Original_title != null) Console.WriteLine("Original_title = {0}", Original_title);
            if (Overview != null) Console.WriteLine("Overview = {0}", Overview);
            if (Popularity != null) Console.WriteLine("Popularity = {0}", Popularity);
            if (Poster_path != null) Console.WriteLine("Poster_path = {0}", Poster_path);
            if (Production_companies != null)
            {
                Console.Write("Production_companies: ");
                for (int i = 0; i < Production_companies.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)Production_companies[i]).Show();
                }
                Console.WriteLine();
            }
            if (Production_countries != null)
            {
                Console.Write("Production_countries: ");
                for (int i = 0; i < Production_countries.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)Production_countries[i]).Show();
                }
                Console.WriteLine();
            }
            if (Release_date != null) Console.WriteLine("Release_date = {0}", Release_date);
            if (Revenue != null) Console.WriteLine("Revenue = {0}", Revenue);
            if (Runtime != null) Console.WriteLine("Runtime = {0}", Runtime);
            if (Spoken_languages != null)
            {
                Console.Write("Spoken_languages: ");
                for (int i = 0; i < Spoken_languages.Count; i++)
                {
                    if (i != 0) Console.Write(", ");
                    ((Item)Spoken_languages[i]).Show();
                }
                Console.WriteLine();
            }
            if (Status != null) Console.WriteLine("Status = {0}", Status);
            if (Tagline != null) Console.WriteLine("Tagline = {0}", Tagline);
            if (Title != null) Console.WriteLine("Title = {0}", Title);
            if (Vote_average != null) Console.WriteLine("Vote_average = {0}", Vote_average);
            if (Vote_count != null) Console.WriteLine("Vote_count = {0}", Vote_count);
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
                GenerateTop(responseContent, onpage);
            }, null);
        }
        public Dictionary Top;
        public event EventHandler TopChanged;
        private void GenerateTop(string response, string onpage)
        {
            List<Movie> Movies = new List<Movie>();
            string str = response;

            string page = GetData(ref str, "{\"page\":", ",\"results\":");
            while (str[1] != ']')
            {
                string adult = GetData(ref str, "{\"adult\":", ",");
                string backdrop_path = GetData(ref str, "\"backdrop_path\":", ",");
                string id = GetData(ref str, "\"id\":", ",");
                string original_title = GetData(ref str, "\"original_title\":", ",");
                string release_date = GetData(ref str, "\"release_date\":", ",");
                string poster_path = GetData(ref str, "\"poster_path\":", ",");
                string popularity = GetData(ref str, "\"popularity\":", ",");
                string title = GetData(ref str, "\"title\":", ",");
                string vote_average = GetData(ref str, "\"vote_average\":", ",");
                string vote_count = GetData(ref str, "\"vote_count\":", "}");
                if (str[1] == ',') str = str.Remove(1, 1);
                Movies.Add(new Movie(adult, backdrop_path, null, null, null, null, id, null, original_title, null,
                    popularity, poster_path, null, null, release_date, null, null, null, null, null, title,
                    vote_average, vote_count));
            }
            GetData(ref str, "[", "],");
            string total_pages = GetData(ref str, "\"total_pages\":", ",");
            string total_results = GetData(ref str, "\"total_results\":", "}");

            Top = new Dictionary(Movies, page, total_pages, total_results);
            TopChanged.Invoke(null, null);
        }

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
                GenerateUpcoming(responseContent, onpage);
            }, null);
        }
        public Dictionary Upcoming;
        public event EventHandler UpcomingChanged;
        private void GenerateUpcoming(string response, string onpage)
        {
            string str = response;
            List<Movie> Movies = new List<Movie>();

            GetData(ref str, "{\"dates\"", ":");
            string min_date = GetData(ref str, "{\"minimum\":", ",");
            string max_date = GetData(ref str, "\"maximum\":", "},");

            string page = GetData(ref str, "\"page\":", ",\"results\":");
            while (str[1] != ']')
            {
                string adult = GetData(ref str, "{\"adult\":", ",");
                string backdrop_path = GetData(ref str, "\"backdrop_path\":", ",");
                string id = GetData(ref str, "\"id\":", ",");
                string original_title = GetData(ref str, "\"original_title\":", ",");
                string release_date = GetData(ref str, "\"release_date\":", ",");
                string poster_path = GetData(ref str, "\"poster_path\":", ",");
                string popularity = GetData(ref str, "\"popularity\":", ",");
                string title = GetData(ref str, "\"title\":", ",");
                string vote_average = GetData(ref str, "\"vote_average\":", ",");
                string vote_count = GetData(ref str, "\"vote_count\":", "}");
                if (str[1] == ',') str = str.Remove(1, 1);
                Movies.Add(new Movie(adult, backdrop_path, null, null, null, null, id, null, original_title, null,
                    popularity, poster_path, null, null, release_date, null, null, null, null, null, title,
                    vote_average, vote_count));
            }
            GetData(ref str, "[", "],");
            string total_pages = GetData(ref str, "\"total_pages\":", ",");
            string total_results = GetData(ref str, "\"total_results\":", "}");

            Upcoming = new Dictionary(Movies, page, total_pages, total_results);
            UpcomingChanged.Invoke(null, null);

        }

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
                GenerateNowPlaying(responseContent, onpage);
            }, null);
        }
        public Dictionary NowPlaying;
        public event EventHandler NowPlayingChanged;
        private void GenerateNowPlaying (string response, string onpage)
        {
            string str = response;

            List<Movie> Movies = new List<Movie>();

            GetData(ref str, "{\"dates\"", ":");
            string min_date = GetData(ref str, "{\"minimum\":", ",");
            string max_date = GetData(ref str, "\"maximum\":", "},");

            string page = GetData(ref str, "\"page\":", ",\"results\":");
            while (str[1] != ']')
            {
                string adult = GetData(ref str, "{\"adult\":", ",");
                string backdrop_path = GetData(ref str, "\"backdrop_path\":", ",");
                string id = GetData(ref str, "\"id\":", ",");
                string original_title = GetData(ref str, "\"original_title\":", ",");
                string release_date = GetData(ref str, "\"release_date\":", ",");
                string poster_path = GetData(ref str, "\"poster_path\":", ",");
                string popularity = GetData(ref str, "\"popularity\":", ",");
                string title = GetData(ref str, "\"title\":", ",");
                string vote_average = GetData(ref str, "\"vote_average\":", ",");
                string vote_count = GetData(ref str, "\"vote_count\":", "}");
                if (str[1] == ',') str = str.Remove(1, 1);
                Movies.Add(new Movie(adult, backdrop_path, null, null, null, null, id, null, original_title, null,
                    popularity, poster_path, null, null, release_date, null, null, null, null, null, title,
                    vote_average, vote_count));
            }
            GetData(ref str, "[", "],");
            string total_pages = GetData(ref str, "\"total_pages\":", ",");
            string total_results = GetData(ref str, "\"total_results\":", "}");

            NowPlaying = new Dictionary(Movies, page, total_pages, total_results);
            NowPlayingChanged.Invoke(null, null);
        }

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
                GenerateMovieById(responseContent, mov_id);
            }, null);
        }
        public Movie MovieById;
        public event EventHandler MovieByIdChanged;
        private void GenerateMovieById(string responseContent,string mov_id)
        {
            string film = responseContent;
            string adult = GetData(ref film, "{\"adult\":",",");
            string backdrop_path = GetData(ref film, "\"backdrop_path\":",",");
            string collection = GetData(ref film, "\"belongs_to_collection\":",",");
            string budget = GetData(ref film, "\"budget\":",",");
            List<Item> genres = new List<Item>();
            GetData(ref film, "\"genres\"", ":");
            while (film[1] != ']')
            {
                genres.Add(new Item(GetData(ref film, "{\"id\":", ","), GetData(ref film, "\"name\":", "}")));
                if (film[1] == ',') film = film.Remove(1, 1);
            }
            GetData(ref film, "[", "],");
            string homepage = GetData(ref film, "\"homepage\":", ",");
            string id = GetData(ref film, "\"id\":", ",");
            string id_imdb = GetData(ref film, "\"imdb_id\":", ",");
            string original_title = GetData(ref film, "\"original_title\":", ",");
            string overview = GetData(ref film, "\"overview\":", ",");
            string popularity = GetData(ref film, "\"popularity\":", ",");
            string poster_path = GetData(ref film, "\"poster_path\":", ",");
            List<Item> production_companies = new List<Item>();
            GetData(ref film, "\"production_companies\"", ":");
            while (film[1] != ']')
            {
                production_companies.Add(new Item(GetData(ref film, "\"id\":", "}"), GetData(ref film, "{\"name\":", ",")));
                if (film[1] == ',') film = film.Remove(1, 1);
            }
            GetData(ref film, "[", "],");
            List<Item> production_countries = new List<Item>();
            GetData(ref film, "\"production_countries\"", ":");
            while (film[1] != ']')
            {
                production_countries.Add(new Item(GetData(ref film, "{\"iso_3166_1\":\"", "\","), GetData(ref film, "\"name\":", "}")));
                if (film[1] == ',') film = film.Remove(1, 1);
            }
            GetData(ref film, "[", "],");
            string release_date = GetData(ref film, "\"release_date\":", ",");
            string revenue = GetData(ref film, "\"revenue\":", ",");
            string runtime = GetData(ref film, "\"runtime\":", ",");
            List<Item> spoken_languages = new List<Item>();
            GetData(ref film, "\"spoken_languages\"", ":");
            while (film[1] != ']')
            {
                spoken_languages.Add(new Item(GetData(ref film, "{\"iso_639_1\":", ","), GetData(ref film, "\"name\":", "}")));
                if (film[1] == ',') film = film.Remove(1, 1);
            }
            GetData(ref film, "[", "],");
            string status = GetData(ref film, "\"status\":", ",");
            string tagline = GetData(ref film, "\"tagline\":", ",");
            string title = GetData(ref film, "\"title\":", ",");
            string vote_average = GetData(ref film, "\"vote_average\":", ",");
            string vote_count = GetData(ref film, "\"vote_count\":", "}");

            MovieById = new Movie(adult, backdrop_path, collection, budget, genres, homepage, id, id_imdb,
                original_title, overview, popularity, poster_path, production_companies, production_countries,
                release_date, revenue, runtime, spoken_languages, status, tagline, title, vote_average, vote_count);
            MovieByIdChanged.Invoke(null,null);
        }

        /// <summary>
        /// Умный в гору не пойдёт, умный гору обойдёт.
        /// </summary>
        /// <param name="doc"></param>
        /// <param name="begin"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        string GetData(ref string doc, string begin, string end)
        {
            if (doc[doc.IndexOf(begin) + begin.Length] == '\"') 
            {
                //Console.WriteLine(doc[doc.IndexOf(begin) + begin.Length]);
                begin += '\"'; 
                end = '\"' + end; 
            }
            char[] ch = new char[doc.IndexOf(end) - (doc.IndexOf(begin) + begin.Length)];
            doc.CopyTo(doc.IndexOf(begin) + begin.Length, ch, 0,
                doc.IndexOf(end) - (doc.IndexOf(begin) + begin.Length));

            string str = "";
            foreach (char c in ch)
            {
                if ((int)c != 0) str += c;
            }
            doc = doc.Remove(doc.IndexOf(begin), begin.Length + str.Length + end.Length);

            return str;
        }
    }
}
