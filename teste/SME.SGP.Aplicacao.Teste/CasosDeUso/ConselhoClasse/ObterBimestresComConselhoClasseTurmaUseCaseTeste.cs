using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConselhoClasse
{
    public class ObterBimestresComConselhoClasseTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly IObterBimestresComConselhoClasseTurmaUseCase _useCase;

        public ObterBimestresComConselhoClasseTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterBimestresComConselhoClasseTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public void Construtor_Quando_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new ObterBimestresComConselhoClasseTurmaUseCase(null));
        }

        [Fact]
        public async Task Executar_Quando_Turma_Id_Valido_Deve_Enviar_Query_Retornar_Lista_De_Bimestres()
        {
            var turmaId = 987L;
            var retornoEsperado = new List<BimestreComConselhoClasseTurmaDto>
            {
                new BimestreComConselhoClasseTurmaDto { Bimestre = 1, ConselhoClasseId = 10, FechamentoTurmaId = 100 },
                new BimestreComConselhoClasseTurmaDto { Bimestre = 2, ConselhoClasseId = 20, FechamentoTurmaId = 200 }
            };

            _mediatorMock.Setup(m => m.Send(
                It.Is<ObterBimestresComConselhoClasseTurmaQuery>(q => q.Id == turmaId),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await _useCase.Executar(turmaId);

            Assert.NotNull(resultado);
            Assert.Equal(retornoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<ObterBimestresComConselhoClasseTurmaQuery>(q => q.Id == turmaId),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
