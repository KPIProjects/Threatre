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
    public partial class Now : PhoneApplicationPage
    {
        public Now()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
            Storage.Instance.GetNowPlaying("1");
            Storage.Instance.NowPlayingChanged += UpdateViewWithData;
        }
        private Dictionary data;
        List<Header<ItemLL>> lst;
        private void UpdateViewWithData(object sender, EventArgs e)
        {
            this.data = (Dictionary)sender;
            lst = new List<Header<ItemLL>>
            {
                new Header<ItemLL>("Now")
            };
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {

                int completed = 0;
                for (int i = 0; i < data.results.Count; i++)
                {
                    GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.results[i].poster_path, i, (img, idx) =>
                    {
                        Deployment.Current.Dispatcher.BeginInvoke(() =>
                        {
                            //Create image out of bytes
                            if (img != null)
                            {
                                System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                                MemoryStream ms = new MemoryStream(img);
                                bitmapImage.SetSource(ms);

                                //Set image if you desire
                                lst[0].Add(new ItemLL()
                                {
                                    Label = data.results[idx].original_title,
                                    Description = "Release: " + data.results[idx].release_date + "\n" +
                                        "Rating: " + data.results[idx].vote_average,
                                    Number = (idx + 1).ToString(),
                                    Image = bitmapImage,
                                    index = idx,
                                    rating = data.results[idx].vote_average
                                });

                            }

                            completed++;
                            if (completed == data.results.Count)
                            {
                                lst[0].Sort(new ComparatorByRating());

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
            ItemLL selected = (ItemLL)LongList.SelectedItem;
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + data.results[selected.index].id, UriKind.Relative));
        }
    }
}