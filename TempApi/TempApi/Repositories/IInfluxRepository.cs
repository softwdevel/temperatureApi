using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

namespace TemperatureApi.Repositories
{
    public interface IInfluxRepository
    {
        Task AddPointAsync(PointData point);
        Task AddTemperatureAsync(TempModel data);
        Task AddSpecificPointAsync(PointData point, string bucket, string org);
        Task<TempModel> GetTemp(int startDate);
        Task<TempAgregatedModel> GetAgregatedTemp();
    }
}
