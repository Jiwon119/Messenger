using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Shared
{
    public enum SensorID : int
    {
        None,

        Humidity = 1,
        Rainfall,
        Pressure,
        innerWaterHeight,
        OuterWaterHeight,

        Max
    }

    public static class SensorIDParser
    {
        private static readonly int SensorID_Bandwidth = 100;
        private static readonly int RTUID_Bandwidth = 10000;

        public static bool MakeSensorID(int orgIdx, int rtuIdx, int sensorIdx, out int id)
        {
            id = 0;
            if (orgIdx < 0)
                return false;
            if (rtuIdx < 0 || rtuIdx >= RTUID_Bandwidth / SensorID_Bandwidth)
                return false;
            if (sensorIdx < 0 || sensorIdx >= SensorID_Bandwidth)
                return false;

            id = (orgIdx * RTUID_Bandwidth) + (rtuIdx * SensorID_Bandwidth) + (sensorIdx);
            return true;
        }

        public static bool ParseSensorID(int id, out int orgIdx, out int rtuIdx, out int sensorIdx)
        {
            orgIdx = 0;
            rtuIdx = 0;
            sensorIdx = 0;
            if (id < 0)
                return false;

            // 10112
            int left = id / SensorID_Bandwidth; // 101
            sensorIdx = id % SensorID_Bandwidth; // 12

            int rtu_left = (id % RTUID_Bandwidth) - sensorIdx; // 112 - 12 = 100
            rtuIdx = rtu_left / 100; // 1

            orgIdx = id / RTUID_Bandwidth; // 1
            return true;
        }

    }
}
