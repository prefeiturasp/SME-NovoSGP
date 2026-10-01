using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Integracoes;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Comandos
{
    public class ComandosUsuarioAutenticarTeste
    {
        private const string LOGIN = "45773413876";
        private const string CHAVE_CACHE = "login:45773413876";
        private const AutenticacaoStatusEol STATUS_SENHA_ERRADA = (AutenticacaoStatusEol)2;

        private readonly ComandosUsuario comandosUsuario;
        private readonly Mock<IServicoAutenticacao> servicoAutenticacao;
        private readonly Mock<IRepositorioCache> repositorioCache;
        private readonly Mock<IMediator> mediator;

        public ComandosUsuarioAutenticarTeste()
        {
            servicoAutenticacao = new Mock<IServicoAutenticacao>();
            repositorioCache = new Mock<IRepositorioCache>();
            mediator = new Mock<IMediator>();

            comandosUsuario = new ComandosUsuario(servicoAutenticacao.Object, new Mock<IServicoUsuario>().Object, new Mock<IServicoPerfil>().Object,
                repositorioCache.Object, new Mock<IServicoAbrangencia>().Object, new Mock<IRepositorioHistoricoEmailUsuario>().Object,
                new Mock<IRepositorioSuporteUsuario>().Object, mediator.Object);
        }

        [Fact(DisplayName = "Autenticar - Senha incorreta não deve retornar o login em cache")]
        public async Task Senha_incorreta_nao_deve_retornar_login_em_cache()
        {
            ConfigurarStatusEol(STATUS_SENHA_ERRADA);
            ConfigurarCache(CriarRetornoEmCache(modificarSenha: false));

            var retorno = await comandosUsuario.Autenticar(LOGIN, "senhaErrada");

            Assert.False(retorno.Autenticado);
            Assert.Null(retorno.Token);
            repositorioCache.Verify(r => r.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
            servicoAutenticacao.Verify(s => s.AutenticarNoEol(It.IsAny<AutenticacaoApiEolDto>()), Times.Never);
        }

        [Fact(DisplayName = "Autenticar - Senha correta deve retornar o login em cache")]
        public async Task Senha_correta_deve_retornar_login_em_cache()
        {
            var retornoEmCache = CriarRetornoEmCache(modificarSenha: false);
            ConfigurarStatusEol(AutenticacaoStatusEol.Ok);
            ConfigurarCache(retornoEmCache);

            var retorno = await comandosUsuario.Autenticar(LOGIN, "senhaCorreta");

            Assert.True(retorno.Autenticado);
            Assert.Equal(retornoEmCache.Token, retorno.Token);
            servicoAutenticacao.Verify(s => s.AutenticarNoEol(It.IsAny<AutenticacaoApiEolDto>()), Times.Never);
        }

        [Fact(DisplayName = "Autenticar - Cache com modificar senha deve ser ignorado após troca de senha")]
        public async Task Cache_com_modificar_senha_deve_ser_ignorado()
        {
            ConfigurarStatusEol(AutenticacaoStatusEol.Ok);
            ConfigurarCache(CriarRetornoEmCache(modificarSenha: true));
            ConfigurarAutenticacaoEolNaoAutenticada();

            await comandosUsuario.Autenticar(LOGIN, "novaSenha");

            servicoAutenticacao.Verify(s => s.AutenticarNoEol(It.IsAny<AutenticacaoApiEolDto>()), Times.Once);
        }

        [Fact(DisplayName = "Autenticar - Senha padrão não deve ler o cache")]
        public async Task Senha_padrao_nao_deve_ler_cache()
        {
            ConfigurarStatusEol(AutenticacaoStatusEol.SenhaPadrao);
            ConfigurarCache(CriarRetornoEmCache(modificarSenha: false));
            ConfigurarAutenticacaoEolNaoAutenticada();

            await comandosUsuario.Autenticar(LOGIN, "Sgp3876");

            repositorioCache.Verify(r => r.Obter(It.IsAny<string>(), It.IsAny<bool>()), Times.Never);
            servicoAutenticacao.Verify(s => s.AutenticarNoEol(It.IsAny<AutenticacaoApiEolDto>()), Times.Once);
        }

        [Fact(DisplayName = "Autenticar - Login não autenticado não deve ser salvo em cache")]
        public async Task Login_nao_autenticado_nao_deve_ser_salvo_em_cache()
        {
            ConfigurarStatusEol(AutenticacaoStatusEol.Ok);
            ConfigurarAutenticacaoEolNaoAutenticada();

            await comandosUsuario.Autenticar(LOGIN, "senhaCorreta");

            repositorioCache.Verify(r => r.SalvarAsync(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<int>(), It.IsAny<bool>()), Times.Never);
        }

        private void ConfigurarStatusEol(AutenticacaoStatusEol status)
        {
            mediator.Setup(m => m.Send(It.IsAny<AutenticarQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new AutenticacaoApiEolDto { CodigoRf = LOGIN, Status = status });
        }

        private void ConfigurarCache(UsuarioAutenticacaoRetornoDto retornoEmCache)
        {
            repositorioCache.Setup(r => r.Obter(CHAVE_CACHE, It.IsAny<bool>()))
                .Returns(JsonConvert.SerializeObject(retornoEmCache));
        }

        private void ConfigurarAutenticacaoEolNaoAutenticada()
        {
            servicoAutenticacao.Setup(s => s.AutenticarNoEol(It.IsAny<AutenticacaoApiEolDto>()))
                .ReturnsAsync((new UsuarioAutenticacaoRetornoDto(), LOGIN, new List<Guid>(), false, false));
        }

        private static UsuarioAutenticacaoRetornoDto CriarRetornoEmCache(bool modificarSenha)
        {
            return new UsuarioAutenticacaoRetornoDto
            {
                Autenticado = true,
                ModificarSenha = modificarSenha,
                Token = CriarTokenExpirandoEm(DateTimeOffset.UtcNow.AddDays(1))
            };
        }

        private static string CriarTokenExpirandoEm(DateTimeOffset expiracao)
        {
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{{\"exp\":{expiracao.ToUnixTimeSeconds()}}}"))
                .TrimEnd('=').Replace('+', '-').Replace('/', '_');

            return $"cabecalho.{payload}.assinatura";
        }
    }
}
