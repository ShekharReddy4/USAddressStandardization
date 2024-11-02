using System.ComponentModel.DataAnnotations;
using USAddressStandardization.Models;

namespace USAddressStandardization.Models
{
    public class AddressViewModel
    {
        [Required(ErrorMessage = "Please enter an address to standardize")]
        [Display(Name = "Raw Address")]
        public string RawAddress { get; set; }

        public Address StandardizedAddress { get; set; }

        public string ErrorMessage { get; set; }
    }
}