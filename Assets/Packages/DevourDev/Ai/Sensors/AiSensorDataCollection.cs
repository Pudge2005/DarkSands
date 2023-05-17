using System.Collections.Generic;

namespace DevourDev.Ai
{
    public class AiSensorDataCollection
    {
        private readonly Dictionary<int, AiSensorDataBase> _items = new();


        public void MarkAllDataIrrelevant()
        {
            var vals = _items.Values;

            foreach (var v in vals)
            {
                v.IsRelevant = false;
            }
        }


        /// <summary>
        /// For Sensors. Can return
        /// irrelevant or empty data.
        /// For guaranteed relevant
        /// data get it from sensor.
        /// </summary>
        public AiSensorDataBase GetBaseData(AiSensorBase sensor)
        {
            int hash = sensor.GetHashCode();

            if (!_items.TryGetValue(hash, out var data))
            {
                data = sensor.CreateBaseData();
                _items.Add(hash, data);
            }

            return data;
        }
    }
}
