using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Theatre
{
    /// <summary>
    /// Класс, который содержит в себе успешность запроса, количество фильмов на странице,
    /// список фильмов на странице, хеш.
    /// </summary>
    [DataContract]
    public class KinoafishaResponse<T>
    {
        [DataMember]
        public string success;

        [DataMember]
        public string count;

        [DataMember]
        public List<T> result;

        [DataMember]
        public string hash;
    }
}
