using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace Theatre
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Конструктор
        public MainPage()
        {
            InitializeComponent();
        }

        private void NowButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/Now.xaml", UriKind.Relative));
        }

        private void SoonButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/XAMLs/Soon.xaml", UriKind.Relative));
        }
    }
}