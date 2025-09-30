using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoDiariosBordo
{
    public class ConsolidarDiariosBordoCarregarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarDiariosBordoCarregarUseCase _useCase;

        public ConsolidarDiariosBordoCarregarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarDiariosBordoCarregarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Consolidacao_Inativa_Deve_Retornar_True_Sem_Executar_Processo()
        {
            var parametroInativo = new ParametrosSistema { Ativo = false };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroInativo);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverConsolidacoesDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Never);
            _mediatorMock.Verify(m => m.Send(It.IsAny<AtualizarParametroSistemaCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Consolidacao_Ativa_Deve_Executar_Todo_O_Processo_Corretamente()
        {
            var parametroAtivo = new ParametrosSistema { Ativo = true, Valor = "data_antiga" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroAtivo);

            var uesIds = new List<long> { 1, 2, 3 };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(uesIds);

            var resultado = await _useCase.Executar(new MensagemRabbit());

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<RemoverConsolidacoesDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<PublicarFilaSgpCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(uesIds.Count));

            foreach (var ueId in uesIds)
            {
                _mediatorMock.Verify(m => m.Send(
                    It.Is<PublicarFilaSgpCommand>(cmd => (cmd.Filtros as FiltroConsolidacaoDiariosBordoPorUeDto).UeId == ueId),
                    It.IsAny<CancellationToken>()),
                Times.Once);
            }

            _mediatorMock.Verify(m => m.Send(
                It.Is<AtualizarParametroSistemaCommand>(cmd =>
                    cmd.Parametro == parametroAtivo &&
                    !string.IsNullOrWhiteSpace(cmd.Parametro.Valor) &&
                    cmd.Parametro.Valor != "data_antiga"),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }
    }
}
