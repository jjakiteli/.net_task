using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class VehicleRead
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Brand { get; set; }

        [DataMember]
        public string Model { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public string Currency { get; set; }
    }
}
