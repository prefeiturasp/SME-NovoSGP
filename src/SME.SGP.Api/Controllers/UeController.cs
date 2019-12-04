using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ues")]
    [Authorize("Bearer")]
    public class UeController : ControllerBase
    {
        public UeController()
        {
        }
    }
}