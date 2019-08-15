using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
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
        private readonly ServicoJurema servicoJurema;

        public CicloController(IConsultasCiclo consultasCiclo)
        {
            this.consultasCiclo = consultasCiclo ?? throw new System.ArgumentNullException(nameof(consultasCiclo));
            this.servicoJurema = servicoJurema;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CicloDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult Get()
        {
            return Ok(consultasCiclo.Listar(new List<int>()));
        }

        [HttpGet]
        [Route("teste")]
        public IActionResult Teste()
        {
            servicoJurema.ObterListaObjetivosAprendizagem();
            return Ok();
        }
    }
}