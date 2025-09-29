using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task Executar_Quando_Nao_Encontrar_Consolidacoes_Deve_Finalizar_Sem_Salvar()
        {
            var filtro = new FiltroConsolidacaoDiariosBordoPorUeDto(123456);
            var filtroJson = JsonConvert.SerializeObject(filtro);
            var mensagem = new MensagemRabbit("Test", filtroJson, Guid.NewGuid(), "test_user");

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<SME.SGP.Dominio.ConsolidacaoDiariosBordo>());

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_Quando_Encontrar_Consolidacoes_Deve_Salvar_Cada_Uma_Delas()
        {
            var filtro = new FiltroConsolidacaoDiariosBordoPorUeDto(123456);
            var filtroJson = JsonConvert.SerializeObject(filtro);
            var mensagem = new MensagemRabbit("Test", filtroJson, Guid.NewGuid(), "test_user");

            var consolidacoes = new List<SME.SGP.Dominio.ConsolidacaoDiariosBordo>
            {
                new SME.SGP.Dominio.ConsolidacaoDiariosBordo(),
                new SME.SGP.Dominio.ConsolidacaoDiariosBordo()
            };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(q => q.UeId == filtro.UeId), It.IsAny<CancellationToken>()))
                .ReturnsAsync(consolidacoes);

            var resultado = await _useCase.Executar(mensagem);

            Assert.True(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterConsolidacaoDiariosBordoTurmasPorUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarConsolidacaoDiariosBordoCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(consolidacoes.Count));
        }
    }
}
