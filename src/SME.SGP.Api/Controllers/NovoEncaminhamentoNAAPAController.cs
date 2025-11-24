using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/novo-encaminhamento-naapa")]
    [Authorize("Bearer")]
    public class NovoEncaminhamentoNAAPAController : ControllerBase
    {

    }
}