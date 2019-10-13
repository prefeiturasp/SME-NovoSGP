using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Dominio;
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
        private readonly IComandosAbrangencia comandosAbrangencia;
        private readonly IComandosUsuario comandosUsuario;

        public AutenticacaoController(IComandosUsuario comandosUsuario, IComandosAbrangencia comandosAbrangencia)
        {
            this.comandosUsuario = comandosUsuario ?? throw new System.ArgumentNullException(nameof(comandosUsuario));
            this.comandosAbrangencia = comandosAbrangencia ?? throw new System.ArgumentNullException(nameof(comandosAbrangencia));
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

        [HttpGet("salvar-abrangencia")]
        public async Task<IActionResult> Teste()
        {
            var abrangencia = new AbrangenciaRetornoEolDto();
            var abrangenciaDre = new AbrangenciaDreRetornoEolDto()
            {
                Abreviacao = "TST",
                Codigo = "TSTS",
                Nome = "DRE DE TESTE VIA CODIGO"
            };

            var abrangenciaUe = new AbrangenciaUeRetornoEolDto() { Nome = "ESCOLA TESTE", Codigo = "ET" };
            abrangenciaUe.Turmas.Add(new AbrangenciaTurmaRetornoEolDto() { Ano = 2019, AnoLetivo = 1, Codigo = "TUR AB", NomeTurma = "TURMA AB", CodigoModalidade = "1", Modalidade = "VESPERTINO" });

            abrangenciaDre.Ues.Add(abrangenciaUe);

            abrangencia.Dres.Add(abrangenciaDre);

            await comandosAbrangencia.Salvar(abrangencia, 1);
            return Ok();
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