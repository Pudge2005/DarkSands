using System;

namespace DevourDev.Utility
{
    public static class TimeConverter
    {
        private const string _millisecondPostfix = "ms";
        private const string _secondPostfix = "s";
        private const string _minutePostfix = "m";
        private const string _hourPostfix = "h";
        private const string _dayPostfix = "d";

        private const float _secsInMin = 60f;
        private const float _minsInHour = 60f;
        private const float _hoursInDay = 24f;


        public static string ConvertSeconds(float seconds)
        {
            var timeSpan = TimeSpan.FromSeconds(seconds);
            return timeSpan.ToString("hh\\:mm\\:ss");
            //switch (seconds)
            //{
            //    case > _secsInMin * _minsInHour *_hoursInDay:

            //    default:
            //        break;
            //}
        }
    }
}
