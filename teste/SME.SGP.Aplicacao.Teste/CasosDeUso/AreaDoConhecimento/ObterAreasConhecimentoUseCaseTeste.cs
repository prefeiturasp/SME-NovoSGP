using System.Collections.Generic;
using Moq;
using Xunit;
using MediatR;
using System.Threading.Tasks;
using SME.SGP.Infra;
using SME.SGP.Aplicacao.Queries;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AreaDoConhecimento
{
    public class ObterAreasConhecimentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterAreasConhecimentoUseCase _useCase;

        public ObterAreasConhecimentoUseCaseTeste()
        {
            // Inicializa o mock do IMediator
            _mediatorMock = new Mock<IMediator>();

            // Cria uma instância do UseCase, passando o mock do Mediator
            _useCase = new ObterAreasConhecimentoUseCase(_mediatorMock.Object);
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

            // Define o comportamento esperado do Mediator para o ObterAreasConhecimentoQuery
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAreasConhecimentoQuery>(), default))
                .ReturnsAsync(new List<AreaDoConhecimentoDto>
                {
                new AreaDoConhecimentoDto { Id = 1, Nome = "Matemática", CodigoComponenteCurricular = 101 },
                new AreaDoConhecimentoDto { Id = 2, Nome = "Física", CodigoComponenteCurricular = 102 }
                });

            // Act
            var resultado = await _useCase.Executar(disciplinas);

            // Assert
            // Verifica se o Send foi chamado com a query correta
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAreasConhecimentoQuery>(query =>
                query.ComponentesCurriculares == disciplinas), default), Times.Once);

            // Verifica se o resultado retornado não é nulo
            Assert.NotNull(resultado);

            // Verifica se o resultado contém as áreas de conhecimento esperadas
            Assert.Contains(resultado, area => area.Nome == "Matemática");
            Assert.Contains(resultado, area => area.Nome == "Física");
        }
    }
}
