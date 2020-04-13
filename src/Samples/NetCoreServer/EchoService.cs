using Contract;

namespace NetCoreServer
{
    public class EchoService : Contract.IEchoService
    {
        public CustomerFetchLightResponse CustomerFetchLight(CustomerFetchLightRequest request)
        {
            return new CustomerFetchLightResponse()
            {
                Name = request.Id.ToString() + "_" + request.IdWorking.ToString()
            };
        }
    }
}