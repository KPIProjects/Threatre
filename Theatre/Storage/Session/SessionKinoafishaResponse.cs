using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Theatre.Storage.Session
{
    [DataContract]
    public class SessionResponse
    {
        [DataMember]
        public string k_id;

        [DataMember]
        public string k_name;

        [DataMember]
        public string k_price;

        [DataMember]
        public string k_url;

        [DataMember]
        public string k_type;

        [DataMember]
        public string k_bron;

        [DataMember]
        public string k_go;

        [DataMember]
        public string k_adv;

        [DataMember]
        public string k_adv_i;

        [DataMember]
        public string k_t0;

        [DataMember]
        public string k_t1;

        [DataMember]
        public string k_t2;

        [DataMember]
        public string k_t3;

        [DataMember]
        public string k_t4;

        [DataMember]
        public string h_name;

        [DataMember]
        public string h_is3d;

        [DataMember]
        public string sessions;
    }
}
