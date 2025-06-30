using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Relatorio
{
    public class GerarRelatorioProdutividadeFrequenciaUseCaseTeste
    {
        [Fact]
        public async Task Deve_Gerar_Relatorio_Produtividade_Frequencia()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new GerarRelatorioProdutividadeFrequenciaUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioProdutividadeFrequenciaDto
            {
                AnoLetivo = 2024,
                CodigoDre = "DRE01",
                CodigoUe = "UE01",
                Bimestre = 1,
                RfProfessor = "123456",
                TipoRelatorioProdutividade = TipoRelatorioProdutividadeFrequencia.MédiaPorProfessor
            };

            var usuario = new Usuario
            {
                Nome = "Usuário Teste",
                CodigoRf = "654321"
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(usuario);

            mediatorMock
                .Setup(m => m.Send(It.IsAny<GerarRelatorioCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            var resultado = await useCase.Executar(filtro);

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            mediatorMock.Verify(m => m.Send(It.Is<GerarRelatorioCommand>(c =>
                c.TipoRelatorio == TipoRelatorio.ProdutividadeFrequencia &&
                c.Filtros == filtro &&
                c.Formato == TipoFormatoRelatorio.Xlsx &&
                c.RotaRelatorio == RotasRabbitSgpRelatorios.RotaRelatoriosSolicitadosProdutividadeFrequencia
            ), It.IsAny<CancellationToken>()), Times.Once);

            Assert.Equal(usuario.Nome, filtro.UsuarioNome);
            Assert.Equal(usuario.CodigoRf, filtro.UsuarioRf);
            Assert.True(resultado);
        }

        [Fact]
        public async Task Deve_Lancar_Excecao_Quando_Usuario_Nulo()
        {
            var mediatorMock = new Mock<IMediator>();
            var useCase = new GerarRelatorioProdutividadeFrequenciaUseCase(mediatorMock.Object);

            var filtro = new FiltroRelatorioProdutividadeFrequenciaDto();

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterUsuarioLogadoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((Usuario)null);

            await Assert.ThrowsAsync<NegocioException>(() => useCase.Executar(filtro));
        }
    }
}
