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

namespace Theatre
{
    public partial class Top : PhoneApplicationPage
    {
        public Top()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
            Storage.Instance.GetTop("1", UpdateViewWithData);
        }
        List<Header<ShortMovie>> lst;
        private void UpdateViewWithData(Dictionary data)
        {
            lst = new List<Header<ShortMovie>>
            {
                new Header<ShortMovie>("Top")
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
                                data.results[idx].Thumbnail = bitmapImage;
                                lst[0].Add(data.results[idx]);
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
            ShortMovie selected = (ShortMovie)LongList.SelectedItem;
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + selected.id, UriKind.Relative));
        }
    }
}