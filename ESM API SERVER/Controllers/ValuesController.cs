using Microsoft.AspNetCore.Mvc;

namespace ESM_API_SERVER
{
    [Route("/")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        [HttpGet("/")]
        public VersionsInformation[] Get() 
        {
            return Globales.VersionArrayCache;
        }
    }
}
