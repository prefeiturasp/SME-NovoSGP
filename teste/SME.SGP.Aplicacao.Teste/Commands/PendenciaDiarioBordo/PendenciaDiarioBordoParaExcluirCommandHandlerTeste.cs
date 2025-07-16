using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Commands.PendenciaDiarioBordo
{
    public class PendenciaDiarioBordoParaExcluirCommandHandlerTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaDiarioBordoParaExcluirCommandHandler _commandHandler;
        private readonly Faker _faker;

        public PendenciaDiarioBordoParaExcluirCommandHandlerTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _commandHandler = new PendenciaDiarioBordoParaExcluirCommandHandler(_mediatorMock.Object);
            _faker = new Faker("pt_BR");
        }

        [Fact(DisplayName = "Deve retornar true quando a lista de pendências para excluir for vazia")]
        public async Task Handle_QuandoListaVazia_DeveRetornarTrue()
        {
            // Organização
            var comando = new PendenciaDiarioBordoParaExcluirCommand(new List<PendenciaDiarioBordoParaExcluirDto>());

            // Ação
            var resultado = await _commandHandler.Handle(comando, default);

            // Verificação
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        public static IEnumerable<object[]> CasosDeTeste()
        {
            // Cenário 1: Todas as exclusões com sucesso
            yield return new object[] { new[] { true, true, true }, true };

            // Cenário 2: Uma das exclusões falha
            yield return new object[] { new[] { true, false, true }, false };

            // Cenário 3: Todas as exclusões falham
            yield return new object[] { new[] { false, false, false }, false };
        }

        [Theory(DisplayName = "Deve retornar o resultado agregado da exclusão das pendências")]
        [MemberData(nameof(CasosDeTeste))]
        public async Task Handle_QuandoProcessarLista_DeveRetornarResultadoAgregado(bool[] resultadosDosComandos, bool resultadoFinalEsperado)
        {
            // Organização
            var listaDePendencias = new List<PendenciaDiarioBordoParaExcluirDto>();
            for (int i = 0; i < resultadosDosComandos.Length; i++)
            {
                listaDePendencias.Add(new PendenciaDiarioBordoParaExcluirDto
                {
                    AulaId = _faker.Random.Long(1, 1000),
                    ComponenteCurricularId = _faker.Random.Long(1, 1000)
                });
            }

            var comando = new PendenciaDiarioBordoParaExcluirCommand(listaDePendencias);

            // Configura a sequência de retornos do mediator
            var setupSequencia = _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand>(), default));
            foreach (var item in resultadosDosComandos)
            {
                setupSequencia.ReturnsAsync(item);
            }

            // Ação
            var resultado = await _commandHandler.Handle(comando, default);

            // Verificação
            resultado.Should().Be(resultadoFinalEsperado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(resultadosDosComandos.Length));
        }
    }
}
