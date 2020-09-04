using Microsoft.AspNetCore.Mvc;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.EscolaAqui.SolicitarReiniciarSenha;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.EscolaAqui;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/escola-aqui/usuarios")]
    //[Authorize("Bearer")]
    public class UsuarioEscolaAquiController : ControllerBase
    {
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CO_A, Policy = "Bearer")]
        public async Task<IActionResult> ReiniciarSenha([FromBody] SolicitarReiniciarSenhaDto solicitarReiniciarSenhaDto, [FromServices] ISolicitarReiniciarSenhaUseCase solicitarReiniciarSenhaUseCase)
        {

            //TODO: Validar Diretor


            await solicitarReiniciarSenhaUseCase.Executar(solicitarReiniciarSenhaDto.Cpf);  
            return Ok();
        }

        [HttpGet("{cpf}")]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 200)]
        [ProducesResponseType(typeof(UsuarioEscolaAquiDto), 204)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        //[Permissao(Permissao.CO_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUsuarioPorCpf(string cpf, [FromServices] IObterUsuarioPorCpfUseCase obterUsuarioPorCpfUseCase)
        {
            return Ok(await obterUsuarioPorCpfUseCase.Executar(cpf));
        }
    }
}