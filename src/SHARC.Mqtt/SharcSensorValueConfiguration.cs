// Copyright (c) 2024 TrakHound Inc., All Rights Reserved.
// TrakHound Inc. licenses this file to you under the MIT license.

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
