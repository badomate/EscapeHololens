using Microsoft.MixedReality.OpenXR.Remoting;
using UnityEngine;

namespace Gameplay
{
    public class PseudoGameManager : MonoBehaviour
    {
        public int turnId = 0;
        public int teammateId = 0;
        public int targetId = 0;

        public static PseudoGameManager Instance { get; private set; }
        private void Awake()
        {
            // If there is an instance, and it's not me, delete myself.

            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
                //HoloRemoting();
            }
        }

        public void HoloRemoting()
        {
            RemotingConnectConfiguration configuration =
                    new RemotingConnectConfiguration { RemotePort = 8265, MaxBitrateKbps = 20000 };
            AppRemoting.StartConnectingToPlayer(configuration);
        }
    }
}