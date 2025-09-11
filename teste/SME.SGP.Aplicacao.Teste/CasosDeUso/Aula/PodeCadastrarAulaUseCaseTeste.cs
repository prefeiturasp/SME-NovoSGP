using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Aula
{
    public class PodeCadastrarAulaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PodeCadastrarAulaUseCase _useCase;

        public PodeCadastrarAulaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new PodeCadastrarAulaUseCase(_mediatorMock.Object);
        }

        private FiltroPodeCadastrarAulaDto CriarFiltro(long aulaId = 0, DateTime? data = null, TipoAula tipo = TipoAula.Normal)
        {
            return new FiltroPodeCadastrarAulaDto
            {
                AulaId = aulaId,
                TurmaCodigo = "TURMA123",
                ComponentesCurriculares = new[] { 101L, 102L },
                DataAula = data ?? DateTime.Today,
                EhRegencia = false,
                TipoAula = tipo
            };
        }

        [Fact]
        public async Task Executar_DeveRetornarCadastroAulaDto_ComGrade_QuandoPodeCadastrar()
        {
            // Arrange
            var filtro = CriarFiltro();
            var gradeMock = new GradeComponenteTurmaAulasDto();

            _mediatorMock.Setup(m => m.Send(It.IsAny<PodeCadastrarAulaNoDiaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterGradeAulasPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(gradeMock);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.True(resultado.PodeCadastrarAula);
            Assert.Equal(gradeMock, resultado.Grade);
        }

        [Fact]
        public async Task Executar_DeveRetornarCadastroAulaDto_SemGrade_QuandoTipoAulaForReposicao()
        {
            // Arrange
            var filtro = CriarFiltro(tipo: TipoAula.Reposicao);

            _mediatorMock.Setup(m => m.Send(It.IsAny<PodeCadastrarAulaNoDiaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.True(resultado.PodeCadastrarAula);
            Assert.Null(resultado.Grade); // Reposição não retorna grade
        }

        [Fact]
        public async Task Executar_DeveLancarExcecao_QuandoNaoPodeCadastrar()
        {
            // Arrange
            var filtro = CriarFiltro();

            _mediatorMock.Setup(m => m.Send(It.IsAny<PodeCadastrarAulaNoDiaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Contains("Não é possível cadastrar aula do tipo", ex.Message);
        }

        [Fact]
        public async Task Executar_NaoDeveChamarPodeCadastrarAulaNoDia_QuandoDataNaoFoiAlterada()
        {
            // Arrange
            var data = DateTime.Today;
            var filtro = CriarFiltro(aulaId: 99, data: data);

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterDataAulaQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(data); // mesma data original

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.True(resultado.PodeCadastrarAula);
            _mediatorMock.Verify(m => m.Send(It.IsAny<PodeCadastrarAulaNoDiaQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
