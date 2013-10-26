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
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("id"))
            {
                Storage.Instance.GetMovieById(NavigationContext.QueryString["id"].ToString());
                Storage.Instance.MovieByIdChanged += UpdateViewWithData;
            }
        }

        private Movie data;
        private void UpdateViewWithData(object sender, EventArgs e)
        {
            this.data = (Movie)sender;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Original.Text = data.title;

                // Genres //
                Genre.Text = "";
                if (data.genres.Count > 0)
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
                    Date.Text = "Release: " + data.release_date;
                else
                    Date.Text = "Release: N/A";

                // Companies //

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
                    Country.Text = "Country: " + data.production_countries[0].name;
                else
                    Country.Text = "Country: N/A";

                // Budget //
                if (data.budget != "0")
                    Budget.Text = "Budget: " + data.budget;
                else
                    Budget.Text = "Budget: N/A";

                
            });

            GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.poster_path, img =>
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
            });

        }


        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/PosterView.xaml?url=" + data.poster_path, UriKind.Relative));
        }
        

//Call this as following

                
    }
}