using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SME.SGP.Api.Filtros;
using SME.SGP.Aplicacao;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.Usuarios;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Api.Controllers
{
    [ApiController]
    [Route("api/v1/usuarios")]
    [Authorize("Bearer")]
    public class UsuarioController : ControllerBase
    {
        private readonly IComandosUsuario comandosUsuario;
        private readonly IConsultasUsuario consultasUsuario;

        public UsuarioController(IComandosUsuario comandosUsuario, IConsultasUsuario consultasUsuario)
        {
            this.comandosUsuario = comandosUsuario;
            this.consultasUsuario = consultasUsuario;
        }

        [Route("{codigoRf}/email")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.M_A, Policy = "Bearer")]
        public async Task<IActionResult> AlterarEmail([FromBody] AlterarEmailDto novoEmail, string codigoRf)
        {
            await comandosUsuario.AlterarEmail(novoEmail, codigoRf);
            return Ok();
        }

        [Route("autenticado/email")]
        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.M_A, Policy = "Bearer")]
        public async Task<IActionResult> AlterarEmailUsuarioLogado([FromBody] AlterarEmailDto alterarEmailDto)
        {
            await comandosUsuario.AlterarEmailUsuarioLogado(alterarEmailDto.NovoEmail);
            return Ok();
        }

        [HttpGet("situacoes")]
        [ProducesResponseType(typeof(IEnumerable<KeyValuePair<int, string>>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.M_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarSituacoes([FromServices] IObterListaSituacoesUsuarioUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [HttpGet("perfis")]
        [ProducesResponseType(typeof(IEnumerable<SituacaoUsuario>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.M_C, Policy = "Bearer")]
        public async Task<IActionResult> ListarPerfis([FromServices] IObterHierarquiaPerfisUsuarioUseCase useCase)
        {
            return Ok(await useCase.Executar());
        }

        [Route("meus-dados")]
        [HttpGet]
        [ProducesResponseType(typeof(MeusDadosDto), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [ProducesResponseType(typeof(RetornoBaseDto), 601)]
        [Permissao(Permissao.M_C, Policy = "Bearer")]
        public async Task<IActionResult> BuscarMeusDados(string login)
        {
            return Ok(await consultasUsuario.BuscarMeusDados());
        }

        [Route("imagens/perfil")]
        [HttpPost]
        [Permissao(Permissao.M_A, Policy = "Bearer")]
        public IActionResult ModificarImagem([FromBody] ImagemPerfilDto imagemPerfilDto)
        {
            return Ok("https://telegramic.org/media/avatars/stickers/52cae315e8a464eb80a3.png");
        }

        [Route("sme")]
        [HttpGet]
        [ProducesResponseType(typeof(PaginacaoResultadoDto<UsuarioCoreSsoDto>), 200)]
        [ProducesResponseType(typeof(RetornoBaseDto), 500)]
        [Permissao(Permissao.AS_C, Policy = "Bearer")]
        public async Task<IActionResult> ObterUsuariosSme([FromServices] IObterUsuariosCoreSsoUseCase obterUsuariosCoreSsoUseCase,
                                                          [FromQuery] string rf = null,
                                                          [FromQuery] string nome = null,
                                                          [FromQuery] int pagina = 1,
                                                          [FromQuery] int registrosPorPagina = 10)
        {
            var resultado = await obterUsuariosCoreSsoUseCase.Executar(rf, nome, pagina, registrosPorPagina);
            return Ok(resultado);
        }
    }
}