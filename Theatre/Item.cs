using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Theatre
{
    [DataContract]
    public class Item
    {
        [DataMember]
        string id;

        [DataMember]
        string name;
    }
}
