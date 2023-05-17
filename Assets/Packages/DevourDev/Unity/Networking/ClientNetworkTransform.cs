using Unity.Netcode.Components;
using UnityEngine;

namespace DevourDev.Unity.Networking
{
    public class ClientNetworkTransform : NetworkTransform
    {
        [SerializeField] private bool _isServerAuthoritative = false;


        protected override bool OnIsServerAuthoritative()
        {
            return _isServerAuthoritative;
        }
    }
}
