using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Theatre
{
    [DataContract]
    public class Item
    {
        [DataMember]
        public string id;

        [DataMember]
        public string name;
    }
}
