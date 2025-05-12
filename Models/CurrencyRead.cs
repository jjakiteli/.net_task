using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class CurrencyRead
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }
    }
}
