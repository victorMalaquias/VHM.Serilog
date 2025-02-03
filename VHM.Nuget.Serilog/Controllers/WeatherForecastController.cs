using Microsoft.AspNetCore.Mvc;

namespace VHM.Nuget.Serilog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        [HttpGet(Name = "SerilogSeq")]
        public int Get()
        {
            try
            {
                int a = 1;
                int b = 0;

                int q = a / b;

                return q;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}
