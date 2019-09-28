using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dto;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/autenticacao")]
    [ValidaDto]
    public class AutenticacaoController : ControllerBase
    {
        private readonly IComandosUsuario comandosUsuario;

        public AutenticacaoController(IComandosUsuario comandosUsuario)
        {
            this.comandosUsuario = comandosUsuario ?? throw new System.ArgumentNullException(nameof(comandosUsuario));
        }

        [HttpPost("alterar-senha")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous]
        public async Task<IActionResult> AlterarSenha(Guid token, string novaSenha, [FromServices]IComandosAutenticacao comandosAutenticacao)
        {
            await comandosAutenticacao.AlterarSenhaComTokenRecuperacao(token, novaSenha);
            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDto), 200)]
        public async Task<IActionResult> Autenticar(AutenticacaoDto autenticacaoDto)
        {
            var retornoAutenticacao = await comandosUsuario.Autenticar(autenticacaoDto.Login, autenticacaoDto.Senha);

            if (!retornoAutenticacao.Autenticado)
                return StatusCode(401);

            return Ok(retornoAutenticacao);
        }

        [HttpPost("primeiro-acesso")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> PrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto)
        {
            var retornoAlteracao = await servicoAutenticacao.AlterarSenhaPrimeiroAcesso(primeiroAcessoDto);

            if (!retornoAlteracao.SenhaAlterada)
                return StatusCode(retornoAlteracao.StatusRetorno, retornoAlteracao.Mensagem);

            return Ok();
        }

        [HttpPost("recuperar-senha")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult SolicitarRecuperacaoSenha(string login, [FromServices]IComandosAutenticacao comandosAutenticacao)
        {
            return Ok(comandosAutenticacao.SolicitarRecuperacaoSenha(login));
        }

        [HttpPost("valida-token-recuperacao-senha")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous]
        public IActionResult TokenRecuperacaoSenhaEstaValido(Guid token, [FromServices]IComandosAutenticacao comandosAutenticacao)
        {
            return Ok(comandosAutenticacao.TokenRecuperacaoSenhaEstaValido(token));
        }
    }
}