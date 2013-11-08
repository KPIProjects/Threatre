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
    public class ComparatorByRating : IComparer<ShortMovie>
    {
        public int Compare(ShortMovie x, ShortMovie y)
        {
            return y.vote_average.CompareTo(x.vote_average);
        }
    }
    public class ComparatorByReleaseDate : IComparer<ShortMovie>
    {
        public int Compare(ShortMovie x, ShortMovie y)
        {
            return x.release_date.CompareTo(y.release_date);
        }
    }

}
