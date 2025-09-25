using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitora;
using SME.SGP.Dominio.Entidades;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasPainelEducacionalFluenciaLeitoraUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ConsultasPainelEducacionalFluenciaLeitoraUseCase _useCase;

        public ConsultasPainelEducacionalFluenciaLeitoraUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ConsultasPainelEducacionalFluenciaLeitoraUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Obter_Fluencia_Leitora_Deve_Chamar_Mediator_Send_Com_Query_Correta()
        {
            var periodo = 1;
            var anoLetivo = 2023;
            var codigoDre = "108900";
            var resultadoEsperado = CriarListaPainelEducacionalFluenciaLeitoraDto();

            _mediatorMock.Setup(m => m.Send(
                It.Is<PainelEducacionalFluenciaLeitoraQuery>(q =>
                    q.Periodo == periodo &&
                    q.AnoLetivo == anoLetivo &&
                    q.CodigoDre == codigoDre),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await _useCase.ObterFluenciaLeitora(periodo, anoLetivo, codigoDre);

            Assert.NotNull(resultado);
            Assert.Equal(resultadoEsperado, resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<PainelEducacionalFluenciaLeitoraQuery>(q =>
                    q.Periodo == periodo &&
                    q.AnoLetivo == anoLetivo &&
                    q.CodigoDre == codigoDre),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task Obter_Fluencia_Leitora_Com_Parametros_Diferentes_Deve_Funcionar_Corretamente()
        {
            var periodo = 2;
            var anoLetivo = 2024;
            var codigoDre = "108901";
            var resultadoEsperado = new List<PainelEducacionalFluenciaLeitoraDto>();

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<PainelEducacionalFluenciaLeitoraQuery>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            var resultado = await _useCase.ObterFluenciaLeitora(periodo, anoLetivo, codigoDre);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);

            _mediatorMock.Verify(m => m.Send(
                It.Is<PainelEducacionalFluenciaLeitoraQuery>(q =>
                    q.Periodo == periodo &&
                    q.AnoLetivo == anoLetivo &&
                    q.CodigoDre == codigoDre),
                It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public void Constructor_Deve_Inicializar_Com_Mediator()
        {
            var useCase = new ConsultasPainelEducacionalFluenciaLeitoraUseCase(_mediatorMock.Object);

            Assert.NotNull(useCase);
        }

        [Fact]
        public void Construtor_Com_Mediator_Null_Deve_Permitir_Instanciacao()
        {
            var useCase = new ConsultasPainelEducacionalFluenciaLeitoraUseCase(null);
            Assert.NotNull(useCase);
        }

        [Fact]
        public async Task Obter_Fluencia_Leitora_Deve_Propagar_Excecao_Do_Mediator()
        {
            var periodo = 1;
            var anoLetivo = 2023;
            var codigoDre = "108900";
            var excecaoEsperada = new Exception("Erro no mediator");

            _mediatorMock.Setup(m => m.Send(
                It.IsAny<PainelEducacionalFluenciaLeitoraQuery>(),
                It.IsAny<CancellationToken>()))
                .ThrowsAsync(excecaoEsperada);

            var excecao = await Assert.ThrowsAsync<Exception>(() =>
                _useCase.ObterFluenciaLeitora(periodo, anoLetivo, codigoDre));

            Assert.Equal(excecaoEsperada.Message, excecao.Message);
        }

        private static IEnumerable<PainelEducacionalFluenciaLeitoraDto> CriarListaPainelEducacionalFluenciaLeitoraDto()
        {
            return new List<PainelEducacionalFluenciaLeitoraDto>
            {
                new PainelEducacionalFluenciaLeitoraDto
                {
                    NomeFluencia = "Fluência Adequada",
                    DescricaoFluencia = "Leitura fluente adequada para a idade",
                    DreCodigo = "108900",
                    Percentual = 75.5m,
                    QuantidadeAlunos = 150,
                    Ano = 2023,
                    Periodo = "1º Semestre"
                },
                new PainelEducacionalFluenciaLeitoraDto
                {
                    NomeFluencia = "Fluência em Desenvolvimento",
                    DescricaoFluencia = "Leitura em processo de desenvolvimento",
                    DreCodigo = "108900",
                    Percentual = 24.5m,
                    QuantidadeAlunos = 50,
                    Ano = 2023,
                    Periodo = "1º Semestre"
                }
            };
        }
    }
}