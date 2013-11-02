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
    public partial class Soon : PhoneApplicationPage
    {
        public Soon()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
            Storage.Instance.GetUpcoming("1");
            Storage.Instance.UpcomingChanged += UpdateViewWithData;
        }
        List<Header<ShortMovie>> lst;
        private void UpdateViewWithData(object sender, EventArgs e)
        {
            Dictionary data = (Dictionary)sender;
            lst = new List<Header<ShortMovie>>
            {
                new Header<ShortMovie>("Soon")
            };
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                int completed = 0;
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
                                data.results[idx].Title = data.results[idx].title;
                                data.results[idx].Thumbnail = bitmapImage;
                                data.results[idx].ShortDescription = "Release: " + data.results[idx].release_date + "\n" +
                                                                     "Rating: " + data.results[idx].vote_average;
                                lst[0].Add(data.results[idx]);
                            }
                            completed++;
                            if (completed == data.results.Count)
                            {
                                lst[0].Sort(new ComparatorByReleaseDate());

                                LongList.ItemsSource = lst;
                                LongList.SelectionChanged += LongList_SelectionChanged;


                                ContentPanel_Content.Visibility = Visibility.Visible; //VISIBLE!
                                ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                            }
                        });
                    });
                }
            });
        }


        void LongList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShortMovie selected = (ShortMovie)LongList.SelectedItem;
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + selected.id, UriKind.Relative));
        }
    }
}