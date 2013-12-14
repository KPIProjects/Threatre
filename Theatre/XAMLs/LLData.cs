using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Theatre
{
    public class ComparatorByRating : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            return y.Rating.CompareTo(x.Rating);
        }
    }
    public class ComparatorByReleaseDate : IComparer<Movie>
    {
        public int Compare(Movie x, Movie y)
        {
            return x.ReleaseDate.CompareTo(y.ReleaseDate);
        }
    }

}
