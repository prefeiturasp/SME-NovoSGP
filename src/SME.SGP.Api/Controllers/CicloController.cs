using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dto;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ciclos")]
    [ValidaDto]
    public class CicloController : ControllerBase
    {
        private readonly IConsultasCiclo consultasCiclo;

        public CicloController(IConsultasCiclo consultasCiclo)
        {
            this.consultasCiclo = consultasCiclo ?? throw new System.ArgumentNullException(nameof(consultasCiclo));
        }

        [HttpPost]
        [ProducesResponseType(typeof(IEnumerable<CicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Post(IEnumerable<string> Ano, string AnoSelecionado, int Modalidade)
        {
            return Ok(consultasCiclo.Listar(Ano, AnoSelecionado, Modalidade));
        }

        [HttpGet]
        [Route("sugestao")]
        [ProducesResponseType(typeof(CicloDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Sugestao(int ano)
        {
            return Ok(consultasCiclo.Selecionar(ano));
        }
    }
}