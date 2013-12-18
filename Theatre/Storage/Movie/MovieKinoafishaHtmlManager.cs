using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Theatre.Storage.Movies
{
    class MovieKinoafishaHtmlManager
    {
        public static void DownloadHTMLDetailsForMovie(Movie movie, Action<Movie> callback)
        {
            if (movie.Description == null)
            {
                WebRequest request = WebRequest.CreateHttp(movie.Url);
                request.Method = "GET";
                request.BeginGetResponse(result =>
                {
                    HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                    Stream streamResponse = response.GetResponseStream();
                    StreamReader streamRead = new StreamReader(streamResponse);
                    String responseContent = streamRead.ReadToEnd();
                    ParseMovieHTMLPage(movie, responseContent);
                    callback(movie);
                }, null);
            }
        }

        private static void ParseMovieHTMLPage(Movie movie, string html)
        {
            string clearHTML = html.Replace("\n\t\t\t\t", "").
                                    Replace("\t\t\t", "").
                                    Replace("<p><span class=\"_reachbanner_\">", "").
                                    Replace("<span class=\"hps\">", "").
                                    Replace("<span id=\"result_box\" lang=\"ru\">", "").
                                    Replace("&hellip;", "...").
                                    Replace("&ndash;", " - ").
                                    Replace("&mdash;", "-").
                                    Replace("&amp;", "&").
                                    Replace("&lt;;", "<").
                                    Replace("&gt;", ">").
                                    Replace("&laquo;", "«").
                                    Replace("&raquo;", "»").
                                    Replace("&amp;", "&").
                                    Replace("&nbsp;", " ");
            ParseDescriptionFromHTML(movie, clearHTML);
            ParseLengthFromHTML(movie, clearHTML);
            ParseImagesFromHtml(movie, clearHTML);
            ParseTrailerFromHtml(movie, clearHTML);
            if (movie.Type == MovieType.InCinema)
            {
                ParseReleaseDateFromHTML(movie, clearHTML);
            }

        }

        private static void ParseDescriptionFromHTML(Movie movie, string html)
        {
            string parseDescription = html.Replace("<div class=\"description\" itemprop=\"description\">", "\0").
                                           Replace("</div> <!-- end description -->", "\0").
                                           Replace("<p>", "").
                                           Replace("</p>", "").
                                           Replace("<br />", "\n").
                                           Replace("</span>", "").
                                           Replace("<span>", "").
                                           Split('\0')[1];

            // Clear of span class tag //
            int idx = -1;
            do
            {
                idx = parseDescription.IndexOf("<span class=\"");
                if (idx != -1)
                {
                    int lastidx = parseDescription.IndexOf("\">");
                    parseDescription = parseDescription.Remove(idx, lastidx - idx + 2);
                }
            }
            while (idx != -1);

            // Clear of span title tag //
            idx = -1;
            do
            {
                idx = parseDescription.IndexOf("<span title=\"");
                if (idx != -1)
                {
                    int lastidx = parseDescription.IndexOf("\">");
                    parseDescription = parseDescription.Remove(idx, lastidx - idx + 2);
                }
            }
            while (idx != -1);
            movie.Description = parseDescription;
            movie.DescriptionDidLoad();
        }

        private static void ParseLengthFromHTML(Movie movie, string html)
        {
            if (html.IndexOf("<p>Продолжительность: <span>") == -1)
            {
                movie.Length = "N/A";
                return;
            }
            string parsedLength = html.Replace("<p>Продолжительность: <span>", "\0").
                                      Replace("</span><span itemprop=\"duration\"", "\0").Split('\0')[1];
            movie.Length = parsedLength;
            movie.LengthDidLoad();
        }

        private static void ParseReleaseDateFromHTML(Movie movie, string html)
        {
            if (html.IndexOf("<p>Премьера в Украине: <span>") == -1)
            {
                movie.ReleaseDate = "N/A";
                return;
            }
            string parsedDate = html.Replace("<p>Премьера в Украине: <span>", "\0").
                                     Split('\0')[1].
                                     Replace("</span></p>", "\0").Split('\0')[0];
            movie.ReleaseDate = parsedDate;
            movie.ReleaseDateDidLoad();
        }

        private static void ParseImagesFromHtml(Movie movie, string html)
        {
            movie.MovieImageURLs = new List<string>();
            string parseHtml = html.Replace("<div class=\"shots-holder\">", "\0").Split('\0')[1].
                Replace("<div class=\"no-photos\" style=\"display:none\">", "\0").Split('\0')[0];
            string[] items = parseHtml.Replace("<a class=\"item", "\0").Split('\0');
            foreach (string item in items)
            {
                string[] parts = item.Split('"');
                if (parts.Length >= 2)
                {
                    movie.MovieImageURLs.Add("http://kinoafisha.ua" + parts[2]);
                }
            }
        }

        private static void ParseTrailerFromHtml(Movie movie, string html)
        {
            if (movie.TrailersCount != "0")
            {
                string trailerLink = html.Replace("\"kino-player\" width=\"600\" height=\"350\" src=\"", "\0").Split('\0')[1].Replace("\" frameborder=\"0", "\0").Split('\0')[0];
                movie.TrailerURL = trailerLink;
            }
        }
    }
}
