using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atividade-avaliativa/tipos/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class TipoAvaliacao : ControllerBase
    {
        private readonly IComandosTipoAvaliacao comandoTipoAvaliacao;
        private readonly IConsultaTipoAvaliacao consultaTipoAvaliacao;

        public TipoAvaliacao(IComandosTipoAvaliacao comandoTipoAvaliacao,
            IConsultaTipoAvaliacao consultaTipoAvaliacao)
        {
            this.comandoTipoAvaliacao = comandoTipoAvaliacao ?? throw new System.ArgumentNullException(nameof(comandoTipoAvaliacao));
            this.consultaTipoAvaliacao = consultaTipoAvaliacao ?? throw new System.ArgumentNullException(nameof(consultaTipoAvaliacao));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Alterar([FromBody]TipoAvaliacaoDto dto, long id)
        {
            await comandoTipoAvaliacao.Alterar(dto, id);
            return Ok();
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<TipoAvaliacaoCompletaDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> BuscarTodosAsync([FromQuery]string nome, string descricao, bool? situacao)
        {
            return Ok(await consultaTipoAvaliacao.ListarPaginado(nome, descricao, situacao));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Excluir([FromBody]long[] tiposAvaliacaoId)
        {
            await comandoTipoAvaliacao.Excluir(tiposAvaliacaoId);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Incluir([FromBody]TipoAvaliacaoDto dto)
        {
            await comandoTipoAvaliacao.Inserir(dto);
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AtividadeAvaliativaCompletaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public IActionResult ObterPorId(long id)
        {
            return Ok(consultaTipoAvaliacao.ObterPorId(id));
        }
    }
}