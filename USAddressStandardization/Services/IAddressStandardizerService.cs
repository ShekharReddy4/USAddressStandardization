using USAddressStandardization.Models;

namespace USAddressStandardization.Services
{
    public interface IAddressStandardizerService
    {
        Address StandardizeAddress(string rawAddress);
    }
}
