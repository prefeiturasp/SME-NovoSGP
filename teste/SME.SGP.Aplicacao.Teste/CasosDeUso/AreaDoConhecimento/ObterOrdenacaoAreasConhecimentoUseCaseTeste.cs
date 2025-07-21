using System.Collections.Generic;
using Moq;
using Xunit;
using MediatR;
using System.Threading.Tasks;
using System.Linq;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AreaDoConhecimento
{
    public class ObterOrdenacaoAreasConhecimentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterOrdenacaoAreasConhecimentoUseCase _useCase;

        public ObterOrdenacaoAreasConhecimentoUseCaseTeste()
        {
            // Inicializa o mock do IMediator
            _mediatorMock = new Mock<IMediator>();

            // Cria uma instância do UseCase, passando o mock do Mediator
            _useCase = new ObterOrdenacaoAreasConhecimentoUseCase(_mediatorMock.Object);
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
                GrupoMatrizId = 1
            },
            new DisciplinaDto
            {
                Id = 2,
                CodigoComponenteCurricular = 102,
                Nome = "Física",
                GrupoMatrizId = 1
            }
        };

            var areasConhecimento = new List<AreaDoConhecimentoDto>
        {
            new AreaDoConhecimentoDto { Id = 1, Nome = "Matemática", CodigoComponenteCurricular = 101 },
            new AreaDoConhecimentoDto { Id = 2, Nome = "Física", CodigoComponenteCurricular = 102 }
        };

            var ordenacaoAreasConhecimento = new List<ComponenteCurricularGrupoAreaOrdenacaoDto>
        {
            new ComponenteCurricularGrupoAreaOrdenacaoDto { GrupoMatrizId = 1, AreaConhecimentoId = 1, Ordem = 1 },
            new ComponenteCurricularGrupoAreaOrdenacaoDto { GrupoMatrizId = 1, AreaConhecimentoId = 2, Ordem = 2 }
        };

            // Define o comportamento esperado do Mediator para a query ObterOrdenacaoAreasConhecimentoQuery
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterOrdenacaoAreasConhecimentoQuery>(), default))
                .ReturnsAsync(ordenacaoAreasConhecimento);

            var parametros = (disciplinas, areasConhecimento);

            // Act
            var resultado = await _useCase.Executar(parametros);

            // Assert
            // Verifica se o Send foi chamado com a query correta
            _mediatorMock.Verify(m => m.Send(It.Is<ObterOrdenacaoAreasConhecimentoQuery>(query =>
                query.ComponentesCurricularesTurma == disciplinas &&
                query.AreasConhecimento == areasConhecimento), default), Times.Once);

            // Verifica se o resultado retornado não é nulo
            Assert.NotNull(resultado);

            // Verifica se o resultado contém as áreas de ordenação esperadas
            Assert.Contains(resultado, o => o.AreaConhecimentoId == 1 && o.Ordem == 1);
            Assert.Contains(resultado, o => o.AreaConhecimentoId == 2 && o.Ordem == 2);
        }

        [Fact]
        public async Task Executar_DeveRetornarOrdenacaoCorreta()
        {
            // Arrange
            var disciplinas = new List<DisciplinaDto>
        {
            new DisciplinaDto
            {
                Id = 1,
                CodigoComponenteCurricular = 101,
                Nome = "Matemática",
                GrupoMatrizId = 1
            },
            new DisciplinaDto
            {
                Id = 2,
                CodigoComponenteCurricular = 102,
                Nome = "Física",
                GrupoMatrizId = 1
            }
        };

            var areasConhecimento = new List<AreaDoConhecimentoDto>
        {
            new AreaDoConhecimentoDto { Id = 1, Nome = "Matemática", CodigoComponenteCurricular = 101 },
            new AreaDoConhecimentoDto { Id = 2, Nome = "Física", CodigoComponenteCurricular = 102 }
        };

            var ordenacaoAreasConhecimento = new List<ComponenteCurricularGrupoAreaOrdenacaoDto>
        {
            new ComponenteCurricularGrupoAreaOrdenacaoDto { GrupoMatrizId = 1, AreaConhecimentoId = 1, Ordem = 1 },
            new ComponenteCurricularGrupoAreaOrdenacaoDto { GrupoMatrizId = 1, AreaConhecimentoId = 2, Ordem = 2 }
        };

            // Define o comportamento esperado do Mediator para a query ObterOrdenacaoAreasConhecimentoQuery
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterOrdenacaoAreasConhecimentoQuery>(), default))
                .ReturnsAsync(ordenacaoAreasConhecimento);

            var parametros = (disciplinas, areasConhecimento);

            // Act
            var resultado = await _useCase.Executar(parametros);

            // Assert
            // Verifica se o resultado contém a ordenação correta
            var ordenacaoResult = resultado.ToList();
            Assert.Equal(2, ordenacaoResult.Count);
            Assert.Equal(1, ordenacaoResult.First(o => o.AreaConhecimentoId == 1).Ordem);
            Assert.Equal(2, ordenacaoResult.First(o => o.AreaConhecimentoId == 2).Ordem);
        }
    }
}
