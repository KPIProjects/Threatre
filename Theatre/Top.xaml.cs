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
            ;
        }
    }
}