using Unity.Netcode.Components;
using UnityEngine;

namespace DevourDev.Unity.Networking
{
    public sealed class ClientNetworkAnimator : NetworkAnimator
    {
        [SerializeField] private bool _isServerAuthoritative = false;


        protected override bool OnIsServerAuthoritative()
        {
            return _isServerAuthoritative;
        }
    }
}
