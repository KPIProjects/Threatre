using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

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
        private void UpdateViewWithData(object sender, EventArgs e)
        {
            Movie data = (Movie)sender;

            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Original.Text = data.title;
                Genre.Text = "";
                for (int i=0;i<data.genres.Count;i++)
                {
                    Genre.Text += data.genres[i].name;
                    if (i < data.genres.Count - 1) Genre.Text += ", ";
                }
                Description.Text = data.overview;
                Date.Text = "Release: " + data.release_date;
                for (int i = 0; i < data.production_companies.Count; i++)
                {
                    Director.Text += data.production_companies[i].name;
                    if (i < data.production_companies.Count - 1) Director.Text += ", ";
                }
                for (int i = 0; i < data.production_countries.Count; i++)
                {
                    Country.Text += data.production_countries[i].name;
                    if (i < data.production_countries.Count - 1) Country.Text += ", ";
                }
                Budget.Text = "Budget: " + data.budget;
            });
        }
    }
}