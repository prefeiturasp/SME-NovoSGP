using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAEE
{
    public class ObterPlanosAEEVigentesUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterPlanosAEEVigentesUseCase _useCase;

        public ObterPlanosAEEVigentesUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterPlanosAEEVigentesUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Executar deve substituir AnoLetivo = 0 pelo ano atual")]
        public async Task Executar_DeveSubstituirAnoLetivoZero()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;

            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = 5,
                UeId = 0
            };

            var retornoMediator = new DashboardAEEPlanosVigentesDto
            {
                PlanosVigentes = new List<AEETurmaDto>()
            };

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanosAEEVigentesQuery>(q =>
                        q.Ano == anoAtual &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId
                    ),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
            _mediator.Verify(m => m.Send(
                It.Is<ObterPlanosAEEVigentesQuery>(q => q.Ano == anoAtual),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact(DisplayName = "Executar deve manter AnoLetivo informado")]
        public async Task Executar_DeveUsarAnoLetivoInformado()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2022,
                DreId = 1,
                UeId = 0
            };

            var retornoMediator = new DashboardAEEPlanosVigentesDto
            {
                PlanosVigentes = new List<AEETurmaDto>()
            };

            _mediator.Setup(m => m.Send(
                    It.Is<ObterPlanosAEEVigentesQuery>(q =>
                        q.Ano == 2022 &&
                        q.DreId == 1 &&
                        q.UeId == 0),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoMediator);

            // Act
            var result = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(result);
            _mediator.Verify(m => m.Send(
                It.Is<ObterPlanosAEEVigentesQuery>(q => q.Ano == 2022),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact(DisplayName = "Executar deve agrupar corretamente quando UeId = 0")]
        public async Task Executar_DeveAgruparQuandoUeIdZero()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreId = 1,
                UeId = 0
            };

            var planos = new List<AEETurmaDto>
        {
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "3", Quantidade = 10 },
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "3", Quantidade = 5 }
        };

            var retornoMediator = new DashboardAEEPlanosVigentesDto
            {
                PlanosVigentes = planos
            };

            _mediator
                .Setup(m => m.Send(It.IsAny<ObterPlanosAEEVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            var lista = resultado.PlanosVigentes.ToList();
            Assert.Single(lista);
            Assert.Equal(15, lista[0].Quantidade);
        }

        [Fact(DisplayName = "Executar deve mapear turmas corretamente quando UeId > 0")]
        public async Task Executar_DeveMapearParaDtoTurmas_QuandoUeIdMaiorZero()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreId = 1,
                UeId = 999 // ativa MapearParaDtoTurmas
            };

            var planos = new List<AEETurmaDto>
        {
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, Quantidade = 10, Nome = "Turma X" }
        };

            var retornoMediator = new DashboardAEEPlanosVigentesDto
            {
                PlanosVigentes = planos
            };

            _mediator
                .Setup(m => m.Send(It.IsAny<ObterPlanosAEEVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            var lista = resultado.PlanosVigentes.ToList();
            Assert.Single(lista);
            Assert.Equal("EF - Turma X", lista[0].Descricao);
        }

        [Fact(DisplayName = "Executar deve ordenar corretamente")]
        public async Task Executar_DeveOrdenarCorretamente()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreId = 1,
                UeId = 0
            };

            var planos = new List<AEETurmaDto>
        {
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "2", Quantidade = 10 },
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "1", Quantidade = 10 }
        };

            var retornoMediator = new DashboardAEEPlanosVigentesDto
            {
                PlanosVigentes = planos
            };

            _mediator
                .Setup(m => m.Send(It.IsAny<ObterPlanosAEEVigentesQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            var lista = resultado.PlanosVigentes.ToList();
            Assert.Equal("1", lista[0].AnoTurma);
            Assert.Equal("2", lista[1].AnoTurma);
        }
    }
}
