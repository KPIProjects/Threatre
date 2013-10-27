using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Theatre
{
    [DataContract]
    public class ShortMovie
    {
        [DataMember]
        public string adult;

        [DataMember]
        public string backdrop_path;

        [DataMember]
        public string id;

        [DataMember]
        public string original_title;

        [DataMember]
        public string release_date;

        [DataMember]
        public string popularity;

        [DataMember]
        public string poster_path;

        [DataMember]
        public string title;

        [DataMember]
        public string vote_average;

        [DataMember]
        public string vote_count;
    }

    [DataContract]
    public class Movie: ShortMovie
    {
        [DataMember]
        public string collection;

        [DataMember]
        public string budget;

        [DataMember]
        public List<Item> genres;

        [DataMember]
        public string homepage;

        [DataMember]
        public string imdb_id;

        [DataMember]
        public string overview;

        [DataMember]
        public List<Item> production_companies;

        [DataMember]
        public List<Item> production_countries;

        [DataMember]
        public string revenue;

        [DataMember]
        public string runtime;

        [DataMember]
        public List<Item> spoken_languages;

        [DataMember]
        public string status;

        [DataMember]
        public string tagline;
    }
}
