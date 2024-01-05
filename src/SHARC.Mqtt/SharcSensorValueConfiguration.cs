namespace SHARC.Mqtt
{
    public class SharcSensorValueConfiguration
    {
        public int Deadband { get; set; }

        public string Calibrate { get; set; }

        public int[] CalibratedRange { get; set; }

        public string Convert { get; set; }
    }
}
