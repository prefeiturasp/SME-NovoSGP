using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardInformacoesEscolares
{
    public class ObterModalidadeAnoItineranciaProgramaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterModalidadeAnoItineranciaProgramaUseCase _useCase;

        public ObterModalidadeAnoItineranciaProgramaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterModalidadeAnoItineranciaProgramaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Existem_Dados_Deve_Mapear_E_Ordenar_Corretamente()
        {
            var modalidadesDesordenadas = new List<ModalidadesPorAnoItineranciaProgramaDto>
            {
                new ModalidadesPorAnoItineranciaProgramaDto { Modalidade = Modalidade.Fundamental, Ano = AnoItinerarioPrograma.Seis },
                new ModalidadesPorAnoItineranciaProgramaDto { Modalidade = Modalidade.Medio, Ano = AnoItinerarioPrograma.EducacaoFisica },
                new ModalidadesPorAnoItineranciaProgramaDto { Modalidade = Modalidade.EducacaoInfantil, Ano = AnoItinerarioPrograma.Um }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterModalidadesAnosItineranciaProgramaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(modalidadesDesordenadas);

            var resultado = await _useCase.Executar(2025, 1, 1, 1, 1);
            var resultadoArray = resultado.ToArray();

            Assert.NotNull(resultado);
            Assert.Equal(3, resultadoArray.Length);

            Assert.Equal(1, resultadoArray[0].Ano);
            Assert.Equal("EI-1", resultadoArray[0].ModalidadeAno);

            Assert.Equal(6, resultadoArray[1].Ano);
            Assert.Equal("EF-6", resultadoArray[1].ModalidadeAno);

            Assert.Equal(200, resultadoArray[2].Ano);
            Assert.Equal("Ed. Física", resultadoArray[2].ModalidadeAno);
        }

        [Fact]
        public async Task Executar_Quando_Nao_Existem_Dados_Deve_Retornar_Lista_Vazia()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterModalidadesAnosItineranciaProgramaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<ModalidadesPorAnoItineranciaProgramaDto>());

            var resultado = await _useCase.Executar(2025, 1, 1, 1, 1);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }
    }
}
