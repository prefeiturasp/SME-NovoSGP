using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaRegistroIndividualJobs
{
    public class AtualizarPendenciaRegistroIndividualUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IAtualizarPendenciaRegistroIndividualUseCase _useCase;

        public AtualizarPendenciaRegistroIndividualUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new AtualizarPendenciaRegistroIndividualUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var comando = new AtualizarPendenciaRegistroIndividualCommand(10, 20, DateTime.Today);
            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(comando)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<AtualizarPendenciaRegistroIndividualCommand>(), It.IsAny<CancellationToken>()))
                         .Returns(Task.FromResult(Unit.Value));

            var resultado = await _useCase.Executar(mensagemRabbit);

            _mediatorMock.Verify(m => m.Send(
                It.Is<AtualizarPendenciaRegistroIndividualCommand>(c =>
                    c.TurmaId == comando.TurmaId &&
                    c.CodigoAluno == comando.CodigoAluno &&
                    c.DataRegistro == comando.DataRegistro),
                It.IsAny<CancellationToken>()), Times.Once);

            resultado.Should().BeTrue();
        }
    }
}
