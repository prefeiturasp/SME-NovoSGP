using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/atividade-avaliativa/")]
    [ValidaDto]
    public class AtividadeAvaliativaController : ControllerBase
    {
        private readonly IComandosAtividadeAvaliativa comandoAtividadeAvaliativa;

        public AtividadeAvaliativaController(IComandosAtividadeAvaliativa comandoAtividadeAvaliativa)
        {
            this.comandoAtividadeAvaliativa = comandoAtividadeAvaliativa ?? throw new System.ArgumentNullException(nameof(comandoAtividadeAvaliativa));
        }

        [HttpPut("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_A, Policy = "Bearer")]
        public IActionResult Alterar([FromBody]AtividadeAvaliativaDto atividadeAvaliativaDto, long id)
        {
            comandoAtividadeAvaliativa.Alterar(atividadeAvaliativaDto, id);
            return Ok();
        }

        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.CP_E, Policy = "Bearer")]
        public async Task<IActionResult> ExcluirAsync(long id)
        {
            await comandoAtividadeAvaliativa.Excluir(id);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.CP_I, Policy = "Bearer")]
        public async Task<IActionResult> PostAsync([FromBody]AtividadeAvaliativaDto atividadeAvaliativaDto)
        {
            await comandoAtividadeAvaliativa.Inserir(atividadeAvaliativaDto);
            return Ok();
        }
    }
}