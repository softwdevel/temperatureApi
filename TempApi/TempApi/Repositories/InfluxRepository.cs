using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Core.Flux.Domain;
using InfluxDB.Client.Writes;

namespace TemperatureApi.Repositories
{
    public class InfluxRepository : IInfluxRepository
    {
        private const string Organization = "Group5";
        protected readonly string Bucket = "SensorData";
        protected readonly string Measurement = "Temperature";

       
        protected readonly IInfluxDBClient Client;

        public InfluxRepository(IInfluxDBClient client)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
         
        }


        public async Task AddPointAsync(PointData point)
        {
            //*
             point = PointData.Measurement("Temperature")
                 .Tag("room", "bedroom1")
                 .Tag("id", "3fa85f64-5717-4562-b3fc-2c963f66afa6")
                 .Field("temperature", 55)
                 .Field("humidity", 2584)
                 .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
             //*/
            await Client.GetWriteApiAsync().WritePointAsync(point: point, bucket: Bucket, org: Organization);
        }

        public async Task AddSpecificPointAsync(PointData point, string bucket, string org)
        {
                    /*
             point = PointData.Measurement("Temperature")
                 .Tag("room", "bedroom1")
                 .Tag("id", "3fa85f64-5717-4562-b3fc-2c963f66afa6")
                 .Field("temperature", 55)
                 .Field("humidity", 2584)
                 .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
             //*/
            await Client.GetWriteApiAsync().WritePointAsync(point: point, bucket: bucket, org: org);
        }

        public async Task AddTemperatureAsync(TempModel data)
        {
            var point = PointData.Measurement(Measurement)
                .Tag("room", "bedroom1")
                .Field("temperature", data.temperature)
                .Field("humidity", data.humidity)
                .Timestamp(DateTime.UtcNow, WritePrecision.Ns);
            //*/
            await Client.GetWriteApiAsync().WritePointAsync(point: point, bucket: Bucket, org: Organization);
        }

        public async Task<TempAgregatedModel> GetAgregatedTemp()
        {

            var dateTime = DateTime.Now;

            DateTime initTime;
            DateTime finsihTime;
            var checkDateTime = DateTime.Now;
            var checkDateString = checkDateTime.ToString("yyyy-MM-dd");
            checkDateString = checkDateString + "T08:00:00Z";
            DateTime checkDateStringToDate = DateTime.Parse(checkDateString);


            if (checkDateStringToDate > DateTime.Now)
            {
                // time is before 8:00am
                initTime = dateTime.AddDays(-2);
                finsihTime = dateTime.AddDays(-1);


            } else
            {
                initTime = dateTime.AddDays(-1);
                finsihTime = dateTime;
                // time is after 8:00am
            }
            var stringInitTime = initTime.ToString("yyyy-MM-dd");
            var stringfinsihTime = finsihTime.ToString("yyyy-MM-dd");

            stringInitTime = stringInitTime + "T20:00:00Z";
            stringfinsihTime = stringfinsihTime + "T08:00:00Z";

            
            string query = "from(bucket: \"" + Bucket + "\") |> range(start: " + stringInitTime + ", stop: " + stringfinsihTime + ") |> filter(fn: (r) => r[\"_measurement\"] == \"" + Measurement + "\") |> aggregateWindow(every: 12h, fn: max, createEmpty: false) |> yield(name: \"max\")";
            string queryMin = "from(bucket: \"" + Bucket + "\") |> range(start: " + stringInitTime + ", stop: " + stringfinsihTime + ") |> filter(fn: (r) => r[\"_measurement\"] == \"" + Measurement + "\") |> aggregateWindow(every: 12h, fn: min, createEmpty: false) |> yield(name: \"min\")";

            string queryAvg = "from(bucket: \"" + Bucket + "\") |> range(start: " + stringInitTime + ", stop: " + stringfinsihTime + ") |> filter(fn: (r) => r[\"_measurement\"] == \"" + Measurement + "\") |> aggregateWindow(every: 12h, fn: median, createEmpty: false) |> yield(name: \"median\")";


            /*
            from(bucket: "SensorData")
              |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
              |> filter(fn: (r) => r["_measurement"] == "Temperature")
              |> aggregateWindow(every: 12h, fn: max, createEmpty: false)
              |> yield(name: "max")

            from(bucket: "SensorData")
              |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
              |> filter(fn: (r) => r["_measurement"] == "Temperature")
              |> aggregateWindow(every: 12h, fn: min, createEmpty: false)
              |> yield(name: "min")

            from(bucket: "SensorData")
              |> range(start: v.timeRangeStart, stop: v.timeRangeStop)
              |> filter(fn: (r) => r["_measurement"] == "Temperature")
              |> aggregateWindow(every: 12h, fn: median, createEmpty: false)
              |> yield(name: "median")

            //*/


            TempAgregatedModel agregTemp = new();
            List<FluxTable> fluxTablesMax = await Client.GetQueryApi().QueryAsync(query: query, org: Organization);

            foreach (var fluxTable in fluxTablesMax)
            {
                var value = fluxTable.Records[0].GetValue().ToString();
                if (value == null)
                {
                    value = "0";
                }

                var field = fluxTable.Records[0].GetField();
                if (field == "temperature")
                {
                    agregTemp.maxTemperature = float.Parse(value);
                }
                else if (field == "humidity")
                {
                    agregTemp.maxHumidity = float.Parse(value);
                }
            }
            List<FluxTable> fluxTablesMin = await Client.GetQueryApi().QueryAsync(query: queryMin, org: Organization);

            foreach (var fluxTable in fluxTablesMin)
            {
                var value = fluxTable.Records[0].GetValue().ToString();
                if (value == null)
                {
                    value = "0";
                }

                var field = fluxTable.Records[0].GetField();
                if (field == "temperature")
                {
                    agregTemp.minTemperature = float.Parse(value);
                }
                else if (field == "humidity")
                {
                    agregTemp.minHumidity = float.Parse(value);
                }
            }
            List<FluxTable> fluxTablesAvg = await Client.GetQueryApi().QueryAsync(query: queryAvg, org: Organization);

            foreach (var fluxTable in fluxTablesAvg)
            {
                var value = fluxTable.Records[0].GetValue().ToString();
                if (value == null)
                {
                    value = "0";
                }

                var field = fluxTable.Records[0].GetField();
                if (field == "temperature")
                {
                    agregTemp.avgTemperature = float.Parse(value);
                }
                else if (field == "humidity")
                {
                    agregTemp.avgHumidity = float.Parse(value);
                }
            }
            return agregTemp;
        }

        public Task<TempAgregatedModel> GetAgregatedTemp(int startDate)
        {
            throw new NotImplementedException();
        }

        public Task<TempModel> GetTemp(int startDate)
        {
            throw new NotImplementedException();
        }
    }
}
