using System.Text.RegularExpressions;
using System.Globalization;
using USAddressStandardization.Models;

namespace USAddressStandardization.Services
{
    public class AddressStandardizerService : IAddressStandardizerService
    {
        private readonly Dictionary<string, string> _streetAbbreviations = new()
        {
            {"ST", "STREET"},
            {"RD", "ROAD"},
            {"AVE", "AVENUE"},
            {"BLVD", "BOULEVARD"},
            {"LN", "LANE"},
            {"DR", "DRIVE"},
            {"CT", "COURT"},
            {"PL", "PLACE"},
            {"TER", "TERRACE"},
            {"CIR", "CIRCLE"}
        };

        private readonly Regex _addressRegex = new(
            @"^(?<number>\d+)\s+(?<street>.*?)\s*(?<unit>(?:APT|UNIT|#)\s*[\w-]+)?\s*,?\s*(?<city>[^,]+)\s*,?\s*(?<state>[A-Z]{2})\s*(?<zip>\d{5}(?:-\d{4})?)?$",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        public Address StandardizeAddress(string rawAddress)
        {
            if (string.IsNullOrWhiteSpace(rawAddress))
                return null;

            var match = _addressRegex.Match(rawAddress);
            if (!match.Success)
                return null;

            return new Address
            {
                StreetNumber = StandardizeStreetNumber(match.Groups["number"].Value),
                StreetName = StandardizeStreetName(match.Groups["street"].Value),
                ApartmentUnit = StandardizeUnit(match.Groups["unit"].Value),
                City = StandardizeCity(match.Groups["city"].Value),
                State = match.Groups["state"].Value.ToUpper(),
                ZipCode = StandardizeZipCode(match.Groups["zip"].Value)
            };
        }

        private string StandardizeStreetNumber(string number) =>
            number.TrimStart('0');

        private string StandardizeStreetName(string street)
        {
            string standardized = street.ToUpper().Trim();

            foreach (var abbr in _streetAbbreviations)
            {
                standardized = Regex.Replace(
                    standardized,
                    $@"\b{abbr.Key}\b",
                    abbr.Value,
                    RegexOptions.IgnoreCase
                );
            }

            return standardized;
        }

        private string StandardizeUnit(string unit)
        {
            if (string.IsNullOrWhiteSpace(unit))
                return string.Empty;

            return unit.ToUpper()
                .Replace("#", "UNIT ")
                .Replace("APT", "APARTMENT ")
                .Trim();
        }

        private string StandardizeCity(string city) =>
            CultureInfo.CurrentCulture.TextInfo.ToTitleCase(city.ToLower().Trim());

        private string StandardizeZipCode(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode))
                return string.Empty;

            return zipCode.Length > 5 ?
                $"{zipCode[..5]}-{zipCode[5..].TrimStart('-')}" :
                zipCode;
        }
    }
}