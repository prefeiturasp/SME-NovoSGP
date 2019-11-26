using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atribuicoes/cjs")]
    [Authorize("Bearer")]
    public class AtribuicaoCJController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get([FromQuery]AtribuicaoCJListaFiltroDto atribuicaoCJListaFiltroDto)
        {
            List<AtribuicaoCJListaRetornoDto> retorno = new List<AtribuicaoCJListaRetornoDto>();
            retorno.Add(new AtribuicaoCJListaRetornoDto()
            {
                Id = 1,
                Disciplinas = new string[] { "Matemática" },
                Modalidade = "Fundamental",
                Turma = "1A"
            });

            retorno.Add(new AtribuicaoCJListaRetornoDto()
            {
                Id = 2,
                Disciplinas = new string[] { "Matemática", "Geografia", "História" },
                Modalidade = "EJA",
                Turma = "4A"
            });

            retorno.Add(new AtribuicaoCJListaRetornoDto()
            {
                Id = 3,
                Disciplinas = new string[] { "Ciências" },
                Modalidade = "Ensino Médio",
                Turma = "3C"
            });

            return Ok(retorno);
        }

        [HttpPost]
        [ValidaDto]
        public IActionResult Post([FromBody]AtribuicaoCJPersistenciaDto[] atribuicaoCJPersistenciaDtos)
        {
            return Ok();
        }
    }
}