using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Theatre.Storage.Movies;

namespace Theatre
{
    public class ImageManager
    {
        public void MovieLogoThumbnail(Movie movie, int index, Action<byte[], int> callback)
        {
            DownloadMovieLogoThumbnail(movie, index, callback);
        }

        private void DownloadMovieLogoThumbnail(Movie movie, int index, Action<byte[], int> callback)
        {
            DownloadImage(movie.PosterThumbnailURL, (img) =>
            {
                callback(img, index);
            });
        }

        public void DownloadImage(string url, Action<byte[]> callback)
        {
            byte[] data;
            HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);

            myReq.BeginGetResponse(result =>
            {
                try
                {
                    WebResponse response = myReq.EndGetResponse(result);

                    using (var stream = response.GetResponseStream())
                    {
                        using (BinaryReader br = new BinaryReader(stream))
                        {
                            data = br.ReadBytes(5000000);
                            br.Close();
                        }
                    }
                    response.Close();

                    callback(data);
                }
                catch
                {
                    callback(null);
                }
            }, null);
        }
    }
}
