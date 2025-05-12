using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace Models
{
    [DataContract]
    public class CurrencyCreate
    {
        [DataMember]
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name can't be longer than 100 characters.")]
        public string Name { get; set; }
    }
}
