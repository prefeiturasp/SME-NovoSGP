using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/calendarios/tipos")]
    [ValidaDto]
    [Authorize("Bearer")]
    public class TipoAtividadeAvaliativaController : ControllerBase
    {
        private readonly IComandosTipoAtividadeAvaliativa comandoTipoAtividadeAvaliativa;

        public TipoAtividadeAvaliativaController(IComandosTipoAtividadeAvaliativa comandoTipoAtividadeAvaliativa)
        {
            this.comandoTipoAtividadeAvaliativa = comandoTipoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(comandoTipoAtividadeAvaliativa));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.TCE_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody]TipoAtividadeAvaliativaDto dto, long id)
        {
            await comandoTipoAtividadeAvaliativa.Alterar(dto, id);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.TCE_I, Policy = "Bearer")]
        public async Task<IActionResult> Incluir([FromBody]TipoAtividadeAvaliativaDto dto)
        {
            await comandoTipoAtividadeAvaliativa.Inserir(dto);
            return Ok();
        }
    }
}