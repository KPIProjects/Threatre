using System.Collections.Generic;
using System.Runtime.Serialization;
using Theatre.Storage.Session;

namespace Theatre.Storage.Movies
{
    [DataContract]
    public class MovieKinoafishaResponse
    {
        [DataMember]
        public string id;

        [DataMember]
        public string name;

        [DataMember]
        public string url;

        [DataMember]
        public string image;

        [DataMember]
        public string countries;

        [DataMember]
        public string actors;

        [DataMember]
        public string rejisser;

        [DataMember]
        public string comments_count;

        [DataMember]
        public string reviews_count;

        [DataMember]
        public string trailers_count;

        [DataMember]
        public string photos_count;

        [DataMember]
        public string is3d;

        [DataMember]
        public string is_b;

        [DataMember]
        public string b_link;
    }

    [DataContract]
    public class NowMovie : MovieKinoafishaResponse
    {
        [DataMember]
        public string vote;

        [DataMember]
        public string count_vote;

        [DataMember]
        public string imdb;

        [DataMember]
        public List<SessionResponse> sessions;

        [DataMember]
        public string s_on;

        [DataMember]
        public string ad_on;
    }

    [DataContract]
    public class UpcomingMovie : MovieKinoafishaResponse
    {
        [DataMember]
        public string sess_has;

        [DataMember]
        public string before;

        [DataMember]
        public string entered;

        [DataMember]
        public string worldwide;
    }
}
