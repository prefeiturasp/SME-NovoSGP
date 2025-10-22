using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFrequenciaSemanalUe;
using SME.SGP.Infra.Dtos.PainelEducacional.FrequenciaSemanalUe;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasFrequenciaSemanalUeTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsultasFrequenciaSemanalUeUseCase _useCase;

        public ConsultasFrequenciaSemanalUeTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsultasFrequenciaSemanalUeUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_ArgumentNullException()
        {
            var exception = Assert.Throws<ArgumentNullException>(() =>
                new ConsultasFrequenciaSemanalUeUseCase(null)
            );

            Assert.Equal("mediator", exception.ParamName);
        }

        [Fact]
        public async Task Obter_Frequencia_Semanal_Ue_Quando_Chamado_Deve_Invocar_Mediator_Send_E_Retornar_Dados()
        {
            string codigoUe = "094438";
            int anoLetivo = 2024;
            var retornoEsperado = new List<PainelEducacionalFrequenciaSemanalUeDto>
            {
                new PainelEducacionalFrequenciaSemanalUeDto { Data = "2024-10-21", PercentualFrequencia = 98.5m }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterFrequenciaSemanalUeQuery>(q =>
                    q.CodigoUe == codigoUe && q.AnoLetivo == anoLetivo),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.ObterFrequenciaSemanalUe(codigoUe, anoLetivo);

            Assert.NotNull(resultado);
            Assert.Same(retornoEsperado, resultado);
            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterFrequenciaSemanalUeQuery>(q =>
                    q.CodigoUe == codigoUe && q.AnoLetivo == anoLetivo),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
