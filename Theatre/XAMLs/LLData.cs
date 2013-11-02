using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Theatre
{
    public class ItemLL
    {
        public string Label { get; set; }
        public string Description { get; set; }
        public string Number { get; set; }
        public BitmapImage Image { get; set; }
        public int index { get; set; }
        public string rating { get; set; }
        public string release { get; set; }
        public string poster_path { get; set; }

    }
    public class ComparatorByRating : IComparer<ItemLL>
    {
        public int Compare(ItemLL x, ItemLL y)
        {
            return y.rating.CompareTo(x.rating);
        }
    }
    public class ComparatorByReleaseDate : IComparer<ItemLL>
    {
        public int Compare(ItemLL x, ItemLL y)
        {
            return x.release.CompareTo(y.release);
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
