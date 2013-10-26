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
using System.Windows.Media;

namespace Theatre
{
    public partial class Top : PhoneApplicationPage
    {
        public Top()
        {
            InitializeComponent();
            ContentPanel_Content.Visibility = Visibility.Collapsed; //HIDDEN!
            Storage.Instance.GetTop("1");
            Storage.Instance.TopChanged += UpdateViewWithData;
        }
        private Dictionary data;
        private void UpdateViewWithData(object sender, EventArgs e)
        {
            this.data = (Dictionary)sender;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Movie1Label.Text = data.results[0].title;
                Movie1Description.Text = "Release: " + data.results[0].release_date + "\n" +
                                          "Rating: " + data.results[0].vote_average;

                Movie2Label.Text = data.results[1].title;
                Movie2Description.Text = "Release: " + data.results[1].release_date + "\n" +
                                          "Rating: " + data.results[1].vote_average;

                Movie3Label.Text = data.results[2].title;
                Movie3Description.Text = "Release: " + data.results[2].release_date + "\n" +
                                          "Rating: " + data.results[2].vote_average;

                Movie4Label.Text = data.results[3].title;
                Movie4Description.Text = "Release: " + data.results[3].release_date + "\n" +
                                          "Rating: " + data.results[3].vote_average;

                Movie5Label.Text = data.results[4].title;
                Movie5Description.Text = "Release: " + data.results[4].release_date + "\n" +
                                          "Rating: " + data.results[4].vote_average;

                ContentPanel_1.Tap += ContentPanel_1_Tap;
                ContentPanel_2.Tap += ContentPanel_2_Tap;
                ContentPanel_3.Tap += ContentPanel_3_Tap;
                ContentPanel_4.Tap += ContentPanel_4_Tap;
                ContentPanel_5.Tap += ContentPanel_5_Tap;

                ContentPanel_Content.Visibility = Visibility.Visible; //VISIBLE!
                ContentPanel_Loading.Visibility = Visibility.Collapsed; //HIDDEN!
                //UpdateImages();
            });

            
            GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.results[0].poster_path, img =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Create image out of bytes
                    System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    bitmapImage.SetSource(ms);

                    //Set image if you desire
                    Movie1Image.Source = bitmapImage;
                });
            });
            
            GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.results[1].poster_path, img =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Create image out of bytes
                    System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    bitmapImage.SetSource(ms);

                    //Set image if you desire
                    Movie2Image.Source = bitmapImage;
                });
            });
            GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.results[2].poster_path, img =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Create image out of bytes
                    System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    bitmapImage.SetSource(ms);

                    //Set image if you desire
                    Movie3Image.Source = bitmapImage;
                });
            });
            GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.results[3].poster_path, img =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Create image out of bytes
                    System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    bitmapImage.SetSource(ms);

                    //Set image if you desire
                    Movie4Image.Source = bitmapImage;
                });
            });
            GetImage.GetExternalImageBytes("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185" + data.results[4].poster_path, img =>
            {

                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    //Create image out of bytes
                    System.Windows.Media.Imaging.BitmapImage bitmapImage = new System.Windows.Media.Imaging.BitmapImage();
                    MemoryStream ms = new MemoryStream(img);
                    bitmapImage.SetSource(ms);

                    //Set image if you desire
                    Movie5Image.Source = bitmapImage;
                });
            });

        }

        private void ContentPanel_1_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + data.results[0].id, UriKind.Relative));
        }
        private void ContentPanel_2_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + data.results[1].id, UriKind.Relative));
        }
        private void ContentPanel_3_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + data.results[2].id, UriKind.Relative));
        }
        private void ContentPanel_4_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + data.results[3].id, UriKind.Relative));
        }
        private void ContentPanel_5_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/MoviePage.xaml?id=" + data.results[4].id, UriKind.Relative));
        }


        
    }
}