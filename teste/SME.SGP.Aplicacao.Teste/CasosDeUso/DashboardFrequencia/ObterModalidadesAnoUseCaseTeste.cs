using Moq;
using Xunit;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardFrequencia
{
    public class ObterModalidadesAnoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterModalidadesAnoUseCase _useCase;

        public ObterModalidadesAnoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterModalidadesAnoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveAgruparPorModalidadeEAnoERetornarModalidadeAnoCorreto()
        {
            // Arrange
            int anoLetivo = 2025;
            long dreId = 1;
            long ueId = 123;
            int modalidade = (int)Modalidade.Fundamental;
            int semestre = 2;

            var modalidadesMock = new List<ModalidadesPorAnoDto>
            {
                new ModalidadesPorAnoDto { Modalidade = Modalidade.Fundamental, Ano = 5 },
                new ModalidadesPorAnoDto { Modalidade = Modalidade.Fundamental, Ano = 6 },
                new ModalidadesPorAnoDto { Modalidade = Modalidade.Fundamental, Ano = 5 },
                new ModalidadesPorAnoDto { Modalidade = Modalidade.Medio, Ano = 1 }
            };

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<ObterModalidadesPorAnosQuery>(), 
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(modalidadesMock);

            // Act
            var resultado = await _useCase.Executar(anoLetivo, dreId, ueId, modalidade, semestre);

            // Assert
            Assert.NotNull(resultado);
            var lista = new List<RetornoModalidadesPorAnoDto>(resultado);

            Assert.Equal(3, lista.Count); // Deve agrupar por modalidade e ano

            Assert.Contains(lista, x => x.ModalidadeAno == "EF-5" && x.Ano == 5);
            Assert.Contains(lista, x => x.ModalidadeAno == "EF-6" && x.Ano == 6);
            Assert.Contains(lista, x => x.ModalidadeAno == "EM-1" && x.Ano == 1);

            _mediatorMock.Verify(m => m.Send(
                It.IsAny<ObterModalidadesPorAnosQuery>(), 
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
