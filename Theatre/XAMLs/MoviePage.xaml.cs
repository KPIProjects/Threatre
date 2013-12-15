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
using System.ComponentModel;

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
                int.TryParse(NavigationContext.QueryString["idx"].ToString(), out movieIdx);
                movieType = NavigationContext.QueryString["type"].ToString();
                
                Movie movie = null;
                if (movieType == "now")
                {
                    movie = Storage.Instance.NowMovies[movieIdx];
                }
                else if (movieType == "upcoming")
                {
                    movie = Storage.Instance.UpcomingMovies[movieIdx];
                }
                UpdateViewWithData(movie);
            }
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            this.movie.LengthDidUpdated -= UpdateLength;
            this.movie.DescriptionDidUpdated -= UpdateDescription;
            this.movie.ReleaseDateDidUpdated -= UpdateReleaseDate;

            base.OnBackKeyPress(e);
        }

        private Movie movie;
        private string movieType;
        private int movieIdx;
        private void UpdateViewWithData(Movie movie)
        {
            this.movie = movie;
            if (movie.Type == MovieType.Anounce)
            {
                BuyTicketButton.Visibility = Visibility.Collapsed;
            }
            this.movie.LengthDidUpdated += UpdateLength;
            this.movie.DescriptionDidUpdated += UpdateDescription;
            this.movie.ReleaseDateDidUpdated += UpdateReleaseDate;

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
                Description.Text = movie.Description;

                // Release date //
                Date.Text = "Премьера: " + movie.ReleaseDate;

                // Length
                Length.Text = movie.Length;

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

                Image.Source = movie.PosterThumbnail;
                ContentPanel_Content.Visibility = Visibility.Visible; //VISIBLE!
                ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                
            });
        }

        private void UpdateDescription(object sender, EventArgs e)
        {
            Description.Text = movie.Description;
        }

        private void UpdateReleaseDate(object sender, EventArgs e)
        {
            Date.Text = "Премьера: " + movie.ReleaseDate;
        }

        private void UpdateLength(object sender, EventArgs e)
        {
            Length.Text = movie.Length;
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/PosterView.xaml?url=" + movie.PosterFullsizeURL, UriKind.Relative));
        }

        private void BuyTicketButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/ChooseCinema.xaml?idx=" + movieIdx.ToString(), UriKind.Relative));
        }
        

//Call this as following

                
    }
}