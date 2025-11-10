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

        [Fact(DisplayName = "Deve retornar true quando a lista de pend�ncias para excluir for vazia")]
        public async Task Handle_QuandoListaVazia_DeveRetornarTrue()
        {
            // Organiza��o
            var comando = new PendenciaDiarioBordoParaExcluirCommand(new List<PendenciaDiarioBordoParaExcluirDto>());

            // A��o
            var resultado = await _commandHandler.Handle(comando, default);

            // Verifica��o
            resultado.Should().BeTrue();
            _mediatorMock.Verify(m => m.Send(It.IsAny<IRequest<bool>>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        public static IEnumerable<object[]> CasosDeTeste()
        {
            // Cen�rio 1: Todas as exclus�es com sucesso
            yield return new object[] { new[] { true, true, true }, true };

            // Cen�rio 2: Uma das exclus�es falha
            yield return new object[] { new[] { true, false, true }, false };

            // Cen�rio 3: Todas as exclus�es falham
            yield return new object[] { new[] { false, false, false }, false };
        }

        [Theory(DisplayName = "Deve retornar o resultado agregado da exclus�o das pend�ncias")]
        [MemberData(nameof(CasosDeTeste))]
        public async Task Handle_QuandoProcessarLista_DeveRetornarResultadoAgregado(bool[] resultadosDosComandos, bool resultadoFinalEsperado)
        {
            // Organiza��o
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

            // Configura a sequ�ncia de retornos do mediator
            var setupSequencia = _mediatorMock.SetupSequence(m => m.Send(It.IsAny<ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand>(), default));
            foreach (var item in resultadosDosComandos)
            {
                setupSequencia.ReturnsAsync(item);
            }

            // A��o
            var resultado = await _commandHandler.Handle(comando, default);

            // Verifica��o
            resultado.Should().Be(resultadoFinalEsperado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ExcluirPendenciaDiarioBordoPorIdEComponenteIdCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(resultadosDosComandos.Length));
        }
    }
}
