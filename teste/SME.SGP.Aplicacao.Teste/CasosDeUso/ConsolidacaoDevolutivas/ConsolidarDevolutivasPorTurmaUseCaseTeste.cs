using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoDevolutivas
{
    public class ConsolidarDevolutivasPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsolidarDevolutivasPorTurmaUseCase useCase;

        public ConsolidarDevolutivasPorTurmaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsolidarDevolutivasPorTurmaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_True_Quando_Tudo_Ok()
        {
            var filtro = new FiltroDevolutivaTurmaDTO(turmaId: 123, anoLetivo: 2024);

            var mensagem = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(filtro)
            };

            var diarioBordoDto = new QuantidadeDiarioBordoRegistradoPorAnoletivoTurmaDTO
            {
                TurmaId = filtro.TurmaId,
                QtdeRegistradaDevolutivas = 5,
                QtdeDiarioBordoRegistrados = 10
            };

            var consolidacaoExistente = new SME.SGP.Dominio.ConsolidacaoDevolutivas(filtro.TurmaId, 0, 0);

            mediatorMock.Setup(m => m.Send(It.Is<ObterDiariosDeBordoComDevolutivasPorAnoLetivoTurmaQuery>(
                    q => q.TurmaId == filtro.TurmaId && q.AnoLetivo == filtro.AnoLetivo), It.IsAny<CancellationToken>()))
                .ReturnsAsync(diarioBordoDto);

            mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacaoDevolutivasPorTurmaIdQuery>(
                    q => q.TurmaId == filtro.TurmaId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(consolidacaoExistente);

            mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoQuery>(
                    q => q.Tipo == Dominio.TipoParametroSistema.PeriodoDeDiasDevolutiva), It.IsAny<CancellationToken>()))
                .ReturnsAsync("5");

            mediatorMock.Setup(m => m.Send(It.IsAny<RegistraConsolidacaoDevolutivasTurmaCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Unit.Value)
                .Verifiable();

            var resultado = await useCase.Executar(mensagem);

            resultado.Should().BeTrue();
            mediatorMock.Verify(m => m.Send(It.IsAny<RegistraConsolidacaoDevolutivasTurmaCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
