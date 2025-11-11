using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ExcluirAtribuicaoEsporadicaTeste
{
    public class ListarAtribuicaoEsporadicaUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IContextoAplicacao> _contextoAplicacaoMock;
        private readonly ListarAtribuicaoEsporadicaUseCase _useCase;

        public ListarAtribuicaoEsporadicaUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _contextoAplicacaoMock = new Mock<IContextoAplicacao>();
            _useCase = new ListarAtribuicaoEsporadicaUseCase(_contextoAplicacaoMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_ShouldReturnPaginatedResults_WhenAtribuicoesExist()
        {
            // Arrange
            var filtro = new FiltroAtribuicaoEsporadicaDto
            {
                AnoLetivo = 2024,
                DreId = "DRE1",
                UeId = "UE1",
                ProfessorRF = "12345"
            };

            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroRegistros")).Returns("10");

            var atribuicoesDominio = new List<AtribuicaoEsporadica>
        {
            new AtribuicaoEsporadica { Id = 1, AnoLetivo = 2024, DataInicio = DateTime.Now.AddDays(-10), DataFim = DateTime.Now.AddDays(-5), DreId = "DRE1", UeId = "UE1", ProfessorRf = "12345", Excluido = false, Migrado = false },
            new AtribuicaoEsporadica { Id = 2, AnoLetivo = 2024, DataInicio = DateTime.Now.AddDays(-20), DataFim = DateTime.Now.AddDays(-15), DreId = "DRE1", UeId = "UE1", ProfessorRf = "67890", Excluido = false, Migrado = false }
        };

            var paginacaoResultadoDominio = new PaginacaoResultadoDto<AtribuicaoEsporadica>
            {
                Items = atribuicoesDominio,
                TotalPaginas = 1,
                TotalRegistros = 2
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginacaoResultadoDominio);

            var professoresResumo = new List<ProfessorResumoDto>
        {
            new ProfessorResumoDto { CodigoRF = "12345", Nome = "Professor A" },
            new ProfessorResumoDto { CodigoRF = "67890", Nome = "Professor B" }
        };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterFuncionariosPorRFsQuery>(q => q.CodigosRf.Contains("12345") && q.CodigosRf.Contains("67890")), It.IsAny<CancellationToken>()))
                .ReturnsAsync(professoresResumo);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(paginacaoResultadoDominio.TotalPaginas, result.TotalPaginas);
            Assert.Equal(paginacaoResultadoDominio.TotalRegistros, result.TotalRegistros);
            Assert.NotNull(result.Items);
            Assert.Equal(2, result.Items.Count());

            var firstItem = result.Items.FirstOrDefault();
            Assert.NotNull(firstItem);
            Assert.Equal(atribuicoesDominio[0].Id, firstItem.Id);
            Assert.Equal(atribuicoesDominio[0].ProfessorRf, firstItem.ProfessorRf);
            Assert.Equal("Professor A", firstItem.ProfessorNome);

            var secondItem = result.Items.LastOrDefault();
            Assert.NotNull(secondItem);
            Assert.Equal(atribuicoesDominio[1].Id, secondItem.Id);
            Assert.Equal(atribuicoesDominio[1].ProfessorRf, secondItem.ProfessorRf);
            Assert.Equal("Professor B", secondItem.ProfessorNome);

            _mediatorMock.Verify(m => m.Send(It.Is<ListarAtribuicaoEsporadicaPorFiltrosQuery>(q =>
                q.AnoLetivo == filtro.AnoLetivo &&
                q.DreId == filtro.DreId &&
                q.UeId == filtro.UeId &&
                q.ProfessorRF == filtro.ProfessorRF), It.IsAny<CancellationToken>()), Times.Once);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorRFsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterResumoProfessorPorRFAnoLetivoQuery>(), It.IsAny<CancellationToken>()), Times.Never); 
        }

        [Fact]
        public async Task Executar_ShouldReturnNullItems_WhenNoAtribuicoesFound()
        {
            // Arrange
            var filtro = new FiltroAtribuicaoEsporadicaDto
            {
                AnoLetivo = 2024,
                DreId = "DRE1",
                UeId = "UE1",
                ProfessorRF = "12345"
            };

            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroRegistros")).Returns("10");

            var paginacaoResultadoDominio = new PaginacaoResultadoDto<AtribuicaoEsporadica>
            {
                Items = new List<AtribuicaoEsporadica>(),
                TotalPaginas = 0,
                TotalRegistros = 0
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginacaoResultadoDominio);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalPaginas);
            Assert.Equal(0, result.TotalRegistros);
            Assert.Null(result.Items);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorRFsQuery>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Executar_ShouldReturnNullItems_WhenFirstItemHasZeroId()
        {
            // Arrange
            var filtro = new FiltroAtribuicaoEsporadicaDto
            {
                AnoLetivo = 2024,
                DreId = "DRE1",
                UeId = "UE1",
                ProfessorRF = "12345"
            };

            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroRegistros")).Returns("10");

            var atribuicoesDominio = new List<AtribuicaoEsporadica>
        {
            new AtribuicaoEsporadica { Id = 0, AnoLetivo = 2024, DataInicio = DateTime.Now.AddDays(-10), DataFim = DateTime.Now.AddDays(-5), DreId = "DRE1", UeId = "UE1", ProfessorRf = "12345" }
        };

            var paginacaoResultadoDominio = new PaginacaoResultadoDto<AtribuicaoEsporadica>
            {
                Items = atribuicoesDominio,
                TotalPaginas = 1,
                TotalRegistros = 1
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginacaoResultadoDominio);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.TotalPaginas);
            Assert.Equal(1, result.TotalRegistros);
            Assert.Null(result.Items);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorRFsQuery>(), It.IsAny<CancellationToken>()), Times.Never); 
        }

        [Fact]
        public async Task Executar_ShouldHandleNullItemsInPaginacaoResult()
        {
            // Arrange
            var filtro = new FiltroAtribuicaoEsporadicaDto
            {
                AnoLetivo = 2024,
                DreId = "DRE1",
                UeId = "UE1",
                ProfessorRF = "12345"
            };

            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroPagina")).Returns("1");
            _contextoAplicacaoMock.Setup(x => x.ObterVariavel<string>("NumeroRegistros")).Returns("10");

            var paginacaoResultadoDominio = new PaginacaoResultadoDto<AtribuicaoEsporadica>
            {
                Items = null,
                TotalPaginas = 0,
                TotalRegistros = 0
            };

            _mediatorMock.Setup(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(paginacaoResultadoDominio);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.TotalPaginas);
            Assert.Equal(0, result.TotalRegistros);
            Assert.Null(result.Items);

            _mediatorMock.Verify(m => m.Send(It.IsAny<ListarAtribuicaoEsporadicaPorFiltrosQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterFuncionariosPorRFsQuery>(), It.IsAny<CancellationToken>()), Times.Never); 
        }
    }
}
