using UnityEngine;

namespace DevourDev.Ai
{
    public abstract class AiSensorBase<TContext, TData> : AiSensorBase<TContext>
        where TContext : AiContextBase
        where TData : AiSensorDataBase
    {
        public TData GetRelevantData(TContext context)
        {
            return (TData)GetRelevantBaseData(context);
        }

        protected sealed override void ScanInherited(TContext context)
        {
            var data = (TData)GetOrCreateBaseDataInternal(context);
            Scan(context, data);
            data.IsRelevant = true;
        }

        protected sealed override AiSensorDataBase CreateBaseDataInherited()
        {
            return CreateData();
        }

        protected abstract void Scan(TContext context, TData data);

        protected abstract TData CreateData();
    }

    public abstract class AiSensorBase<TContext> : AiSensorBase
        where TContext : AiContextBase
    {
        public void Scan(TContext context)
        {
            ScanInherited(context);
        }

        public AiSensorDataBase GetRelevantBaseData(TContext context)
        {
            var data = GetOrCreateBaseDataInternal(context);

            if (!data.IsRelevant)
                Scan(context);

            return data;
        }

        internal AiSensorDataBase GetOrCreateBaseDataInternal(TContext context)
        {
            var collection = context.StateMachine.SensorDataCollection;
            var data = collection.GetBaseData(this);

            return data;
        }

        protected abstract void ScanInherited(TContext context);

    }

    public abstract class AiSensorBase : ScriptableObject
    {
        public AiSensorDataBase CreateBaseData()
        {
            return CreateBaseDataInherited();
        }


        protected abstract AiSensorDataBase CreateBaseDataInherited();
    }
}
