using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNumeroAlunos;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Entidades;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase useCase;

        public ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasNumeroEstudantesAgrupamentoNivelAlfabetizacaoUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Estudantes_Quando_Mediator_Retornar_Dados()
        {
            // Arrange
            var anoLetivo = 2025;
            var periodo = 1;
            var resultadoEsperado = new List<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>
            {
                new PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto
                {
                    Ano = 1,
                    NivelAlfabetizacao = NivelAlfabetizacao.PreSilabico,
                    NivelAlfabetizacaoDescricao = "Alfabetização Básica",
                    TotalAlunos = 30,
                    Periodo = 1
                }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(resultadoEsperado);

            // Act
            var resultado = await useCase.ObterNumeroEstudantes(anoLetivo, periodo);

            // Assert
            Assert.NotNull(resultado);
            Assert.NotEmpty(resultado);
            Assert.Equal(resultadoEsperado, resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Deve_Retornar_Vazio_Quando_Mediator_Retornar_Vazio()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoDto>());

            var resultado = await useCase.ObterNumeroEstudantes(2025, 1);

            Assert.NotNull(resultado);
            Assert.Empty(resultado);
        }

        [Fact]
        public async Task Deve_Propagar_Excecao_Quando_Mediator_Lancar_Erro()
        {
            mediatorMock
                .Setup(m => m.Send(It.IsAny<PainelEducacionalNumeroEstudantesAgrupamentoNivelAlfabetizacaoQuery>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new System.Exception("Erro no Mediator"));

            await Assert.ThrowsAsync<System.Exception>(() => useCase.ObterNumeroEstudantes(2025, 1));
        }
    }
}
