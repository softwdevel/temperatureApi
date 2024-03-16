using Microsoft.AspNetCore.Mvc;
using TemperatureApi.Repositories;

namespace TemperatureApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TempController: ControllerBase
    {
        private readonly IInfluxRepository _influxRepository;

        public TempController(IInfluxRepository influxRepository)
        {
            _influxRepository = influxRepository;
        }
        [HttpPost]
        public async Task<ActionResult<TempAgregatedModel>> insertData([FromBody] TempModel model)
        {
            await _influxRepository.AddTemperatureAsync(model);
            return Ok(model);
        }

        [HttpGet]
        [Route("getLatest", Name = "getDate")]
        public async Task<ActionResult<string>> getData()
        {

            //var dateTime = DateTime.Now;
            var agregData = await _influxRepository.GetAgregatedTemp();
            //var dateTime = DateTime.Now;ring("yyyy-mm-dd");

            return Ok(agregData);
        }
        [HttpGet]
        [Route("getCurrent", Name = "getCurrentData")]
        public async Task<ActionResult<string>> getCurrentData()
        {

            //var dateTime = DateTime.Now;
            var agregData = await _influxRepository.GetCurrentTemp();
            //var dateTime = DateTime.Now;ring("yyyy-mm-dd");

            return Ok(agregData);
        }
        /*
        [HttpGet]
        public async Task<ActionResult<TempModel>> getData( DateTime dateTime)
        {
            await _influxRepository.AddTemperatureAsync(model);
            return Ok(model);
        }
        //*/

        /*
        [HttpPost]
        [Route("robot", Name = "RobotStatusAdd")]
        [ProducesResponseType(typeof(RobotStatusModel), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<RobotStatusModel>> AddRobotStatusAsync([FromBody] RobotStatusModel model)
        {
            if (model.IsValid() == false)
                return BadRequest(new ApiResponse((int)HttpStatusCode.BadRequest, "Parameters were not specified."));
            try
            {
                //TODO: put behind service
                //await _robotStatusRepository.AddAsync(model, () => model.Id);
                await _influxRobotStatusRepository.AddAsync(model);
            }
            catch (Exception ex)
            {
                var statusCode = (int)HttpStatusCode.InternalServerError;
                _logger.LogError(ex, "An error occurred:");
                return StatusCode(statusCode, new ApiResponse(statusCode, $"An error has occurred: {ex.Message}"));
            }

            return Ok(model);
        }
        //*/
    }
}
