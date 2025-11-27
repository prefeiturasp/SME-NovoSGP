using MediatR;
using Moq;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAEE
{
    public class ObterAlunosMatriculadosSRMPAEEUseCaseTeste
    {
        private readonly Mock<IMediator> _mediator;
        private readonly ObterAlunosMatriculadosSRMPAEEUseCase _useCase;

        public ObterAlunosMatriculadosSRMPAEEUseCaseTeste()
        {
            _mediator = new Mock<IMediator>();
            _useCase = new ObterAlunosMatriculadosSRMPAEEUseCase(_mediator.Object);
        }

        [Fact(DisplayName = "Executar deve substituir AnoLetivo = 0 pelo ano atual")]
        public async Task Executar_DeveSubstituirAnoZero()
        {
            // Arrange
            var anoAtual = DateTime.Now.Year;

            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 0,
                DreCodigo = "10",
                UeCodigo = null
            };

            _mediator.Setup(m => m.Send(
                It.Is<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(q =>
                    q.Ano == anoAtual &&
                    q.DreCodigo == filtro.DreCodigo
                ),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosMatriculadosEolDto>());

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.Null(resultado);

            _mediator.Verify(m => m.Send(
                It.Is<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(q => q.Ano == anoAtual),
                It.IsAny<CancellationToken>()),
            Times.Once);
        }

        [Fact(DisplayName = "Executar deve retornar null quando não houver alunos")]
        public async Task Executar_DeveRetornarNull_QuandoSemAlunos()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreCodigo = "10",
                UeCodigo = ""
            };

            _mediator.Setup(m => m.Send(
                It.IsAny<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AlunosMatriculadosEolDto>());

            // Act
            var resultado = await _useCase.Executar(filtro);

            // Assert
            Assert.Null(resultado);
        }

        [Fact(DisplayName = "Executar deve mapear corretamente usando MapearParaDto quando UeCodigo for nulo")]
        public async Task Executar_DeveMapearParaDto_QuandoUeCodigoNulo()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreCodigo = "10",
                UeCodigo = null
            };

            var listaEol = new List<AlunosMatriculadosEolDto>
        {
            new AlunosMatriculadosEolDto { Modalidade = "EF", Ano = "3", Quantidade = 2, Ordem = 1, ComponenteCurricularId = 1030 },
            new AlunosMatriculadosEolDto { Modalidade = "EF", Ano = "3", Quantidade = 3, Ordem = 1, ComponenteCurricularId = 1310 }
        };

            _mediator.Setup(m => m.Send(
                It.IsAny<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEol);

            // Act
            var resultado = (await _useCase.Executar(filtro)).ToList();

            // Assert
            Assert.Single(resultado);
            Assert.Equal(1, resultado[0].Ordem);
            Assert.Equal("EF - 3", resultado[0].Descricao);
            Assert.Equal(3, resultado[0].QuantidadePAEE);  // 1310
            Assert.Equal(2, resultado[0].QuantidadeSRM);   // 1030
        }

        [Fact(DisplayName = "Executar deve mapear para turmas usando MapearParaDtoTurmas quando UeCodigo for informado")]
        public async Task Executar_DeveMapearParaTurmas_QuandoUeCodigoInformado()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreCodigo = "10",
                UeCodigo = "0001" // ativa MapearParaDtoTurmas
            };

            var listaEol = new List<AlunosMatriculadosEolDto>
        {
            new AlunosMatriculadosEolDto { Modalidade = "EF", Turma = "T1", Quantidade = 5, Ordem = 2, ComponenteCurricularId = 1310 },
            new AlunosMatriculadosEolDto { Modalidade = "EF", Turma = "T1", Quantidade = 4, Ordem = 2, ComponenteCurricularId = 1030 }
        };

            _mediator.Setup(m => m.Send(
                It.IsAny<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEol);

            // Act
            var resultado = (await _useCase.Executar(filtro)).ToList();

            // Assert
            Assert.Single(resultado);
            Assert.Equal("EF - T1", resultado[0].Descricao);
            Assert.Equal(5, resultado[0].QuantidadePAEE);
            Assert.Equal(4, resultado[0].QuantidadeSRM);
        }

        [Fact(DisplayName = "Executar deve ordenar por Ordem e depois por Descricao")]
        public async Task Executar_DeveOrdenarCorretamente()
        {
            // Arrange
            var filtro = new FiltroDashboardAEEDto
            {
                AnoLetivo = 2023,
                DreCodigo = "10",
                UeCodigo = null
            };

            var listaEol = new List<AlunosMatriculadosEolDto>
        {
            new AlunosMatriculadosEolDto { Modalidade = "EF", Ano = "2", Quantidade = 1, Ordem = 2, ComponenteCurricularId = 1030 },
            new AlunosMatriculadosEolDto { Modalidade = "EF", Ano = "1", Quantidade = 1, Ordem = 1, ComponenteCurricularId = 1030 }
        };

            _mediator
                .Setup(m => m.Send(It.IsAny<ObterAlunosMatriculadosPorAnoLetivoECCEolQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(listaEol);

            // Act
            var resultado = (await _useCase.Executar(filtro)).ToList();

            // Assert
            Assert.Equal("EF - 1", resultado[0].Descricao);
            Assert.Equal("EF - 2", resultado[1].Descricao);
        }
    }
}
