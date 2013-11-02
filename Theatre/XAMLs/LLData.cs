using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

    public class GroupingLayer<TKey, TElement> : IGrouping<TKey, TElement>
    {

        private readonly IGrouping<TKey, TElement> grouping;

        public GroupingLayer(IGrouping<TKey, TElement> unit)
        {
            grouping = unit;
        }

        public TKey Key
        {
            get { return grouping.Key; }
        }

        public IEnumerator<TElement> GetEnumerator()
        {
            return grouping.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return grouping.GetEnumerator();
        }
    }

    public class Header<T> : List<T>
    {
        public Header(string key)
        {
            this.Key = key;
        }

        public string Key { get; private set; }
    } 
}
