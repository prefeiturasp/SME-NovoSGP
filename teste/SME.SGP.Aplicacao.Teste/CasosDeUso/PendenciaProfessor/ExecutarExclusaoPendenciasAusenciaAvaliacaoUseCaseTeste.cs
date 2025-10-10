using FluentAssertions;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PendenciaProfessor
{
    public class ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase _useCase;

        public ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ExecutarExclusaoPendenciasAusenciaAvaliacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Mensagem_Valida_Deve_Enviar_Comando_E_Retornar_True()
        {
            var dataAvaliacao = new DateTime(2025, 10, 9);
            var command = new VerificaExclusaoPendenciasAusenciaAvaliacaoCommand("T123",
                                                                                 new[] { "CC456" },
                                                                                 TipoPendencia.AulasReposicaoPendenteAprovacao,
                                                                                 dataAvaliacao);

            var mensagemRabbit = new MensagemRabbit
            {
                Mensagem = JsonConvert.SerializeObject(command)
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<VerificaExclusaoPendenciasAusenciaAvaliacaoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            var resultado = await _useCase.Executar(mensagemRabbit);

            resultado.Should().BeTrue();

            _mediatorMock.Verify(m => m.Send(
                    It.Is<VerificaExclusaoPendenciasAusenciaAvaliacaoCommand>(c =>
                        c.TurmaCodigo == command.TurmaCodigo &&
                        c.ComponentesCurriculares[0] == command.ComponentesCurriculares[0] &&
                        c.TipoPendencia == command.TipoPendencia &&
                        c.DataAvaliacao == command.DataAvaliacao),
                    It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
