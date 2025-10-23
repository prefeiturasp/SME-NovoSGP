using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterProficienciaIdep;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasProficienciaIdepPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasProficienciaIdepPainelEducacionalUseCase useCase;

        public ConsultasProficienciaIdepPainelEducacionalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasProficienciaIdepPainelEducacionalUseCase(mediatorMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async Task Executar_Quando_Codigo_Ue_Invalido_Deve_Lancar_Negocio_Exception(string codigoUe)
        {
            var anoLetivo = 2025;

            await Assert.ThrowsAsync<NegocioException>(() => useCase.ObterProficienciaIdep(anoLetivo, codigoUe));

            mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<IEnumerable<PainelEducacionalProficienciaIdepDto>>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Codigo_Ue_Valido_Deve_Retornar_Proficiencia()
        {
            var anoLetivo = 2025;
            var codigoUe = "123456";
            var mockRetorno = new List<PainelEducacionalProficienciaIdepDto>
            {
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = anoLetivo }
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterProficienciaIdepQuery>(q => q.AnoLetivo == anoLetivo && q.CodigoUe == codigoUe),
                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(mockRetorno);

            var resultado = await useCase.ObterProficienciaIdep(anoLetivo, codigoUe);

            Assert.NotNull(resultado);
            Assert.Equal(mockRetorno, resultado);
            mediatorMock.Verify(m => m.Send(It.Is<ObterProficienciaIdepQuery>(q => q.AnoLetivo == anoLetivo && q.CodigoUe == codigoUe),
                                            It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Ano_Nao_Informado_Deve_Retornar_Ultimos_5_Anos()
        {
            var anoLetivo = 0; 
            var codigoUe = "123456";
            var mockRetorno = new List<PainelEducacionalProficienciaIdepDto>
            {
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = 2025 },
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = 2024 },
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = 2023 },
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = 2022 },
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = 2021 },
                new PainelEducacionalProficienciaIdepDto { AnoLetivo = 2020 } 
            };

            mediatorMock.Setup(m => m.Send(It.Is<ObterProficienciaIdepQuery>(q => q.AnoLetivo == anoLetivo && q.CodigoUe == codigoUe),
                                           It.IsAny<CancellationToken>()))
                        .ReturnsAsync(mockRetorno);

            var resultado = await useCase.ObterProficienciaIdep(anoLetivo, codigoUe);

            Assert.NotNull(resultado);
            Assert.Equal(5, resultado.Count()); 
            Assert.Collection(resultado,
                item => Assert.Equal(2025, item.AnoLetivo),
                item => Assert.Equal(2024, item.AnoLetivo),
                item => Assert.Equal(2023, item.AnoLetivo),
                item => Assert.Equal(2022, item.AnoLetivo),
                item => Assert.Equal(2021, item.AnoLetivo)); 

            mediatorMock.Verify(m => m.Send(It.Is<ObterProficienciaIdepQuery>(q => q.AnoLetivo == anoLetivo && q.CodigoUe == codigoUe),
                                            It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

