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
    [Route("api/v1/comunicacao/grupos/")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class GrupoComunicacaoController : ControllerBase
    {
        private readonly IComandosGrupoComunicacao comandoGrupoComunicacao;
        private readonly IConsultaGrupoComunicacao consultaGrupoComunicacao;

        public GrupoComunicacaoController(IComandosGrupoComunicacao comandoGrupoComunicacao,
            IConsultaGrupoComunicacao consultaGrupoComunicacao)
        {
            this.comandoGrupoComunicacao = comandoGrupoComunicacao ?? throw new System.ArgumentNullException(nameof(comandoGrupoComunicacao));
            this.consultaGrupoComunicacao = consultaGrupoComunicacao ?? throw new System.ArgumentNullException(nameof(consultaGrupoComunicacao));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Alterar([FromBody]GrupoComunicacaoDto dto, long id)
        {
            await comandoGrupoComunicacao.Alterar(dto, id);
            return Ok();
        }

        [HttpGet("listar")]
        [ProducesResponseType(typeof(IEnumerable<GrupoComunicacaoCompletoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> BuscarTodosAsync([FromQuery]FiltroGrupoComunicacaoDto filtro)
        {
            return Ok(await consultaGrupoComunicacao.Listar(filtro));
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Excluir([FromBody]long id)
        {
            await comandoGrupoComunicacao.Excluir(id);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Incluir([FromBody]GrupoComunicacaoDto dto)
        {
            await comandoGrupoComunicacao.Inserir(dto);
            return Ok();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(AtividadeAvaliativaCompletaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public IActionResult ObterPorId(long id)
        {
            return Ok(consultaGrupoComunicacao.ObterPorId(id));
        }
    }
}