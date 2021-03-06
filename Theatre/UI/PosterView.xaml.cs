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

namespace Theatre.UI
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
                AppSettings.Instance.ImageManager.DownloadImage(NavigationContext.QueryString["url"].ToString(),(img) =>
                {

                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        Image.Source = img;
                        ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                    });
                });
            }
        }
    }
}