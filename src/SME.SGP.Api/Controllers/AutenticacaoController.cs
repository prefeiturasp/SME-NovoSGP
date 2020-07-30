using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Route("senha")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Authorize("Bearer")]
        public async Task<IActionResult> AlterarSenha([FromBody]AlterarSenhaDto alterarSenhaDto)
        {
            await comandosUsuario.AlterarSenha(alterarSenhaDto);
            return Ok();
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(TrocaPerfilDto), 200)]
        [Route("perfis/{guid}")]
        [Authorize("Bearer")]
        public async Task<IActionResult> AtualizarPerfil(Guid guid)
        {
            if (guid == Guid.Empty)
                throw new NegocioException("Informe um perfil");

            var retorno = await comandosUsuario.ModificarPerfil(guid);

            return Ok(retorno);
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

        [HttpGet("{login}/perfis/listar")]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(IEnumerable<PrioridadePerfil>), 500)]
        public async Task<IActionResult> ListarPerfisUsuario(string login, [FromServices]IServicoUsuario servicoUsuario)
        {
            var retorno = await servicoUsuario.ObterPerfisUsuario(login);

            if (retorno == null || !retorno.Any())
                return NoContent();

            return Ok(retorno);
        }

        [HttpPost("primeiro-acesso")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> PrimeiroAcesso(PrimeiroAcessoDto primeiroAcessoDto)
        {
            var retornoAlteracao = await comandosUsuario.AlterarSenhaPrimeiroAcesso(primeiroAcessoDto);

            if (!retornoAlteracao.SenhaAlterada)
                return StatusCode(retornoAlteracao.StatusRetorno, retornoAlteracao.Mensagem);

            var retornoAutenticacao = await comandosUsuario.Autenticar(primeiroAcessoDto.Usuario, primeiroAcessoDto.NovaSenha);

            if (!retornoAutenticacao.Autenticado)
                return StatusCode(403);

            return Ok(retornoAutenticacao);
        }

        [HttpPost("recuperar-senha")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> RecuperarSenha([FromForm]RecuperacaoSenhaDto recuperacaoSenhaDto)
        {
           var retorno = await comandosUsuario.AlterarSenhaComTokenRecuperacao(recuperacaoSenhaDto);
            if (!retorno.Autenticado)
                return StatusCode(401);

            return Ok(retorno);
        }

        [HttpPut("{codigoRf}/reiniciar-senha")]
        [ProducesResponseType(typeof(UsuarioReinicioSenhaDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioReinicioSenhaDto), 601)]
        [AllowAnonymous]
        public async Task<IActionResult> ReiniciarSenha([FromServices] IReiniciarSenhaUseCase reiniciarSenhaUseCase, string codigoRf, [FromBody] DreUeDto dreUeDto)
        {
            var retorno = await reiniciarSenhaUseCase.ReiniciarSenha(codigoRf, dreUeDto.DreCodigo, dreUeDto.UeCodigo);

            if (retorno.DeveAtualizarEmail)
                return StatusCode(601, retorno);

            else return Ok(retorno);
        }

        [HttpPost("revalidar")]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(UsuarioReinicioSenhaDto), 200)]
        public async Task<IActionResult> Revalidar()
        {
            var tokenRetorno = await comandosUsuario.RevalidarLogin();

            if (tokenRetorno == null)
                return StatusCode(401);

            return Ok(tokenRetorno);
        }

        [HttpGet("sair")]
        [ProducesResponseType(typeof(bool), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Authorize(Policy = "Bearer")]
        public IActionResult Sair()
        {
            comandosUsuario.Sair();
            return Ok();
        }

        [HttpPost("solicitar-recuperacao-senha")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        public async Task<IActionResult> SolicitarRecuperacaoSenha(string login)
        {
            return Ok(await comandosUsuario.SolicitarRecuperacaoSenha(login));
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