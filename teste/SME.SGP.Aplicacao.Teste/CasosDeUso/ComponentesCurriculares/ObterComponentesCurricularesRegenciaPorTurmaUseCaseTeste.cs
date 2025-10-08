using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ComponentesCurriculares
{
    public class ObterComponentesCurricularesRegenciaPorTurmaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterComponentesCurricularesRegenciaPorTurmaUseCase _useCase;

        public ObterComponentesCurricularesRegenciaPorTurmaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterComponentesCurricularesRegenciaPorTurmaUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_Encontrada_Deve_Lancar_NegocioException()
        {
            var turmaId = 99L;
            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == turmaId), default))
                         .ReturnsAsync((Turma)null);

            var exception = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(turmaId));

            Assert.Equal("Turma não encontrada.", exception.Message);
        }

        [Fact]
        public async Task Executar_Quando_Turma_Nao_For_Fundamental_Deve_Buscar_Componentes_Com_Ano_E_Turno_Zerados()
        {
            var turmaId = 1L;
            var turma = new Turma { ModalidadeCodigo = Modalidade.Medio, Ano = "1", QuantidadeDuracaoAula = 5 };
            var componentesEsperados = new List<DisciplinaDto> { new DisciplinaDto { Nome = "Matemática" } };
            long anoEsperado = 0;
            long turnoEsperado = 0;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == turmaId), default))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>(q => q.Ano == anoEsperado && q.Turno == turnoEsperado), default))
                         .ReturnsAsync(componentesEsperados);

            var resultado = await _useCase.Executar(turmaId);

            Assert.NotNull(resultado);
            Assert.Same(componentesEsperados, resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>(q => q.Ano == anoEsperado && q.Turno == turnoEsperado), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Turma_For_Fundamental_E_Ano_For_Invalido_Deve_Buscar_Componentes_Com_Ano_Zerado()
        {
            var turmaId = 2L;
            var turma = new Turma { ModalidadeCodigo = Modalidade.Fundamental, Ano = "Inválido", QuantidadeDuracaoAula = 5 };
            var componentesEsperados = new List<DisciplinaDto> { new DisciplinaDto { Nome = "Língua Portuguesa" } };
            long anoEsperado = 0;
            long turnoEsperado = 5;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == turmaId), default))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>(q => q.Ano == anoEsperado && q.Turno == turnoEsperado), default))
                         .ReturnsAsync(componentesEsperados);

            var resultado = await _useCase.Executar(turmaId);

            Assert.NotNull(resultado);
            Assert.Same(componentesEsperados, resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>(q => q.Ano == anoEsperado && q.Turno == turnoEsperado), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_Turma_For_Fundamental_E_Ano_Valido_Deve_Buscar_Componentes_Com_Ano_E_Turno_Corretos()
        {
            var turmaId = 3L;
            var turma = new Turma { ModalidadeCodigo = Modalidade.Fundamental, Ano = "4", QuantidadeDuracaoAula = 6 };
            var componentesEsperados = new List<DisciplinaDto> { new DisciplinaDto { Nome = "Ciências" } };
            long anoEsperado = 4;
            long turnoEsperado = 6;

            _mediatorMock.Setup(m => m.Send(It.Is<ObterTurmaComUeEDrePorIdQuery>(q => q.TurmaId == turmaId), default))
                         .ReturnsAsync(turma);

            _mediatorMock.Setup(m => m.Send(It.Is<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>(q => q.Ano == anoEsperado && q.Turno == turnoEsperado), default))
                         .ReturnsAsync(componentesEsperados);

            var resultado = await _useCase.Executar(turmaId);

            Assert.NotNull(resultado);
            Assert.Same(componentesEsperados, resultado);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterComponentesCurricularesRegenciaPorAnoETurnoQuery>(q => q.Ano == anoEsperado && q.Turno == turnoEsperado), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
