﻿namespace TemperatureApi.Repositories
{
    public class InfluxConfig
    {
        public const string ConfigName = "InfluxDB";

        public string ApiKey { get; init; }
        public string Address { get; init; }
    }
}
