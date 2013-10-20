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
    public partial class Page1 : PhoneApplicationPage
    {
        public Page1()
        {
            InitializeComponent();
            Storage.Instance.GetTop("1");
            Storage.Instance.TopChanged += UpdateViewWithData;
        }

        private void UpdateViewWithData(object sender, EventArgs e)
        {
            Dictionary data = Storage.Instance.Top;
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                Movie1Label.Text = data.Movies[0].Title + " (" + data.Movies[0].Release_date + ")";
                Movie1Description.Text = "Rating: " + data.Movies[0].Vote_average;

                Movie2Label.Text = data.Movies[1].Title + " (" + data.Movies[1].Release_date + ")";
                Movie2Description.Text = "Rating: " + data.Movies[1].Vote_average;

                Movie3Label.Text = data.Movies[2].Title + " (" + data.Movies[2].Release_date + ")";
                Movie3Description.Text = "Rating: " + data.Movies[2].Vote_average;

                Movie4Label.Text = data.Movies[3].Title + " (" + data.Movies[3].Release_date + ")";
                Movie4Description.Text = "Rating: " + data.Movies[3].Vote_average;

                Movie5Label.Text = data.Movies[4].Title + " (" + data.Movies[4].Release_date + ")";
                Movie5Description.Text = "Rating: " + data.Movies[4].Vote_average;

                //UpdateImages();
            });
        }

        private void UpdateImages()
        {
            Dictionary data = Storage.Instance.Top;

            var request = WebRequest.CreateHttp("http://d3gtl9l2a4fn1j.cloudfront.net/t/p/w185"+data.Movies[0].Poster_path);
            request.Method = "GET";
            //request.KeepAlive = false; 
            request.BeginGetResponse(result =>
            {
                HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
                Stream streamResponse = response.GetResponseStream();
                BinaryReader streamRead = new BinaryReader(streamResponse);

                FileStream writeStream = new FileStream("moviePicture1.png", FileMode.Create);
                BinaryWriter writeBinary = new BinaryWriter(writeStream);

                for (int i = 0; i < streamResponse.Length; i++)
                {
                    byte b = streamRead.ReadByte();
                    writeBinary.Write(b);
                }

                streamResponse.Close();
                streamRead.Close();
                writeBinary.Close();
                /*Deployment.Current.Dispatcher.BeginInvoke(() =>
                {*/
                Movie1Image.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("moviePicture1.png", UriKind.RelativeOrAbsolute));
               /* });*/
            }, null);
        }
    }
}