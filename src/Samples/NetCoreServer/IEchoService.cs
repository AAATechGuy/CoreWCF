namespace Contract
{
    using System.Runtime.Serialization;
    using System.ServiceModel;

    [ServiceContract]
    public partial interface IEchoService
    {
        /// <summary>
        /// Fetch <see cref="Customer"/> given CustomerId.
        /// </summary>
        /// <param name="request">A <see cref="CustomerFetchLightRequest"/> instance.</param>
        /// <returns>A <see cref="CustomerFetchLightResponse"/> instance.</returns>
        /// <exception cref="FaultException{SenderFaultDetail}">
        /// <list type="bullet">
        /// <item><see cref="SenderErrorCode.UserTokenInvalid"/></item>
        /// <item><see cref="SenderErrorCode.AuthorizationFailure"/></item>
        /// </list>
        /// </exception>
        /// <example>This sample shows how to use the <see cref="CustomerFetch"/> method:
        /// <code language="C#" source="Samples\Customer\CustomerFetchSample.cs" region="SampleSession" />
        /// </example>
        [OperationContract(Action = "CustomerFetchLight")]
        //// [FaultContract(typeof(SenderFaultDetail))]
        CustomerFetchLightResponse CustomerFetchLight(CustomerFetchLightRequest request);
    }

    /// <summary>
    /// Request parameter for <see cref="IClientCenterMiddleTier.CustomerFetchLight"/> method.
    /// </summary>
    /// <example>This sample shows how to pass <see cref="CustomerFetchRequest"/> to <see cref="IClientCenterMiddleTier.CustomerFetch"/> method:
    /// <code language="C#" source="Samples\Customer\CustomerFetchSample.cs" region="SampleSession" />
    /// </example>
    [MessageContract(WrapperNamespace = Namespaces.Default)]
    public class CustomerFetchLightRequest : Request
    {
        /// <summary>
        /// The id of the customer to be fetched.
        /// </summary>
        [MessageBodyMember]
        public int Id;

        [MessageBodyMember(Namespace = Namespaces.Default)]
        public int IdWorking;
    }

    /// <summary>
    /// Response parameter for <see cref="IClientCenterMiddleTier.CustomerFetchLight"/> method.
    /// </summary>
    /// <example>This sample shows how to get <see cref="CustomerFetchResponse"/> in <see cref="IClientCenterMiddleTier.CustomerFetch"/> method:
    /// <code language="C#" source="Samples\Customer\CustomerFetchSample.cs" region="SampleSession" />
    /// </example>
    [MessageContract(WrapperNamespace = Namespaces.Default)]
    public class CustomerFetchLightResponse : Response
    {
        /// <summary>
        /// Customer Name
        /// </summary>
        [MessageBodyMember]
        public string Name;
    }

    [MessageContract]
    [DataContract(Namespace = Namespaces.Default)]
    public class Request : MessageBase { }

    [MessageContract]
    [DataContract(Namespace = Namespaces.Default)]
    public class Response : MessageBase { }

    [MessageContract]
    [DataContract(Namespace = Namespaces.Default)]
    public class MessageBase { }

    public static class Namespaces
    {
        /// <summary>
        /// CCMT v3.0 service namespace.
        /// </summary>
        public const string Default = "http://advertising.microsoft.com/2009/08/clientcenter/mt";
    }
}