using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class CurrencyUpdate
    {
        [DataMember]
        [Required(ErrorMessage = "Id is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Id must be greater than 0.")]
        public int Id { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }
    }
}
