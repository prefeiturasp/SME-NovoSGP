using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.Ocorrencias.Alteracoes;
using SME.SGP.Infra.Dtos.Ocorrencias.Detalhes;
using SME.SGP.Infra.Dtos.Ocorrencias.Listagens;
using SME.SGP.Infra.Dtos.Ocorrencias.Persistencias;
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
            var lista = new List<OcorrenciaListagemDto>
            {
                new OcorrenciaListagemDto { AlunoOcorrencia = "Carlos Augusto Ferreira Dias (1234567)", DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"), Id = 1, Titulo = "Briga na escola"},
                new OcorrenciaListagemDto { AlunoOcorrencia = "Marcos Lobo (1234567)", DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"), Id = 2, Titulo = "Acidente em sala de aula"},
                new OcorrenciaListagemDto { AlunoOcorrencia = "3 alunos envolvidos", DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"), Id = 3, Titulo = "Vandalismo no pátio"},
            };

            var paginas = new PaginacaoResultadoDto<OcorrenciaListagemDto>() {
                Items = lista,
                TotalPaginas = 1,
                TotalRegistros = lista.Count()
            };

            return await Task.FromResult(Ok(paginas));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OcorrenciaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        // O permissionamento será adicionado em uma task separada
        public async Task<IActionResult> Get(long id)
        {
            var alunos = new List<OcorrenciaAlunoDto>
            {
                new OcorrenciaAlunoDto { CodigoAluno = 1234567, Id = 1, Nome = "Carlos Augusto Ferreira Dias"},
                new OcorrenciaAlunoDto { CodigoAluno = 7654321, Id = 2, Nome = "Marcos Lobo" }
            };

            var resultado = new OcorrenciaDto
            {
                Alunos = alunos,
                CriadoEm = DateTime.Now,
                CriadoPor = "Criador do registro",
                CriadoRF = "999999",
                DataOcorrencia = DateTime.Today.ToString("dd/MM/yyyy"),
                Descricao = "Os alunos mencionados na ocorrência tiveram um briga em sala de aula após discussão",
                HoraOcorrencia = "09:30",
                Id = 1,
                OcorrenciaTipoId = 1,
                Titulo = "Briga na escola"
            };

            return await Task.FromResult(Ok(resultado));
        }

        [HttpPost]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Inserir([FromBody] InserirOcorrenciaDto dto)
        {
            if(dto is null)
                throw new NegocioException("Os dados da ocorrência devem ser informados.");

            var auditoria = new AuditoriaDto
            {
                CriadoEm = DateTime.Now.AddDays(-1),
                CriadoPor = "Criador do registro",
                CriadoRF = "999999",
                Id = 1
            };

            return await Task.FromResult(Ok(auditoria));
        }

        [HttpPut]
        [ProducesResponseType(typeof(AuditoriaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        public async Task<IActionResult> Alterar([FromBody] AlterarOcorrenciaDto dto)
        {
            if (dto is null || dto.Id == default)
                throw new NegocioException("Os dados da ocorrência devem ser informados.");

            var auditoria = new AuditoriaDto
            {
                AlteradoEm = DateTime.Now,
                AlteradoPor = "Usuário que fez a alteração",
                AlteradoRF = "999999",
                CriadoEm = DateTime.Now.AddDays(-1),
                CriadoPor = "Criador do registro",
                CriadoRF = "999999",
                Id = 1
            };

            return await Task.FromResult(Ok(auditoria));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
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