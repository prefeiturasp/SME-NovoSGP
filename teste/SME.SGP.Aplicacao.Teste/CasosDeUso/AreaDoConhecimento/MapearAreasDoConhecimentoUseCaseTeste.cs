using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.Queries.AreaDoConhecimento.MapearAreasConhecimento;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AreaDoConhecimento
{
    public class MapearAreasDoConhecimentoUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MapearAreasDoConhecimentoUseCase _useCase;

        public MapearAreasDoConhecimentoUseCaseTeste()
        {
            // Inicializa o mock do IMediator
            _mediatorMock = new Mock<IMediator>();

            // Cria uma instância do UseCase, passando o mock do Mediator
            _useCase = new MapearAreasDoConhecimentoUseCase(_mediatorMock.Object);
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
                CodigoComponenteCurricularTerritorioSaber = 102,
                Nome = "Matemática",
                NomeComponenteInfantil = "Matemática Infantil",
                GrupoMatrizId = 1,
                Regencia = false
            }
        };

            var areasConhecimento = new List<AreaDoConhecimentoDto>
        {
            new AreaDoConhecimentoDto
            {
                Id = 1,
                Nome = "Matemática",
                CodigoComponenteCurricular = 101
            }
        };

            var componentesCurricularesGrupoAreaOrdem = new List<ComponenteCurricularGrupoAreaOrdenacaoDto>
        {
            new ComponenteCurricularGrupoAreaOrdenacaoDto
            {
                GrupoMatrizId = 1,
                AreaConhecimentoId = 1,
                Ordem = 1
            }
        };

            long grupoMatrizId = 1;

            // Define o comportamento esperado do Mediator
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<MapearAreasConhecimentoQuery>(), default))
                .ReturnsAsync(new List<IGrouping<(string Nome, int? Ordem, long Id), AreaDoConhecimentoDto>>());

            var parametros = (disciplinas, areasConhecimento, componentesCurricularesGrupoAreaOrdem, grupoMatrizId);

            // Act
            var resultado = await _useCase.Executar(parametros);

            // Assert
            _mediatorMock.Verify(m => m.Send(It.Is<MapearAreasConhecimentoQuery>(query =>
                query.ComponentesCurricularesTurma == disciplinas &&
                query.AreasConhecimentos == areasConhecimento &&
                query.GruposAreaOrdenacao == componentesCurricularesGrupoAreaOrdem &&
                query.GrupoMatrizId == grupoMatrizId), default), Times.Once);
        }

        [Fact]
        public async Task MapearAreasConhecimentoQueryHandler_DeveFiltrarAreasConhecimentoCorretamente()
        {
            // Arrange
            var componentesCurricularesTurma = new List<DisciplinaDto>
    {
        new DisciplinaDto
        {
            Id = 1,
            CodigoComponenteCurricular = 101,
            CodigoComponenteCurricularTerritorioSaber = 102,
            Nome = "Matemática",
            Regencia = false,
            GrupoMatrizId = 1
        },
        new DisciplinaDto
        {
            Id = 2,
            CodigoComponenteCurricular = 201,
            Nome = "Física",
            Regencia = true,
            GrupoMatrizId = 1
        }
    };

            var areasConhecimento = new List<AreaDoConhecimentoDto>
    {
        new AreaDoConhecimentoDto
        {
            Id = 1,
            Nome = "Matemática",
            CodigoComponenteCurricular = 101
        },
        new AreaDoConhecimentoDto
        {
            Id = 2,
            Nome = "Física",
            CodigoComponenteCurricular = 201
        }
    };

            var componentesCurricularesGrupoAreaOrdem = new List<ComponenteCurricularGrupoAreaOrdenacaoDto>
    {
        new ComponenteCurricularGrupoAreaOrdenacaoDto
        {
            GrupoMatrizId = 1,
            AreaConhecimentoId = 1,
            Ordem = 1
        },
        new ComponenteCurricularGrupoAreaOrdenacaoDto
        {
            GrupoMatrizId = 1,
            AreaConhecimentoId = 2,
            Ordem = 2
        }
    };

            var grupoMatrizId = 1;

            var query = new MapearAreasConhecimentoQuery(componentesCurricularesTurma, areasConhecimento, componentesCurricularesGrupoAreaOrdem, grupoMatrizId);

            var handler = new MapearAreasConhecimentoQueryHandler();

            // Act
            var resultado = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(resultado);

            // Agora validamos os grupos e garantimos que existam pelo menos dois resultados agrupados
            Assert.True(resultado.Any());  // Verifica se há pelo menos um grupo
            Assert.Contains(resultado, group => group.Key.Nome == "Matemática" && group.Key.Ordem == 1);
            Assert.Contains(resultado, group => group.Key.Nome == "Física" && group.Key.Ordem == 2);

            // Se você espera apenas um grupo, pode descomentar esta linha:
            // Assert.Single(resultado);
        }
    }
}
