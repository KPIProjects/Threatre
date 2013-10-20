using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Theatre
{
    public partial class Movies : PhoneApplicationPage
    {
        public Movies()
        {
            InitializeComponent();
            Storage.Instance.GetNowPlaying("1");
            Storage.Instance.NowPlayingChanged += UpdateViewWithData;
        }

        private void UpdateViewWithData(object sender, EventArgs e)
        {
            ;
        }
    }
}