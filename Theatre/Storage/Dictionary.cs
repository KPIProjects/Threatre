using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Theatre
{
    /// <summary>
    /// Класс, который содержит в себе список фильмов на странице, номер страницы, 
    /// количество страниц и количество фильмов.
    /// </summary>
    /// 
    [DataContract]
    public class Dictionary
    {
        [DataMember]
        public string page;

        [DataMember]
        public string total_pages;

        [DataMember]
        public string total_results;

        [DataMember]
        public List<ShortMovie> results;
    }
}
