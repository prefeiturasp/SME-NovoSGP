using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoDiariosBordo
{
    public class ConsolidarDiariosBordoPorUeTratarUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsolidarDiariosBordoPorUeTratarUseCase _useCase;

        public ConsolidarDiariosBordoPorUeTratarUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsolidarDiariosBordoPorUeTratarUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Houver_Consolidacoes_Deve_Buscar_E_Salvar_Cada_Uma()
        {
            long ueId = 12345;
            var filtro = new FiltroConsolidacaoDiariosBordoPorUeDto(ueId);
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var listaConsolidacoes = new List<SME.SGP.Dominio.ConsolidacaoDiariosBordo>
            {
                new SME.SGP.Dominio.ConsolidacaoDiariosBordo { Id = 1, TurmaId = 10 },
                new SME.SGP.Dominio.ConsolidacaoDiariosBordo { Id = 2, TurmaId = 20 }
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(q => q.UeId == ueId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaConsolidacoes);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(q => q.UeId == ueId), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(listaConsolidacoes.Count));

            foreach (var consolidacao in listaConsolidacoes)
            {
                _mediatorMock.Verify(m => m.Send(It.Is<SalvarConsolidacaoDiariosBordoCommand>(c => c.ConsolidacaoDiariosBordo.Id == consolidacao.Id), It.IsAny<CancellationToken>()), Times.Once);
            }
        }

        [Fact]
        public async Task Executar_Quando_Nao_Houver_Consolidacoes_Deve_Apenas_Buscar_E_Finalizar()
        {
            long ueId = 67890;
            var filtro = new FiltroConsolidacaoDiariosBordoPorUeDto(ueId);
            var mensagemRabbit = new MensagemRabbit { Mensagem = JsonConvert.SerializeObject(filtro) };

            var listaVazia = new List<SME.SGP.Dominio.ConsolidacaoDiariosBordo>();

            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(q => q.UeId == ueId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(listaVazia);

            var resultado = await _useCase.Executar(mensagemRabbit);

            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(q => q.UeId == ueId), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
