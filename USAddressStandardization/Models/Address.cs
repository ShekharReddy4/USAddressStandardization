namespace USAddressStandardization.Models
{

    public class Address
    {
        public string StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string ApartmentUnit { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }

        public string FormattedAddress => $"{StreetNumber} {StreetName}" +
            (!string.IsNullOrEmpty(ApartmentUnit) ? $"\n{ApartmentUnit}" : "") +
            $"\n{City}, {State} {ZipCode}";
    }

}

