
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/abae")]
    [ValidaDto]
    //[Authorize("Bearer")]
    public class ABAEController : ControllerBase
    {
        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(CadastroAcessoABAEDto), 200)]
        [Permissao(Permissao.ABA_I, Policy = "Bearer")]
        public async Task<IActionResult> Incluir([FromBody] CadastroAcessoABAEDto cadastroAcessoAbaeDto, [FromServices] ISalvarCadastroAcessoABAEUseCase salvarCadastroAcessoAbaeUse)
        {
            return Ok(await salvarCadastroAcessoAbaeUse.Executar(cadastroAcessoAbaeDto));
        }
        
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(CadastroAcessoABAEDto), 200)]
        [Permissao(Permissao.ABA_A, Policy = "Bearer")]
        public async Task<IActionResult> Alterar([FromBody] CadastroAcessoABAEDto cadastroAcessoAbaeDto, [FromServices] ISalvarCadastroAcessoABAEUseCase salvarCadastroAcessoAbaeUse)
        {
            return Ok(await salvarCadastroAcessoAbaeUse.Executar(cadastroAcessoAbaeDto));
        }
        
        [HttpGet("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(CadastroAcessoABAEDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ABA_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPorId(int id, [FromServices] IObterCadastroAcessoABAEUseCase obterCadastroAcessoAbaeUse)
        {
            return Ok(await obterCadastroAcessoAbaeUse.Executar(id));
        }
        
        [HttpDelete("{id}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ABA_E, Policy = "Bearer")]
        public async Task<IActionResult> Excluir([FromRoute] long id, [FromServices] IExcluirCadastroAcessoABAEUseCase excluirCadastroAcessoABAEUseCase)
        {
            return Ok(await excluirCadastroAcessoABAEUseCase.Executar(id));
        }
        
        [HttpGet]
        [ProducesResponseType(204)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<DreUeNomeSituacaoABAEDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.ABA_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarPaginada([FromQuery] FiltroDreIdUeIdNomeSituacaoABAEDto filtro, [FromServices] IObterPaginadoCadastroAcessoABAEUseCase obterPaginadoCadastroAcessoAbaeUseCase)
        {
            return Ok(await obterPaginadoCadastroAcessoAbaeUseCase.Executar(filtro));
        }

        [Route("dres/{codigoDre}/ues/{codigoUe}/funcionarios")]
        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.ABA_C, Permissao.RBA_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterFuncionarios(string codigoDre, string codigoUe, 
                                                            [FromQuery] string codigoRf, [FromQuery] string nomeServidor,
                                                           [FromServices] IObterFuncionariosABAEUseCase obterFuncionariosUseCase)
        {
            return Ok(await obterFuncionariosUseCase.Executar(new FiltroFuncionarioDto(codigoDre,
                                                                                       codigoUe,
                                                                                       codigoRf,
                                                                                       nomeServidor)));
        }
    }
}