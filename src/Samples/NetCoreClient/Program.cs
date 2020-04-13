using Newtonsoft.Json;
using System;
using System.Net;
using System.ServiceModel;
using System.Text;

namespace NetCoreClient
{
    class Program
    {
        private readonly static string _basicHttpEndPointAddress = @"http://localhost:3090/clientcenter/mt";
        private readonly static string _soapEnvelopeContent = @"
<s:Envelope xmlns:s='http://schemas.xmlsoap.org/soap/envelope/' _LoggingListEnvelopeCount='2'>
  <s:Header _LoggingListHeaderCount='5'>
    <h:RequestDetails xmlns:h='http://advertising.microsoft.com/2009/08/clientcenter/mt' xmlns:i='http://www.w3.org/2001/XMLSchema-instance' _LoggingListRequestDetailsCount='4'>
      <h:ClientApplication>CampaignWebUI</h:ClientApplication>
      <h:ClientMachine>RD00155DF9FDC1</h:ClientMachine>
      <h:RequestId>f62dfba5-389b-495c-990a-d60bf3c9944a</h:RequestId>
      <h:SessionId>54d694e5-9112-4686-ab71-ab9c0077d205</h:SessionId>
    </h:RequestDetails>
    <h:TrackingId xmlns:h='http://advertising.microsoft.com/2009/08/clientcenter/mt'>716ae182-b09b-4589-b587-5744991f70ae</h:TrackingId>
    <h:UserToken xmlns:h='http://advertising.microsoft.com/2009/08/clientcenter/mt'>userToken</h:UserToken>
  </s:Header>
  <s:Body _LoggingListBodyCount='1'>
    <CustomerFetchLightRequest xmlns='http://advertising.microsoft.com/2009/08/clientcenter/mt' _LoggingListCustomerFetchLightRequestCount='1'>
      <Id>1</Id>
      <IdWorking>2</IdWorking>
    </CustomerFetchLightRequest>
  </s:Body>
</s:Envelope>".Replace("'", "\"");

        static void Main(string[] args)
        {
            CallUsingWcf();
            CallUsingWebRequest();

            Console.WriteLine("Hit enter to exit");
            Console.ReadLine();
        }

        private static void CallUsingWcf()
        {
            var factory = new ChannelFactory<Contract.IEchoService>(new BasicHttpBinding(), new EndpointAddress(_basicHttpEndPointAddress));
            factory.Open();
            var channel = factory.CreateChannel();
            ((IClientChannel)channel).Open();

            var result = channel.CustomerFetchLight(
                new Contract.CustomerFetchLightRequest()
                {
                    Id = 1,
                    IdWorking = 2
                });

            Console.WriteLine("\r\n==> httpWcfCall CustomerFetchLight.response => " + result.Name);
            if (result.Name.Contains("1_2")) { Console.WriteLine("SUCCESS: valid response"); }
            else { Console.WriteLine("FAIL: invalid response"); }

            ((IClientChannel)channel).Close();
            factory.Close();

            //// factory = new ChannelFactory<Contract.IEchoService>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8808/nettcp"));
            //// factory.Open();
            //// channel = factory.CreateChannel();
            //// ((IClientChannel)channel).Open();
            //// Console.WriteLine("net.tcp Echo(\"Hello\") => " + channel.Echo("Hello"));
            //// ((IClientChannel)channel).Close();
            //// factory.Close();

            //// // Complex type testing
            //// factory = new ChannelFactory<Contract.IEchoService>(new BasicHttpBinding(), new EndpointAddress(_basicHttpEndPointAddress));
            //// factory.Open();
            //// channel = factory.CreateChannel();
            //// ((IClientChannel)channel).Open();
            //// Console.WriteLine("http EchoMessage(\"Complex Hello\") => " + channel.ComplexEcho(new Contract.EchoMessage() { Text = "Complex Hello" }));
            //// ((IClientChannel)channel).Close();
            //// factory.Close();
        }

        private static void CallUsingWebRequest()
        {
            // 
            // The following sample, creates a basic web request to the specified endpoint, sends the SOAP request and reads the response
            // 

            // Prepare the raw content
            var utf8Encoder = new UTF8Encoding();
            var bodyContentBytes = utf8Encoder.GetBytes(_soapEnvelopeContent);

            // Create the web request
            var webRequest = System.Net.WebRequest.Create(new Uri(_basicHttpEndPointAddress));
            webRequest.Headers.Add("SOAPAction", "CustomerFetchLight");
            webRequest.ContentType = "text/xml";
            webRequest.Method = "POST";
            webRequest.ContentLength = bodyContentBytes.Length;

            // Append the content
            var requestContentStream = webRequest.GetRequestStream();
            requestContentStream.Write(bodyContentBytes, 0, bodyContentBytes.Length);

            // Send the request and read the response
            using (System.IO.Stream responseStream = webRequest.GetResponse().GetResponseStream())
            {
                using (System.IO.StreamReader responsereader = new System.IO.StreamReader(responseStream))
                {
                    var soapResponse = responsereader.ReadToEnd();
                    Console.WriteLine($"\r\n==> WebRequestHttp SOAP Response: {soapResponse}");

                    if (soapResponse.Contains("1_2")) { Console.WriteLine("SUCCESS: valid response"); }
                    else { Console.WriteLine("FAIL: invalid response"); }
                }
            }
        }
    }
}