using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/comunicacao")]
    [Authorize("Bearer")]
    public class ComunicadoContro : ControllerBase
    {
        private readonly IComandoComunicado comandoComunicado;
        private readonly IConsultaComunicado consultaComunicado;

        public ComunicadoContro(IConsultaComunicado consultaComunicado, IComandoComunicado comandoComunicado)
        {
            this.consultaComunicado = consultaComunicado ?? throw new System.ArgumentNullException(nameof(consultaComunicado));
            this.comandoComunicado = comandoComunicado ?? throw new System.ArgumentNullException(nameof(comandoComunicado));
        }
    }
}