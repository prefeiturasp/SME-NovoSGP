using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula.Pendencia
{
    public class PendenciaAulaAvaliacaoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PendenciaAulaAvaliacaoUseCase _useCase;

        public PendenciaAulaAvaliacaoUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PendenciaAulaAvaliacaoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarComandos_QuandoExistiremAulasPendentes()
        {
            // Arrange
            var filtro = new DreUeDto(1, 2);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            var aulas = new List<SME.SGP.Dominio.Aula>
            {
                new SME.SGP.Dominio.Aula { Id = 100, DisciplinaId = "1051" },
                new SME.SGP.Dominio.Aula { Id = 200, DisciplinaId = "1060" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulas);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(aulas); // mantém as aulas

            _mediatorMock.Setup(m => m.Send(It.IsAny<SalvarPendenciaAulasPorTipoCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Unit.Value);

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.Is<ObterPendenciasAtividadeAvaliativaQuery>(q =>
                q.DreId == filtro.DreId && q.UeId == filtro.UeId), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAulasPendenciaSemFechamentoTurmaDiscplinaProcessadoQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.Is<SalvarPendenciaAulasPorTipoCommand>(cmd =>
                cmd.TipoPendenciaAula == TipoPendencia.Avaliacao &&
                ((IEnumerable<SME.SGP.Dominio.Aula>)cmd.Aulas).Count() == aulas.Count), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_DeveRetornarTrue_QuandoNaoExistiremAulas()
        {
            // Arrange
            var filtro = new DreUeDto(1, 2);
            var mensagem = new MensagemRabbit(JsonConvert.SerializeObject(filtro));

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(new List<SME.SGP.Dominio.Aula>());

            // Act
            var resultado = await _useCase.Executar(mensagem);

            // Assert
            Assert.True(resultado);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterPendenciasAtividadeAvaliativaQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<SalvarPendenciaAulasPorTipoCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
