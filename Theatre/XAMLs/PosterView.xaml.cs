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
    public partial class PosterView : PhoneApplicationPage
    {
        public PosterView()
        {
            InitializeComponent();
        }
        
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (NavigationContext.QueryString.ContainsKey("url"))
            {
                GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w780" + NavigationContext.QueryString["url"].ToString(),0, (img,idx) =>
                {

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        //Create image out of bytes
                        System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                        MemoryStream ms = new MemoryStream(img);
                        bitmapImage.SetSource(ms);

                        //Set image if you desire
                        Image.Source = bitmapImage;
                        ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                    });
                });
            }
        }
    }
}