using System.Collections.Generic;
using DarkSands.Characters;
using DarkSands.Global;
using DevourDev.Ai;
using DevourDev.Utility;
using UnityEngine;

namespace DarkSands.Ai.Content
{
    [DefaultExecutionOrder(-10000)]
    public sealed class PatrollingManager : Singleton<PatrollingManager>
    {
        [SerializeField] private Transform[] _patrolPoints;


        public IReadOnlyList<Transform> PatrolPoints => _patrolPoints;


        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void InitializeSingleton()
        {
            InitializeSingletonBeforeSplashScreen();
        }
    }

    public sealed class PatrolState : StateBehaviourBase<AiContext>
    {
        protected override void OnStateEnter(AiContext context)
        {
        }

        protected override void OnStateExit(AiContext context)
        {
        }

        protected override void OnStateUpdate(AiContext context)
        {
            var tr = context.Character.transform;
        }
    }

    [CreateAssetMenu(menuName = AssetMenuConstants.Sensors + "Characters In Range")]
    public sealed class CharactersInRangeSensor : AiSensorBase<AiContext, CharactersInRangeSensor.SensorData>
    {
        public sealed class SensorData : AiSensorDataBase
        {
            private readonly List<Character> _characters = new();


            public List<Character> Characters => _characters;
        }


        protected override void Scan(AiContext context, SensorData data)
        {
            var list = data.Characters;
            list.Clear();

            var lmask = GlobalLayers.Instance.Characters;
            float range = 10f; //it will be stat and assosiated via Scriptable Object

            var charactersSpan = NonAllocHelpers.OverlapSphere(context.Character.transform.position, range, lmask, QueryTriggerInteraction.Ignore);
            var len = charactersSpan.Length;

            for (int i = 0; i < len; i++)
            {
                if (charactersSpan[i].TryGetComponent<Character>(out var character))
                    list.Add(character);
            }

            // Furthergoing fetching (angle, behind obstacle...).
        }

        protected override SensorData CreateData()
        {
            throw new System.NotImplementedException();
        }
    }
}
