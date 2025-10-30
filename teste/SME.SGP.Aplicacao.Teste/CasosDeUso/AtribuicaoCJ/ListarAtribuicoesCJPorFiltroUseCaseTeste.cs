using MediatR;
using Moq;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.AtribuicaoCJ
{
    public class ListarAtribuicoesCJPorFiltroUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ListarAtribuicoesCJPorFiltroUseCase _useCase;

        public ListarAtribuicoesCJPorFiltroUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ListarAtribuicoesCJPorFiltroUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_DeveRetornarListaVaziaQuandoNaoHaAtribuicoes()
        {
            // Arrange
            var filtro = new AtribuicaoCJListaFiltroDto
            {
                UeId = "UE001",
                AnoLetivo = 2024,
                UsuarioRf = "12345",
                UsuarioNome = "Teste User",
                Historico = false
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<Dominio.AtribuicaoCJ>());

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterAtribuicoesPorTurmaEProfessorQuery>(q =>
                q.UeId == filtro.UeId &&
                q.AnoLetivo == filtro.AnoLetivo &&
                q.UsuarioRf == filtro.UsuarioRf &&
                q.UsuarioNome == filtro.UsuarioNome &&
                q.Substituir == true &&
                q.Historico == filtro.Historico
            ), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLancarNegocioExceptionQuandoDisciplinasNaoSaoEncontradasNoEol()
        {
            // Arrange
            var filtro = new AtribuicaoCJListaFiltroDto
            {
                UeId = "UE001",
                AnoLetivo = 2024,
                UsuarioRf = "12345",
                UsuarioNome = "Teste User",
                Historico = false
            };

            var atribuicoesMock = new List<Dominio.AtribuicaoCJ>
            {
                new Dominio.AtribuicaoCJ
                {
                    DisciplinaId = 101,
                    ProfessorRf = "12345",
                    Modalidade = Modalidade.Fundamental,
                    TurmaId = "T001",
                    UeId = "UE001",
                    Turma = new Turma { Nome = "Turma A", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = 2024 }
                }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atribuicoesMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(q => q.CodigoTurma == "T001"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Enumerable.Empty<DisciplinaResposta>()); 

            // Act & Assert
            var ex = await Assert.ThrowsAsync<NegocioException>(() => _useCase.Executar(filtro));
            Assert.Equal("Não foi possível obter as descrições das disciplinas no Eol.", ex.Message);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(q => q.CodigoTurma == "T001"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveUsarNomeComumParaDisciplinasInfantisEmAnoAnterior()
        {
            // Arrange
            var filtro = new AtribuicaoCJListaFiltroDto
            {
                UeId = "UE001",
                AnoLetivo = DateTime.Now.Year - 1,
                UsuarioRf = "12345",
                UsuarioNome = "Teste User",
                Historico = false
            };

            var atribuicoesMock = new List<Dominio.AtribuicaoCJ>
            {
                new Dominio.AtribuicaoCJ
                {
                    DisciplinaId = 201,
                    ProfessorRf = "12345",
                    Modalidade = Modalidade.EducacaoInfantil,
                    TurmaId = "T002",
                    UeId = "UE001",
                    Turma = new Turma { Nome = "Infantil B", ModalidadeCodigo = Modalidade.EducacaoInfantil, AnoLetivo = DateTime.Now.Year - 1 }
                }
            };

            var disciplinasEolMock = new List<DisciplinaResposta>
            {
                new DisciplinaResposta { CodigoComponenteCurricular = 201, Nome = "Higiene Pessoal"}
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atribuicoesMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(q => q.CodigoTurma == "T002"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinasEolMock);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList);

            var firstAtribuicao = resultList.First();
            Assert.Equal("Educação Infantil", firstAtribuicao.Modalidade);
            Assert.Equal("Infantil B", firstAtribuicao.Turma);
            Assert.Contains("Higiene Pessoal", firstAtribuicao.Disciplinas); 
            Assert.DoesNotContain("Corpo e Gestos", firstAtribuicao.Disciplinas);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(q => q.CodigoTurma == "T002"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task Executar_DeveLidarComDisciplinasTerritorioSaber()
        {
            // Arrange
            var filtro = new AtribuicaoCJListaFiltroDto
            {
                UeId = "UE001",
                AnoLetivo = 2024,
                UsuarioRf = "12345",
                UsuarioNome = "Teste User",
                Historico = false
            };

            var atribuicoesMock = new List<Dominio.AtribuicaoCJ>
            {
                new Dominio.AtribuicaoCJ
                {
                    DisciplinaId = 300,
                    ProfessorRf = "12345",
                    Modalidade = Modalidade.Fundamental,
                    TurmaId = "T003",
                    UeId = "UE001",
                    Turma = new Turma { Nome = "Turma B", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = 2024 }
                },
                new Dominio.AtribuicaoCJ
                {
                    DisciplinaId = 301,
                    ProfessorRf = "12345",
                    Modalidade = Modalidade.Fundamental,
                    TurmaId = "T003",
                    UeId = "UE001",
                    Turma = new Turma { Nome = "Turma B", ModalidadeCodigo = Modalidade.Fundamental, AnoLetivo = 2024 }
                }
            };

            var disciplinasEolMock = new List<DisciplinaResposta>
            {
                new DisciplinaResposta { CodigoComponenteCurricular = 301, Nome = "Matemática do Campo", TerritorioSaber = true, CodigoComponenteTerritorioSaber = 300, CodigosTerritoriosAgrupamento = new long[] {300} },
                new DisciplinaResposta { CodigoComponenteCurricular = 302, Nome = "Cultura Regional" }
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(atribuicoesMock);
            _mediatorMock.Setup(m => m.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(q => q.CodigoTurma == "T003"), It.IsAny<CancellationToken>()))
                .ReturnsAsync(disciplinasEolMock);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            var resultList = result.ToList();
            Assert.Single(resultList);

            var firstAtribuicao = resultList.First();
            Assert.Contains("Matemática do Campo", firstAtribuicao.Disciplinas); // Should include territory discipline
            Assert.DoesNotContain("Cultura Regional", firstAtribuicao.Disciplinas);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterAtribuicoesPorTurmaEProfessorQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.Is<ObterDisciplinasPorCodigoTurmaQuery>(q => q.CodigoTurma == "T003"), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.VerifyNoOtherCalls();
        }
    }

    internal static class EnumExtensions
    {
        public static T GetAttribute<T>(this Enum value) where T : Attribute
        {
            var type = value.GetType();
            var name = Enum.GetName(type, value);
            if (name == null) return null;
            var field = type.GetField(name);
            if (field == null) return null;
            return (T)Attribute.GetCustomAttribute(field, typeof(T));
        }
    }
}