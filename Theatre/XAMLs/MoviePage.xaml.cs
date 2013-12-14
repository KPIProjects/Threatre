using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;

namespace Theatre
{
    public partial class MoviePage : PhoneApplicationPage
    {
        public MoviePage()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("idx") && NavigationContext.QueryString.ContainsKey("type"))
            {
                int idx = 0;
                int.TryParse(NavigationContext.QueryString["idx"].ToString(), out idx);
                string type = NavigationContext.QueryString["type"].ToString();
                Movie movie = null;
                if (type == "now")
                {
                    movie = Storage.Instance.NowMovies[idx];
                }
                else if (type == "upcoming")
                {
                    movie = Storage.Instance.UpcomingMovies[idx];
                }
                UpdateViewWithData(movie);
            }
        }

        private Movie movie;
        private void UpdateViewWithData(Movie movie)
        {
            this.movie = movie;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Original.Text = movie.Title;
                
                // Genres //
                Genre.Text = "";
                for (int i = 0; i < movie.Genres.Count; i++)
                {
                    Genre.Text += movie.Genres[i];
                    if (i < movie.Genres.Count - 1) Genre.Text += ", ";
                }

                // Description //
                //Description.Text = data.overview;

                // Release date //
                if (movie.ReleaseDate != null)
                    Date.Text = "Премьера: " + movie.ReleaseDate;
                else
                {
                    if (movie.Rating != null)
                    {
                        Date.Text = "Рейтинг: " + movie.Rating;
                    }
                }

                // Companies //
                Director.Text = "";
                for (int i = 0; i < movie.Directors.Count; i++)
                {
                    Director.Text += movie.Directors.Keys.ElementAt(i);
                    if (i < movie.Directors.Count - 1) Director.Text += ", ";
                }

                // Countries //
                if (movie.Countries.Count > 0)
                    Country.Text = "Страна: " + movie.Countries[0];
                else
                    Country.Text = "Страна: N/A";
                /*
                // Budget //
                
                */
                Image.Source = movie.PosterThumbnail;
                ContentPanel_Content.Visibility = Visibility.Visible; //VISIBLE!
                ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                
            });
            /*
            GetImage.GetExternalImageBytes(movie.PosterThumbnail, 0, (img, idx) =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Create image out of bytes
                    System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    bitmapImage.SetSource(ms);

                    //Set image if you desire
                    Image.Source = bitmapImage;
                    Image.Tap += Image_Tap;
                });
            });*/

        }


        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/PosterView.xaml?url=" + movie.PosterFullsizeURL, UriKind.Relative));
        }
        

//Call this as following

                
    }
}