using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardRegistroIndividual
{
    public class ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase _useCase;

        public ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterDashboardQuantidadeDeAlunosSemRegistroPorPeriodoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Contem_UeId_Deve_Chamar_Query_Por_Ue()
        {
            var diasParametro = 15;
            var dataInicialEsperada = DateTime.Today.AddDays(-diasParametro);
            var parametro = new ParametrosSistema { Valor = diasParametro.ToString() };
            var filtro = new FiltroDasboardRegistroIndividualDTO { UeId = 10, AnoLetivo = 2025 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametro);

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery>(q => q.DataInicial == dataInicialEsperada), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Filtro_Nao_Contem_UeId_Deve_Chamar_Query_Por_Ano()
        {
            var diasParametro = 10;
            var dataInicialEsperada = DateTime.Today.AddDays(-diasParametro);
            var parametro = new ParametrosSistema { Valor = diasParametro.ToString() };
            var filtro = new FiltroDasboardRegistroIndividualDTO { UeId = 0, AnoLetivo = 2025 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametro);

            await _useCase.Executar(filtro);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterQuantidadeDeAunosSemRegistroPorPeriodoAnoQuery>(q => q.DataInicicial == dataInicialEsperada), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterQuantidadeDeAunosSemRegistroPorPeriodoUeQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
