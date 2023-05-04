using UnityEngine;

namespace DevourDev.Unity.TwoDThreeD
{
    public sealed class DebugRotator : MonoBehaviour
    {
        [SerializeField] private Vector3 _axis = Vector3.up;
        [SerializeField] private float _speed = 20;


        private void Update()
        {
            transform.Rotate(_axis, _speed * Time.deltaTime);
        }
    }
}
