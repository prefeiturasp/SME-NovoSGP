using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterComunicadosTotais
{
    public class ObterComunicadosTotaisUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadosTotaisUseCase useCase;

        public ObterComunicadosTotaisUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadosTotaisUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Com_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterComunicadosTotaisUseCase(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_Query_Correta()
        {
            var anoLetivo = 2025;
            var codigoDre = "DRE01";
            var codigoUe = "UE01";

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosTotaisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new ComunicadosTotaisResultado());

            await useCase.Executar(anoLetivo, codigoDre, codigoUe);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComunicadosTotaisQuery>(q =>
                    q.AnoLetivo == anoLetivo &&
                    q.CodigoDre == codigoDre &&
                    q.CodigoUe == codigoUe),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Resultados_Corretamente()
        {
            var anoLetivo = 2025;
            var codigoDre = "DRE01";
            var codigoUe = "UE01";

            var resultadoEsperado = new ComunicadosTotaisResultado
            {
                TotalComunicadosVigentes = 12,
                TotalComunicadosExpirados = 7
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosTotaisQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(anoLetivo, codigoDre, codigoUe);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.TotalComunicadosVigentes, resultado.TotalComunicadosVigentes);
            Assert.Equal(resultadoEsperado.TotalComunicadosExpirados, resultado.TotalComunicadosExpirados);
        }
    }
}
