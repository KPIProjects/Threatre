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
using System.Windows.Media;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace Theatre
{
    public partial class Top : PhoneApplicationPage
    {
        private ObservableCollection<ObservableCollection<ShortMovie>> lst = new ObservableCollection<ObservableCollection<ShortMovie>>();
        private int visiblePages = 1;
        private bool canAddImages = false;

        public Top()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
            LongList.ItemsSource = lst;
            Storage.Instance.GetTop("1", UpdateViewWithData);
        }

        private void UpdateViewWithData(Dictionary data)
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                int completed = 0;
                List<ShortMovie> newMovies = new List<ShortMovie>();
                lst.Add(new ObservableCollection<ShortMovie>());

                for (int i = 0; i < data.results.Count; i++)
                {
                    GetImage.ThumbnailImageForMovieDBWithPath(data.results[i].poster_path, i, (img, idx) =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            if (img != null)
                            {
                                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                                MemoryStream ms = new MemoryStream(img);
                                bitmapImage.SetSource(ms);

                                //Set image if you desire
                                data.results[idx].Thumbnail = bitmapImage;
                                newMovies.Add(data.results[idx]);
                            }
                            completed++;
                            if (completed == data.results.Count)
                            {
                                newMovies.Sort(new ComparatorByRating());

                                foreach (ShortMovie movie in newMovies)
                                {
                                    lst[visiblePages - 1].Add(movie);
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
            ShortMovie selected = (ShortMovie)LongList.SelectedItem;
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + selected.id, UriKind.Relative));
        }
        
        void LongList_Link(object sender, LinkUnlinkEventArgs e)
        {
            if (canAddImages)
            {
                ShortMovie item = (ShortMovie)e.ContentPresenter.Content;
                if (item.id == lst[visiblePages - 1].Last().id)
                {
                    canAddImages = false;
                    visiblePages++;
                    Storage.Instance.GetTop(visiblePages.ToString(), UpdateViewWithData);
                }
            }
        }


    }
}