using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using Theatre.Storage.Movies;

namespace Theatre
{
    public class ImageManager
    {
        /* Thumbnail */
        public void MovieLogoThumbnail(Movie movie, int index, Action<BitmapImage, int> callback)
        {
            if (IsMovieLogoInLocalStorage(movie))
            {
                UseLocalMovieLogoThumbnail(movie, index, callback);
            }
            else
            {
                DownloadMovieLogoThumbnail(movie, index, callback);
            }
        }

        private string LocalMovieThumbnailPrefix = "movie_";
        private string LocalMovieThumbnailPostfix = "_poster_small.jpg";

        private bool IsMovieLogoInLocalStorage(Movie movie)
        {
            String fileName = LocalMovieThumbnailPrefix + movie.ID + LocalMovieThumbnailPostfix;
            return isFileInLocalStorage(fileName);
        }

        private void UseLocalMovieLogoThumbnail(Movie movie, int index, Action<BitmapImage, int> callback)
        {
            String fileName = LocalMovieThumbnailPrefix + movie.ID + LocalMovieThumbnailPostfix;
            LoadLocalFileForMovie(fileName, index, callback);
        }

        private void CreateLocalImageForMovie(Movie movie, BitmapImage img)
        {
            String fileName = LocalMovieThumbnailPrefix + movie.ID + LocalMovieThumbnailPostfix;
            CreateLocalImageWithName(fileName, img);
        }

        private void DownloadMovieLogoThumbnail(Movie movie, int index, Action<BitmapImage, int> callback)
        {
            DownloadImage(movie.PosterThumbnailURL, (img) =>
            {
                CreateLocalImageForMovie(movie, img);
                callback(img, index);
            });
        }


        /* Screens */
        public void GetScreenWithURL(string url, int index, Action<BitmapImage, int> callback)
        {
            if (IsScreenInLocalStorage(url))
            {
                UseLocalScreen(url, index, callback);
            }
            else
            {
                DownloadScreen(url, index, callback);
            }
        }

        private bool IsScreenInLocalStorage(string imageLink)
        {
            String fileName = imageLink.Replace("http://kinoafisha.ua/", "").Replace("/", ".");
            return isFileInLocalStorage(fileName);
        }

        private void UseLocalScreen(string imageLink, int index, Action<BitmapImage, int> callback)
        {
            String fileName = imageLink.Replace("http://kinoafisha.ua/", "").Replace("/", ".");
            LoadLocalFileForMovie(fileName, index, callback);
        }

        private void BackupScreen(string imageLink, BitmapImage img)
        {
            String fileName = imageLink.Replace("http://kinoafisha.ua/", "").Replace("/", ".");
            CreateLocalImageWithName(fileName, img);
        }

        private void DownloadScreen(string imageLink, int index, Action<BitmapImage, int> callback)
        {
            DownloadImage(imageLink, (img) =>
            {
                BackupScreen(imageLink, img);
                callback(img, index);
            });
        }

        /* Abstract operations */
        private void LoadLocalFileForMovie(string fileName, int index, Action<BitmapImage, int> callback)
        {
            BitmapImage img = new BitmapImage();

            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream fileStream = myIsolatedStorage.OpenFile(fileName, FileMode.Open, FileAccess.Read))
                {
                    img.SetSource(fileStream);
                    callback(img, index);
                }
            }
        }

        private bool isFileInLocalStorage(string fileName)
        {
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return myIsolatedStorage.FileExists(fileName);
            }
        }

        private void CreateLocalImageWithName(string fileName, BitmapImage img)
        {
            // Create a filename for JPEG file in isolated storage.

            // Create virtual store and file stream. Check for duplicate tempJPEG files.
            using (IsolatedStorageFile myIsolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                IsolatedStorageFileStream fileStream = myIsolatedStorage.CreateFile(fileName);

                StreamResourceInfo sri = null;
                Uri uri = new Uri(fileName, UriKind.Relative);
                sri = Application.GetResourceStream(uri);

                WriteableBitmap wb = new WriteableBitmap(img);

                // Encode WriteableBitmap object to a JPEG stream.
                Extensions.SaveJpeg(wb, fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);

                //wb.SaveJpeg(fileStream, wb.PixelWidth, wb.PixelHeight, 0, 85);
                fileStream.Close();
            }
        }


        public void DownloadImage(string url, Action<BitmapImage> callback)
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
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        //Create image out of bytes
                        System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                        MemoryStream ms = new MemoryStream(data);
                        bitmapImage.SetSource(ms);
                        callback(bitmapImage);
                    });
                    
                }
                catch
                {
                    callback(null);
                }
            }, null);
        }
    }
}
