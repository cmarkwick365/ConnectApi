using LsConnectService;
using System.ServiceModel;

namespace LsConnectClient;

public interface IWcfClient
{
    IConnectServer ConnectServerClient { get;  }
}

public class WcfClient : IWcfClient
{
    public IConnectServer ConnectServerClient { get;  }

    public WcfClient(string? endpointAddress)
    {
        var binding = new BasicHttpBinding();
        if (endpointAddress == null) throw new ApplicationException("endpointAddress not found in appsettings!");
        var endpoint = new EndpointAddress(new Uri(endpointAddress));
        var channelFactory = new ChannelFactory<IConnectServer>(binding, endpoint);
        ConnectServerClient = channelFactory.CreateChannel();
    }


    
}