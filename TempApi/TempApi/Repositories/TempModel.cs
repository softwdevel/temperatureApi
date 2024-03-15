namespace TemperatureApi.Repositories
{
    public class TempModel
    {
        public float temperature { get; set; }
        public float humidity { get; set; }
    }
    public class TempAgregatedModel
    {
        public float avgTemperature { get; set; }
        public float minTemperature { get; set; }
        public float maxTemperature { get; set; }
        public float avgHumidity { get; set; }
        public float minHumidity { get; set; }
        public float maxHumidity { get; set; }
    }
}
