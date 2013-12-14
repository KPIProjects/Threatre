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
                /*
                // Genres //
                Genre.Text = "";
                if (data.actors.Count > 0)
                {
                    for (int i = 0; i < data.genres.Count; i++)
                    {
                        Genre.Text += data.genres[i].name;
                        if (i < data.genres.Count - 1) Genre.Text += ", ";
                    }
                }

                // Description //
                Description.Text = data.overview;

                // Release date //
                if (data.release_date != "")
                    Date.Text = "Премьера: " + data.release_date;
                else
                    Date.Text = "Премьера: N/A";

                // Companies //
                Director.Text = "";
                if (data.production_companies.Count > 0)
                {
                    for (int i = 0; i < data.production_companies.Count; i++)
                    {
                        Director.Text += data.production_companies[i].name;
                        if (i < data.production_companies.Count - 1) Director.Text += ", ";
                    }
                }

                // Countries //
                if (data.production_countries.Count > 0)
                    Country.Text = "Страна: " + data.production_countries[0].name;
                else
                    Country.Text = "Страна: N/A";

                // Budget //
                if (data.budget != "0")
                    Budget.Text = "Бюджет: " + data.budget;
                else
                    Budget.Text = "Бюджет: N/A";
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