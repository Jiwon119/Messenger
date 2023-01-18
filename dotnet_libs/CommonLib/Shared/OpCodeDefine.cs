using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Shared
{
    public enum OpCode
    {
        None = 0,

        Admin_RC_Command = 1,
        Admin_RC_BroadcastData = 2,

        Admin_ResetStatus = 5,

        Request_HumidityData = 10,
        Request_RainfallData = 11,

        Response_HumidityData = 12,
        Response_RainfallData = 13,

        Request_AllRTUData = 14,
        Response_AllRTUData = 15,

        Detour_AllRTUData = 16,
        Notify_PredictWaterHeight_Model2 = 17,

        Request_IlluminanceData = 20,
        Response_IlluminanceData = 21,

        Request_HM_RF_Data = 22,
        Response_HM_RF_Data = 23,

        RC_Change_TestHD1Value = 24,
        RC_Change_TestRD2Value = 25,
        RC_Reset_State = 26,
        RC_Make_State_Snapshot = 27,

        Max
    }
}
