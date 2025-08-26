using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PeriodoFechamento
{
    public class ObterPeriodoFechamentoVigenteUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterPeriodoFechamentoVigenteUseCase useCase;

        public ObterPeriodoFechamentoVigenteUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterPeriodoFechamentoVigenteUseCase(mediatorMock.Object);
        }

        [Fact(DisplayName = "Executar deve retornar o período de fechamento vigente do mediator")]
        public async Task Deve_Executar_Com_Sucesso()
        {
            var filtro = new FiltroPeriodoFechamentoVigenteDto
            {
                AnoLetivo = 2025,
                ModalidadeTipoCalendario = ModalidadeTipoCalendario.FundamentalMedio
            };

            var esperado = new PeriodoFechamentoVigenteDto
            {
                Calendario = "2025 - Fundamental",
                AnoLetivo = 2025,
                Bimestre = 1,
                PeriodoFechamentoInicio = new DateTime(2025, 3, 1),
                PeriodoFechamentoFim = new DateTime(2025, 3, 10),
                PeriodoEscolarInicio = new DateTime(2025, 2, 1),
                PeriodoEscolarFim = new DateTime(2025, 6, 30)
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterPeriodoFechamentoVigentePorAnoModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(esperado);

            var resultado = await useCase.Executar(filtro);

            Assert.NotNull(resultado);
            Assert.Equal(esperado.AnoLetivo, resultado.AnoLetivo);
            Assert.Equal(esperado.Bimestre, resultado.Bimestre);
            Assert.Equal(esperado.Calendario, resultado.Calendario);
            Assert.Equal(esperado.PeriodoFechamentoInicio, resultado.PeriodoFechamentoInicio);
            Assert.Equal(esperado.PeriodoFechamentoFim, resultado.PeriodoFechamentoFim);
            Assert.Equal(esperado.PeriodoEscolarInicio, resultado.PeriodoEscolarInicio);
            Assert.Equal(esperado.PeriodoEscolarFim, resultado.PeriodoEscolarFim);

            mediatorMock.Verify(m => m.Send(It.Is<ObterPeriodoFechamentoVigentePorAnoModalidadeQuery>(
                q => q.AnoLetivo == filtro.AnoLetivo &&
                     q.ModalidadeTipoCalendario == filtro.ModalidadeTipoCalendario),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
