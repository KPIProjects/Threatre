﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO;
using System.Collections.ObjectModel;
using Theatre.Storage;
using Theatre.Storage.Movies;

namespace Theatre.UI
{
    public partial class Soon : PhoneApplicationPage
    {
        private ObservableCollection<ObservableCollection<Movie>> lst = new ObservableCollection<ObservableCollection<Movie>>();
        private int visiblePages = 1;
        private bool canAddImages = false;

        public Soon()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
            LongList.ItemsSource = lst;
            DataStorage.Instance.GetUpcoming(1, UpdateViewWithData);
        }

        private void UpdateViewWithData(List<Movie> data)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                int completed = (visiblePages - 1) * 10;
                List<Movie> newMovies = new List<Movie>();
                lst.Add(new ObservableCollection<Movie>());

                for (int i = (visiblePages-1) * 10; i < data.Count; i++)
                {
                    GetImage.GetExternalImageBytes(data[i].PosterThumbnailURL, i, (img, idx) =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            //Create image out of bytes
                            if (img != null)
                            {
                                data[idx].PosterThumbnail = new System.Windows.Media.Imaging.BitmapImage();
                                MemoryStream ms = new MemoryStream(img);
                                data[idx].PosterThumbnail.SetSource(ms);
                                newMovies.Add(data[idx]);
                            }

                            completed++;
                            if (completed == data.Count)
                            {
                                newMovies.Sort(new ComparatorByReleaseDate());

                                for (int j = completed-10; j < completed; j++)
                                {
                                    lst[visiblePages - 1].Add(data[j]);
                                }

                                LongList.Link += LongList_Link;
                                LongList.Tap += LongList_Tap;
                                canAddImages = true;

                                ContentPanel_Content.Visibility = Visibility.Visible; //VISIBLE!
                                ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                            }
                        });
                    });
                }
            });
        }

        void LongList_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            Movie selected = (Movie)LongList.SelectedItem;
            int indx = DataStorage.Instance.UpcomingMovies.IndexOf(selected);
            NavigationService.Navigate(new Uri("/UI/MoviePage.xaml?idx=" + indx + "&type=upcoming", UriKind.Relative));
        }

        void LongList_Link(object sender, LinkUnlinkEventArgs e)
        {
            if (canAddImages)
            {
                Movie item = (Movie)e.ContentPresenter.Content;
                if (item.ID == lst[visiblePages - 1].Last().ID)
                {
                    canAddImages = false;
                    visiblePages++;
                    DataStorage.Instance.GetUpcoming(visiblePages, UpdateViewWithData);
                }
            }
        }
    }
}