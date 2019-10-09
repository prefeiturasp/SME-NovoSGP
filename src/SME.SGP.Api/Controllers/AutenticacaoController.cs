using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Infra;
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

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioAutenticacaoRetornoDto), 200)]
        [Route("perfis/{guid}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> AtualizarPerfil(string guid)
        {
            if (string.IsNullOrEmpty(guid))
                throw new NegocioException("Informe um perfil");

            var retornoAutenticacao = await comandosUsuario.ModificarPerfil(guid);

            return Ok(retornoAutenticacao);
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

        [HttpPost("recuperar-senha")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> RecuperarSenha([FromForm]RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
            await comandosUsuario.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto);
            return Ok();
        }

        [HttpPut("{codigoRf}/reiniciar-senha")]
        [ProducesResponseType(typeof(UsuarioReinicioSenhaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioReinicioSenhaDto), 601)]
        [AllowAnonymous]
        public async Task<IActionResult> ReiniciarSenha(string codigoRf)
        {
            if (string.IsNullOrEmpty(codigoRf))
                throw new NegocioException("Informe o Código Rf.");

            var retorno = await comandosUsuario.ReiniciarSenha(codigoRf);
            if (retorno.DeveAtualizarEmail)
                return StatusCode(601, retorno);
            else return Ok(retorno);
        }

        [HttpPost("solicitar-recuperacao-senha")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public IActionResult SolicitarRecuperacaoSenha(string login)
        {
            return Ok(comandosUsuario.SolicitarRecuperacaoSenha(login));
        }

        [HttpGet("valida-token-recuperacao-senha/{token}")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [AllowAnonymous]
        public IActionResult TokenRecuperacaoSenhaEstaValido(Guid token)
        {
            return Ok(comandosUsuario.TokenRecuperacaoSenhaEstaValido(token));
        }
    }
}