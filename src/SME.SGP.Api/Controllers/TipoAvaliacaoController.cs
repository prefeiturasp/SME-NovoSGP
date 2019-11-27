using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atividade-avaliativa/tipos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class TipoAvaliacao : ControllerBase
    {
        private readonly IComandosTipoAvaliacao comandoTipoAvaliacao;

        public TipoAvaliacao(IComandosTipoAvaliacao comandoTipoAtividadeAvaliativa)
        {
            this.comandoTipoAvaliacao = comandoTipoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(comandoTipoAtividadeAvaliativa));
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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous] //ainda nao existe perfil pra essa função
        public async Task<IActionResult> Incluir([FromBody]TipoAvaliacaoDto dto)
        {
            await comandoTipoAvaliacao.Inserir(dto);
            return Ok();
        }
    }
}