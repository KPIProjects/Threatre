using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Theatre.Storage;
using Theatre.Storage.Movies;

namespace Theatre.UI
{
    public partial class ChooseCinema : PhoneApplicationPage
    {
        private ObservableCollection<ObservableCollection<Session>> lst = new ObservableCollection<ObservableCollection<Session>>();
        public ChooseCinema()
        {
            InitializeComponent();
            LongList.ItemsSource = lst;
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("idx") && movie == null)
            {
                int.TryParse(NavigationContext.QueryString["idx"].ToString(), out movieIdx);
                movie = DataStorage.Instance.NowMovies[movieIdx];
                UpdateViewWithData(movie);
            }
        }

        private Movie movie;
        private int movieIdx;
        private void UpdateViewWithData(Movie movie)
        {
            this.movie = movie;
            lst.Add(new ObservableCollection<Session>());
            foreach (Session session in movie.Sessions)
            {
                lst[0].Add(session);
            }
            LongList.Tap += LongList_Tap;
        }


        void LongList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Session selected = (Session)LongList.SelectedItem;
            int indx = this.movie.Sessions.IndexOf(selected);
            NavigationService.Navigate(new Uri("/UI/ChooseSession.xaml?movieIdx=" + movieIdx + "&sessionIdx=" + indx, UriKind.Relative));
        }
    }
}