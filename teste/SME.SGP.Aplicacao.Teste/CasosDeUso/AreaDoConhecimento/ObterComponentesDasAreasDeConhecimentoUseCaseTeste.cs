using System.Collections.Generic;
using Moq;
using Xunit;
using MediatR;
using System.Threading.Tasks;
using System.Linq;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AreaDoConhecimento
{
    public class ObterComponentesDasAreasDeConhecimentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterComponentesDasAreasDeConhecimentoUseCase _useCase;

        public ObterComponentesDasAreasDeConhecimentoUseCaseTeste()
        {
            // Inicializa o mock do IMediator
            _mediatorMock = new Mock<IMediator>();

            // Cria uma instância do UseCase, passando o mock do Mediator
            _useCase = new ObterComponentesDasAreasDeConhecimentoUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveChamarMediatorComQueryCorreta()
        {
            // Arrange
            var disciplinas = new List<DisciplinaDto>
        {
            new DisciplinaDto
            {
                Id = 1,
                CodigoComponenteCurricular = 101,
                Nome = "Matemática",
                Regencia = false,
                GrupoMatrizId = 1
            },
            new DisciplinaDto
            {
                Id = 2,
                CodigoComponenteCurricular = 102,
                Nome = "Física",
                Regencia = false,
                GrupoMatrizId = 1
            }
        };

            var areasConhecimento = new List<AreaDoConhecimentoDto>
        {
            new AreaDoConhecimentoDto { Id = 1, Nome = "Matemática", CodigoComponenteCurricular = 101 },
            new AreaDoConhecimentoDto { Id = 2, Nome = "Física", CodigoComponenteCurricular = 102 }
        };

            // Define o comportamento esperado do Mediator para o ObterComponentesAreasConhecimentoQuery
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponentesAreasConhecimentoQuery>(), default))
                .ReturnsAsync(new List<DisciplinaDto>
                {
                new DisciplinaDto { Id = 1, CodigoComponenteCurricular = 101, Nome = "Matemática" },
                new DisciplinaDto { Id = 2, CodigoComponenteCurricular = 102, Nome = "Física" }
                });

            var parametros = (disciplinas, areasConhecimento);

            // Act
            var resultado = await _useCase.Executar(parametros);

            // Assert
            // Verifica se o Send foi chamado com a query correta
            _mediatorMock.Verify(m => m.Send(It.Is<ObterComponentesAreasConhecimentoQuery>(query =>
                query.ComponentesCurricularesTurma == disciplinas &&
                query.AreasConhecimento == areasConhecimento), default), Times.Once);

            // Verifica se o resultado retornado não é nulo
            Assert.NotNull(resultado);

            // Verifica se o resultado contém as disciplinas esperadas
            Assert.Contains(resultado, d => d.Nome == "Matemática");
            Assert.Contains(resultado, d => d.Nome == "Física");
        }

        [Fact]
        public async Task Executar_DeveRetornarComponentesCorretos()
        {
            // Arrange
            var disciplinas = new List<DisciplinaDto>
        {
            new DisciplinaDto
            {
                Id = 1,
                CodigoComponenteCurricular = 101,
                Nome = "Matemática",
                Regencia = false,
                GrupoMatrizId = 1
            },
            new DisciplinaDto
            {
                Id = 2,
                CodigoComponenteCurricular = 102,
                Nome = "Física",
                Regencia = true,
                GrupoMatrizId = 1
            }
        };

            var areasConhecimento = new List<AreaDoConhecimentoDto>
        {
            new AreaDoConhecimentoDto { Id = 1, Nome = "Matemática", CodigoComponenteCurricular = 101 },
            new AreaDoConhecimentoDto { Id = 2, Nome = "Física", CodigoComponenteCurricular = 102 }
        };

            // Define o comportamento esperado do Mediator para o ObterComponentesAreasConhecimentoQuery
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterComponentesAreasConhecimentoQuery>(), default))
                .ReturnsAsync(new List<DisciplinaDto>
                {
                new DisciplinaDto { Id = 1, CodigoComponenteCurricular = 101, Nome = "Matemática", Regencia = false },
                new DisciplinaDto { Id = 2, CodigoComponenteCurricular = 102, Nome = "Física", Regencia = true }
                });

            var parametros = (disciplinas, areasConhecimento);

            // Act
            var resultado = await _useCase.Executar(parametros);

            // Assert
            // Verifica se o resultado contém as disciplinas com regência ou não
            var disciplinasResultadas = resultado.ToList();
            Assert.Equal(2, disciplinasResultadas.Count);
            Assert.Contains(disciplinasResultadas, d => d.Nome == "Matemática" && !d.Regencia);
            Assert.Contains(disciplinasResultadas, d => d.Nome == "Física" && d.Regencia);
        }
    }
}
