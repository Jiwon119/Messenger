using System;
using System.Collections.Generic;
using System.Text;

namespace CommonLib.Shared
{
    public class DataField
    {
        public DataField(string unit)
        {
            _fieldUnit = unit;
        }

        private string _fieldUnit = "";
        private float _fieldValue = 0.0f;
        private bool _isSet = false;
        private int _seqIdx = 0;

        public bool IsUpdatedValue { get => _isSet; }
        public float Value 
        { 
            get => _fieldValue; 
            set
            {
                _isSet = true;
                _fieldValue = value;
            } 
        }
        public float Reset
        {
            set
            {
                _isSet = false;
                _fieldValue = value;
            }
        }

        public void UnsetFlag()
        {
            _isSet = false;
        }

        public bool CheckSequenceIndex(int seq)
        {
            return _seqIdx == seq;
        }

        public override string ToString()
        {
            return $"{_fieldValue} {_fieldUnit}";
        }
    }

    public class RTU_Data
    {
        public DataField HumidityData = new DataField("%");
        public DataField RainfallData = new DataField("mm/h");
        public DataField PressureData = new DataField("kPa");
        public DataField InnerWHData = new DataField("m");
        public DataField OuterWHData = new DataField("m");

        public int SeqIdx = 0;

        // 테스트용 임시 데이터
        private const float MaxRainfall = 80.0f;
        private const float MinRainFall = 0.0f;

        private const float MaxHumidity = 100.0f;
        private const float MinHumudity = 50.0f;
        private const float FloorHumidity = 20.0f;

        public const float THRainfall = 10.0f;
        public const float THHumidity = 5.0f;

        public float CalcRelativeRainFallFromHumidity()
        {
            float ratioHD = Math.Max(0.0f, HumidityData.Value - MinHumudity) / (MaxHumidity - MinHumudity);
            return MaxRainfall * ratioHD;
            //return Math.Max(0.0f, MaxRainfall * (HumidityData.Value / 100.0f) - 60.0f);
        }

        public float CalcRelativeHumidityFromRainFall()
        {
            float ratioRD = Math.Max(0.0f, RainfallData.Value - MinRainFall) / (MaxRainfall - MinRainFall);
            //float floorHD = MaxHumidity - MinHumudity - (FloorHumidity * ratioRD);
            float floorHD = (FloorHumidity * ratioRD);
            return MinHumudity + (ratioRD * floorHD);
            //return (RainfallData.Value / MaxRainfall) + MinHumudity;
        }

        public static float PredictWaterHeightFrom_Model1(RTU_Data rtu1, RTU_Data rtu2)
        {
            return 0.0f;
        }

        public static float PredictWaterHeightFrom_Model2(RTU_Data rtu1, RTU_Data rtu2)
        {
            return 0.0f;
        }

        //

    }

    public class RTU_InitialData
    {
        public int RTUID { get; set; }
        public RTU_InitialUnit[] InitialData { get; set; }
    }

    public class RTU_InitialUnit
    {
        public string UnitType { get; set; }
        public string UnitName { get; set; }
        public string ValueName { get; set; }
        public string StateType { get; set; }
        public decimal AnnoMin { get; set; }
        public decimal AnnoMax { get; set; }
        public decimal ValueMin { get; set; }
        public decimal ValueMax { get; set; }
    }

    public class RTU_StateData
    {
        public int RTUID { get; set; }
        public RTU_StateUnit[] StateData { get; set; }
    }

    public class RTU_StateUnit
    {
        public string UnitName { get; set; }
        public string StateType { get; set; }
        public decimal DataValue { get; set; }
    }
}
