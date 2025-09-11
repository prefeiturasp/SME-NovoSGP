using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class RelatorioConselhoClasseAtaFinalUseCaseTeste
    {
        [Fact]
        public async Task Deve_Gerar_Relatorio_Conselho_Classe_Ata_Final()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new RelatorioConselhoClasseAtaFinalUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioConselhoClasseAtaFinalDto
            {
                TurmasCodigos = new List<string> { "123", "-99", "456" },
                TipoFormatoRelatorio = TipoFormatoRelatorio.Pdf,
                Visualizacao = AtaFinalTipoVisualizacao.Turma,
                AnoLetivo = 2024,
                Semestre = 1,
                ImprimirComponentesQueNaoLancamNota = false
            };

            var usuarioLogado = new Usuario
            {
                Nome = "Usuário Teste",
                CodigoRf = "123456"
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuarioLogado);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(filtro);

            mediatorMock.Verify(m => m.Send(It.Is<GerarRelatorioCommand>(c =>
            c.TipoRelatorio == TipoRelatorio.ConselhoClasseAtaFinal &&
            c.Filtros == filtro &&
            c.IdUsuarioLogado == usuarioLogado.Id &&
            c.UsuarioLogadoRf == usuarioLogado.ObterCodigoRfLogin() &&
            c.PerfilUsuario == usuarioLogado.PerfilAtual.ToString() &&
            c.Formato == filtro.TipoFormatoRelatorio &&
            c.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosAtaFinalResultados
           ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.True(resultado);
            Assert.DoesNotContain("-99", filtro.TurmasCodigos);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nulo()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new RelatorioConselhoClasseAtaFinalUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioConselhoClasseAtaFinalDto
            {
                TurmasCodigos = new List<string> { "123456" },
                TipoFormatoRelatorio = TipoFormatoRelatorio.Pdf,
                Visualizacao = AtaFinalTipoVisualizacao.Turma,
                AnoLetivo = 2024,
                Semestre = 1,
                ImprimirComponentesQueNaoLancamNota = false
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
        }
    }
}
