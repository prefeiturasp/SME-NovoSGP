using MediatR;
using Microsoft.Extensions.Configuration;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using PlanoAEEDominio = SME.SGP.Dominio.PlanoAEE;

namespace SME.SGP.Aplicacao.Teste.Commands.PlanoAEE
{
    public class AtribuirResponsavelPlanoAEECommandHandlerTeste
    {
        private const long UsuarioPaaiId = 10;
        private const long UsuarioLogadoId = 20;
        private readonly Mock<IMediator> mediator;
        private readonly Mock<IRepositorioPlanoAEE> repositorioPlanoAEE;
        private readonly Mock<IConfiguration> configuration;
        private readonly Mock<IUnitOfWork> unitOfWork;
        private readonly AtribuirResponsavelPlanoAEECommandHandler handler;

        public AtribuirResponsavelPlanoAEECommandHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            repositorioPlanoAEE = new Mock<IRepositorioPlanoAEE>();
            configuration = new Mock<IConfiguration>();
            unitOfWork = new Mock<IUnitOfWork>();
            handler = new AtribuirResponsavelPlanoAEECommandHandler(
                mediator.Object,
                repositorioPlanoAEE.Object,
                configuration.Object,
                unitOfWork.Object);
        }

        [Fact]
        public async Task Deve_Lancar_NegocioException_Quando_Dre_Nao_Esta_Carregada()
        {
            var plano = CriarPlano(SituacaoPlanoAEE.AtribuicaoPAAI);
            var turma = CriarTurmaComUeEDre();
            turma.Ue.Dre = null;

            var excecao = await Assert.ThrowsAsync<NegocioException>(() =>
                handler.Handle(new AtribuirResponsavelPlanoAEECommand(plano, "7994389", turma), CancellationToken.None));

            Assert.Contains("DRE", excecao.Message);
            mediator.Verify(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            repositorioPlanoAEE.Verify(r => r.SalvarAsync(It.IsAny<PlanoAEEDominio>()), Times.Never);
            unitOfWork.Verify(u => u.IniciarTransacao(), Times.Never);
        }

        [Fact]
        public async Task Deve_Gerar_Pendencia_Quando_Atribuir_Paai_Diferente_Do_Usuario_Logado()
        {
            ConfigurarAtribuicao(gerarPendencia: true, usuarioLogadoId: UsuarioLogadoId);
            var plano = CriarPlano(SituacaoPlanoAEE.AtribuicaoPAAI);
            var turma = CriarTurmaComUeEDre();

            var resultado = await handler.Handle(
                new AtribuirResponsavelPlanoAEECommand(plano, "7994389", turma),
                CancellationToken.None);

            Assert.True(resultado);
            Assert.Equal(SituacaoPlanoAEE.ParecerPAAI, plano.Situacao);
            Assert.Equal(UsuarioPaaiId, plano.ResponsavelPaaiId);
            mediator.Verify(m => m.Send(It.Is<GerarPendenciaPlanoAEECommand>(c =>
                c.PlanoAEEId == plano.Id &&
                c.UeId == turma.UeId &&
                c.TurmaId == turma.Id &&
                c.Titulo.Contains("DRE CS")), It.IsAny<CancellationToken>()), Times.Once);
            unitOfWork.Verify(u => u.PersistirTransacao(), Times.Once);
        }

        [Theory]
        [InlineData(SituacaoPlanoAEE.Validado)]
        [InlineData(SituacaoPlanoAEE.Expirado)]
        public async Task Deve_Preservar_Situacao_E_Nao_Gerar_Pendencia_Para_Plano_Finalizado(SituacaoPlanoAEE situacao)
        {
            ConfigurarAtribuicao(gerarPendencia: true, usuarioLogadoId: UsuarioLogadoId);
            var plano = CriarPlano(situacao);
            var turma = CriarTurmaComUeEDre();

            var resultado = await handler.Handle(
                new AtribuirResponsavelPlanoAEECommand(plano, "7994389", turma),
                CancellationToken.None);

            Assert.True(resultado);
            Assert.Equal(situacao, plano.Situacao);
            Assert.Equal(UsuarioPaaiId, plano.ResponsavelPaaiId);
            mediator.Verify(m => m.Send(It.IsAny<ExcluirPendenciaPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Never);
            mediator.Verify(m => m.Send(It.IsAny<GerarPendenciaPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_Quando_Parametro_Esta_Desativado()
        {
            ConfigurarAtribuicao(gerarPendencia: false, usuarioLogadoId: UsuarioLogadoId);

            await handler.Handle(
                new AtribuirResponsavelPlanoAEECommand(
                    CriarPlano(SituacaoPlanoAEE.AtribuicaoPAAI),
                    "7994389",
                    CriarTurmaComUeEDre()),
                CancellationToken.None);

            mediator.Verify(m => m.Send(It.IsAny<GerarPendenciaPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Nao_Deve_Gerar_Pendencia_Quando_Paai_Eh_Usuario_Logado()
        {
            ConfigurarAtribuicao(gerarPendencia: true, usuarioLogadoId: UsuarioPaaiId);

            await handler.Handle(
                new AtribuirResponsavelPlanoAEECommand(
                    CriarPlano(SituacaoPlanoAEE.AtribuicaoPAAI),
                    "7994389",
                    CriarTurmaComUeEDre()),
                CancellationToken.None);

            mediator.Verify(m => m.Send(It.IsAny<GerarPendenciaPlanoAEECommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        private void ConfigurarAtribuicao(bool gerarPendencia, long usuarioLogadoId)
        {
            mediator
                .Setup(m => m.Send(It.IsAny<ObterUsuarioIdPorRfOuCriaQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(UsuarioPaaiId);
            mediator
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ParametrosSistema { Ativo = gerarPendencia });
            mediator
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogadoId);
            repositorioPlanoAEE
                .Setup(r => r.SalvarAsync(It.IsAny<PlanoAEEDominio>()))
                .ReturnsAsync(1);
            configuration.Setup(c => c["UrlFrontEnd"]).Returns("https://novosgp.test/");
        }

        private static PlanoAEEDominio CriarPlano(SituacaoPlanoAEE situacao)
            => new()
            {
                Id = 1,
                AlunoCodigo = "1234567",
                AlunoNome = "Estudante",
                Situacao = situacao
            };

        private static Turma CriarTurmaComUeEDre()
        {
            var ue = new Ue
            {
                Id = 3,
                Nome = "Jardim Gaivotas",
                TipoEscola = TipoEscola.EMEI
            };
            ue.AdicionarDre(new Dre { Id = 4, Abreviacao = "DRE CS" });

            var turma = new Turma
            {
                Id = 2,
                Nome = "1A",
                ModalidadeCodigo = Modalidade.EducacaoInfantil
            };
            turma.AdicionarUe(ue);
            return turma;
        }
    }
}
