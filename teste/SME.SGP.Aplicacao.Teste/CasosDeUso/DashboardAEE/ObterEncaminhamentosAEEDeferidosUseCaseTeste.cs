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
    public class ObterEncaminhamentosAEEDeferidosUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterEncaminhamentosAEEDeferidosUseCase _useCase;

        public ObterEncaminhamentosAEEDeferidosUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterEncaminhamentosAEEDeferidosUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Executar deve substituir AnoLetivo = 0 pelo ano atual")]
        public async Task Executar_DeveTrocarAnoLetivo_QuandoZero()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;

            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreId = 2,
                UeId = 0
            };

            var retornoMediator = new List<AEETurmaDto>();

            _mediator.Setup(m => m.Send(
                    It.Is<ObterEncaminhamentosAEEDeferidosQuery>(q =>
                        q.Ano == anoAtual &&
                        q.DreId == filtro.DreId &&
                        q.UeId == filtro.UeId
                    ),
                    It.IsAny<CancellationToken>()
                ))
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterEncaminhamentosAEEDeferidosQuery>(q => q.Ano == anoAtual),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }


        [Fact(DisplayName = "Executar deve utilizar o AnoLetivo informado")]
        public async Task Executar_DeveUsarAnoInformado()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreId = 10,
                UeId = 0
            };

            var retornoMediator = new List<AEETurmaDto>();

            _mediator.Setup(m => m.Send(
                    It.Is<ObterEncaminhamentosAEEDeferidosQuery>(q =>
                        q.Ano == DateTime.Now.Year &&
                        q.DreId == 10 &&
                        q.UeId == 0),
                    It.IsAny<CancellationToken>())
                )
                .ReturnsAsync(retornoMediator);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.NotNull(resultado);
        }



        [Fact(DisplayName = "Executar deve mapear por agrupamento (UeId = 0)")]
        public async Task Executar_DeveMapearParaDto_QuandoUeIdZero()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreId = 1,
                UeId = 0
            };

            var encaminhamentos = new List<AEETurmaDto>
        {
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "3", Quantidade = 10 },
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "3", Quantidade = 5 }
        };

            _mediator.Setup(m => m.Send(
                    It.IsAny<ObterEncaminhamentosAEEDeferidosQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(encaminhamentos);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.Single(resultado);
            Assert.Equal(15, resultado.First().Quantidade);
        }



        [Fact(DisplayName = "Executar deve mapear por turma (UeId > 0)")]
        public async Task Executar_DeveMapearParaDtoTurmas_QuandoUeIdMaiorZero()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreId = 1,
                UeId = 123   // ativa MapearParaDtoTurmas
            };

            var encaminhamentos = new List<AEETurmaDto>
        {
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, Quantidade = 10, Nome = "Turma A" }
        };

            _mediator.Setup(m => m.Send(
                    It.IsAny<ObterEncaminhamentosAEEDeferidosQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(encaminhamentos);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.Single(resultado);
            Assert.Equal("EF - Turma A", resultado.First().Descricao);
        }



        [Fact(DisplayName = "Executar deve ordenar corretamente")]
        public async Task Executar_DeveOrdenarCorretamente()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = DateTime.Now.Year,
                DreId = 1,
                UeId = 0
            };

            var encaminhamentos = new List<AEETurmaDto>
        {
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "2", Quantidade = 10 },
            new AEETurmaDto { Modalidade = Modalidade.Fundamental, AnoTurma = "1", Quantidade = 10 }
        };

            _mediator
                .Setup(m => m.Send(
                    It.IsAny<ObterEncaminhamentosAEEDeferidosQuery>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(encaminhamentos);

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            var lista = resultado.ToList();
            Assert.Equal("1", lista[0].AnoTurma);
            Assert.Equal("2", lista[1].AnoTurma);
        }
    }
}
