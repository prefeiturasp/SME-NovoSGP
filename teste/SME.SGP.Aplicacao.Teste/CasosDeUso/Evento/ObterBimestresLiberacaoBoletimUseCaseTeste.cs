using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Evento
{
    public class ObterBimestresLiberacaoBoletimUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterBimestresLiberacaoBoletimUseCase _useCase;

        public ObterBimestresLiberacaoBoletimUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterBimestresLiberacaoBoletimUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_Excecao()
        {
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Turma)null);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar("TURMA-INEXISTENTE"));
        }

        [Fact]
        public async Task Executar_Quando_Tipo_Calendario_Nao_Encontrado_Deve_Lancar_Excecao()
        {
            var turma = new Turma { CodigoTurma = "TURMA-VALIDA" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioIdPorTurmaQuery>(q => q.Turma == turma), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(0L);

            await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar("TURMA-VALIDA"));
        }

        [Fact]
        public async Task Executar_Quando_Dados_Validos_Deve_Retornar_Bimestres()
        {
            var turma = new Turma { CodigoTurma = "TURMA-VALIDA" };
            var tipoCalendarioId = 123L;
            var bimestresEsperados = new[] { 1, 2 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterTurmaPorCodigoQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTipoCalendarioIdPorTurmaQuery>(q => q.Turma == turma), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(tipoCalendarioId);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterBimestresEventoLiberacaoBoletimQuery>(q => q.TipoCalendarioId == tipoCalendarioId), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(bimestresEsperados);

            var resultado = await _useCase.Executar("TURMA-VALIDA");

            Assert.NotNull(resultado);
            Assert.Equal(bimestresEsperados, resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterBimestresEventoLiberacaoBoletimQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
