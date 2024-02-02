using Microsoft.MixedReality.OpenXR.Remoting;
using UnityEngine;

namespace SensorHub
{
    public class AppRemotingConnector: MonoBehaviour
    {
        public void Start()
        {
            HoloRemoting();
        }

        public void HoloRemoting()
        {
            RemotingConnectConfiguration configuration =
                new RemotingConnectConfiguration { 
                    RemotePort = 8265, MaxBitrateKbps = 20000 
                };

            AppRemoting.StartConnectingToPlayer(configuration);
        }
    }
}
