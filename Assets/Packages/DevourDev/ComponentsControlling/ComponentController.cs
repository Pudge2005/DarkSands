using UnityEngine;

namespace DevourDev.Unity.Utils
{
    public abstract class ComponentController<TComp> : MonoBehaviour
        where TComp : Component
    {
        [SerializeField] private TComp _component;


        protected TComp Component => _component;


        //public void InitComponentController(TComp component)
        //{
        //    _component = component;
        //}


        protected virtual void Awake()
        {
            _component = gameObject.GetComponent<TComp>();
        }

    }
}
