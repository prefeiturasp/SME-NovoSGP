using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dto;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/autenticacao")]
    [ValidaDto]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IServicoAutenticacao servicoAutenticacao;

        public AutenticacaoController(IServicoAutenticacao servicoAutenticacao)
        {
            this.servicoAutenticacao = servicoAutenticacao ?? throw new System.ArgumentNullException(nameof(servicoAutenticacao));
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDto), 200)]
        public async Task<IActionResult> Autenticar(AutenticacaoDto autenticacaoDto)
        {
            var retornoAutenticacao = await servicoAutenticacao.AutenticarNoEol(autenticacaoDto.Login, autenticacaoDto.Senha);

            if (!retornoAutenticacao.Autenticado)
                return StatusCode(401);

            return Ok(retornoAutenticacao);
        }

        [HttpPost("recuperar-senha")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult RecuperarSenha(string codigo, [FromServices]IComandoAuth comandoAuth)
        {
            return Ok(comandoAuth.RecuperarSenha(codigo));
        }
    }
}