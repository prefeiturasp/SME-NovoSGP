using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Ocorrencias.Listagens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/ocorrencias")]
    [Authorize("Bearer")]
    public class OcorrenciaController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OcorrenciaListagemDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Get([FromQuery] FiltroOcorrenciaListagemDto dto)
        {
            var resultado = new List<OcorrenciaListagemDto>
            {
                new OcorrenciaListagemDto { AlunoOcorrencia = "Carlos Augusto Ferreira Dias (1234567)", DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"), Id = 1, Titulo = "Briga na escola"},
                new OcorrenciaListagemDto { AlunoOcorrencia = "Marcos Lobo (1234567)", DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"), Id = 2, Titulo = "Acidente em sala de aula"},
                new OcorrenciaListagemDto { AlunoOcorrencia = "3 alunos envolvidos", DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"), Id = 3, Titulo = "Vandalismo no pátio"},
            };

            return await Task.FromResult(Ok(resultado));
        }

        [HttpDelete]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Excluir([FromBody] IEnumerable<long> ids)
        {
            if (!ids?.Any() ?? true)
                throw new NegocioException("Selecione uma ou mais ocorrências para serem excluídas.");

            return await Task.FromResult(Ok());
        }
    }
}