using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;


namespace Models
{
    [DataContract]
    public class VehicleCreate
    {
        [DataMember]
        [Required(ErrorMessage = "Brand is required.")]
        [StringLength(100, ErrorMessage = "Brand can't be longer than 100 characters.")]
        public string Brand { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Model is required.")]
        [StringLength(100, ErrorMessage = "Model can't be longer than 100 characters.")]
        public string Model { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Price is required.")]
        public decimal Price { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Currency is required.")]
        public string Currency { get; set; }

        [DataMember]
        public List<string> Currencies { get; set; }
    }
}
