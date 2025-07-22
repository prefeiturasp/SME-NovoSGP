using MediatR;
using Moq;
using SME.SGP.Infra.Dtos.EscolaAqui.Dashboard;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Dashboard.ObterComunicadosTotais
{
    public class ObterComunicadosTotaisAgrupadosPorDreUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterComunicadosTotaisAgrupadosPorDreUseCase useCase;

        public ObterComunicadosTotaisAgrupadosPorDreUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterComunicadosTotaisAgrupadosPorDreUseCase(mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Com_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterComunicadosTotaisAgrupadosPorDreUseCase(null));
        }

        [Fact]
        public async Task Executar_Deve_Chamar_Mediator_Com_AnoLetivo_Correto()
        {
            var anoLetivo = 2025;

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosTotaisAgrupadosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<ComunicadosTotaisPorDreResultado>());

            await useCase.Executar(anoLetivo);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterComunicadosTotaisAgrupadosPorDreQuery>(q => q.AnoLetivo == anoLetivo),
                It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Dados_Do_Mediator()
        {
            var anoLetivo = 2025;

            var resultadoEsperado = new List<ComunicadosTotaisPorDreResultado>
            {
                new ComunicadosTotaisPorDreResultado
                {
                    NomeAbreviadoDre = "DRE01",
                    TotalComunicadosVigentes = 10,
                    TotalComunicadosExpirados = 5
                },
                new ComunicadosTotaisPorDreResultado
                {
                    NomeAbreviadoDre = "DRE02",
                    TotalComunicadosVigentes = 8,
                    TotalComunicadosExpirados = 3
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComunicadosTotaisAgrupadosPorDreQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await useCase.Executar(anoLetivo);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado.Count, ((List<ComunicadosTotaisPorDreResultado>)resultado).Count);
            Assert.Collection(resultado,
                item =>
                {
                    Assert.Equal("DRE01", item.NomeAbreviadoDre);
                    Assert.Equal(10, item.TotalComunicadosVigentes);
                    Assert.Equal(5, item.TotalComunicadosExpirados);
                },
                item =>
                {
                    Assert.Equal("DRE02", item.NomeAbreviadoDre);
                    Assert.Equal(8, item.TotalComunicadosVigentes);
                    Assert.Equal(3, item.TotalComunicadosExpirados);
                });
        }
    }
}
