﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;
using System.Globalization;
using System.Threading;

namespace Theatre.XAMLs
{
    public partial class ChooseSession : PhoneApplicationPage
    {
        private ObservableCollection<KeyedList<string, SimpleSession>> lst = new ObservableCollection<KeyedList<string, SimpleSession>>();
        public ChooseSession()
        {
            InitializeComponent();
            LongList.ItemsSource = lst;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("movieIdx") && NavigationContext.QueryString.ContainsKey("sessionIdx"))
            {
                int movieIdx = 0;
                int.TryParse(NavigationContext.QueryString["movieIdx"].ToString(), out movieIdx);
                int sessionIdx = 0;
                int.TryParse(NavigationContext.QueryString["sessionIdx"].ToString(), out sessionIdx);
                Movie movie = Storage.Instance.NowMovies[movieIdx];
                this.session = movie.Sessions[sessionIdx];
                UpdateViewWithData(session);
            }
        }

        private Session session;
        private void UpdateViewWithData(Session session)
        {
            Name.Text = session.CinemaName;
            Phone.Text = session.CinemaPhone;
            Adress.Text = session.CinemaAdress;
            foreach (Hall hall in session.Halls)
            {
                lst.Add(new KeyedList<string, SimpleSession>(hall.Name, hall.Sessions));
                /*foreach (SimpleSession simpleSession in hall.Sessions)
                {
                    lst[0].Add(simpleSession);
                }*/
            }
            LongList.Tap += LongList_Tap;
        }


        void LongList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            SimpleSession selected = (SimpleSession)LongList.SelectedItem;
            if (selected.URL != "" && selected.URL != null)
            {
                if (selected.URL == "Phone" && session.CinemaPhone != "")
                {
                    PhoneCallTask phoneCallTask = new PhoneCallTask();

                    phoneCallTask.PhoneNumber = session.CinemaPhone;
                    phoneCallTask.DisplayName = session.CinemaName;

                    phoneCallTask.Show();
                }
                else
                {
                    WebBrowserTask webBrowserTask = new WebBrowserTask();
                    webBrowserTask.Uri = new Uri(selected.URL);
                    webBrowserTask.Show();
                }
            }
        }


        private void PhoneButton_Tap(object sender, RoutedEventArgs e)
        {
            if (session.CinemaPhone != "")
            {
                PhoneCallTask phoneCallTask = new PhoneCallTask();

                phoneCallTask.PhoneNumber = session.CinemaPhone;
                phoneCallTask.DisplayName = session.CinemaName;

                phoneCallTask.Show();
            }
        }

        private void MapButton_Tap(object sender, RoutedEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("uk");
            BingMapsTask bingMapsTask = new BingMapsTask();

            bingMapsTask.SearchTerm = session.CinemaAdress;
            bingMapsTask.ZoomLevel = 2;

            bingMapsTask.Show();
        }

    }

    public class KeyedList<TKey, TItem> : List<TItem>
    {
        public TKey Key { protected set; get; }

        public KeyedList(TKey key, IEnumerable<TItem> items)
            : base(items)
        {
            Key = key;
        }

        public KeyedList(IGrouping<TKey, TItem> grouping)
            : base(grouping)
        {
            Key = grouping.Key;
        }
    }
}