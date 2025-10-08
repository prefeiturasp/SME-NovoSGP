using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIndicadoresPap;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.PainelEducacional.IndicadoresPap;
using System.Collections.Generic;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasInformacoesPapUseCaseTeste
    {
        private readonly ConsultasInformacoesPapUseCase consultasInformacoesPapUseCase;
        private readonly Mock<IMediator> mediatorMock;

        public ConsultasInformacoesPapUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            consultasInformacoesPapUseCase = new ConsultasInformacoesPapUseCase(mediatorMock.Object);
        }

        [Fact]
        public void DeveInstanciarUseCase()
        {
            Assert.NotNull(consultasInformacoesPapUseCase);
        }

        [Fact]
        public async void ObterInformacoesPap_QuandoChamado_DeveRetornarIndicadoresPap()
        {
            // Arrange
            var anoLetivo = 2023;
            var codigoDre = "01";
            var codigoUe = "0001";

            var indicadoresPapDto = new IndicadoresPapDto
            {
                NomeDificuldadeTop1 = "Dificuldade 1",
                NomeDificuldadeTop2 = "Dificuldade 2",
                QuantidadesPorTipoPap = new List<IndicadoresPapQuantidadesPorTipoDto>
                {
                    new IndicadoresPapQuantidadesPorTipoDto
                    {
                        TipoPap = TipoPap.Pap2Ano,
                        TotalAlunos = 10,
                        TotalAlunosComFrequenciaInferiorLimite = 2,
                        TotalAlunosDificuldadeOutras = 1,
                        TotalAlunosDificuldadeTop1 = 5,
                        TotalAlunosDificuldadeTop2 = 2,
                        TotalTurmas = 3
                    },
                    new IndicadoresPapQuantidadesPorTipoDto
                    {
                        TipoPap = TipoPap.PapColaborativo,
                        TotalAlunos = 20,
                        TotalAlunosComFrequenciaInferiorLimite = 12,
                        TotalAlunosDificuldadeOutras = 11,
                        TotalAlunosDificuldadeTop1 = 5,
                        TotalAlunosDificuldadeTop2 = 11,
                        TotalTurmas = 13
                    },
                    new IndicadoresPapQuantidadesPorTipoDto
                    {
                        TipoPap = TipoPap.RecuperacaoAprendizagens,
                        TotalAlunos = 22,
                        TotalAlunosComFrequenciaInferiorLimite = 10,
                        TotalAlunosDificuldadeOutras = 10,
                        TotalAlunosDificuldadeTop1 = 5,
                        TotalAlunosDificuldadeTop2 = 1,
                        TotalTurmas = 14
                    }
                },
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterIndicadoresPapQuery>(), default))
                .ReturnsAsync(indicadoresPapDto);

            // Act
            var retorno = await consultasInformacoesPapUseCase.ObterInformacoesPap(anoLetivo, codigoDre, codigoUe);

            // Assert
            Assert.Equal("Dificuldade 1", retorno.NomeDificuldadeTop1);
            Assert.Equal("Dificuldade 2", retorno.NomeDificuldadeTop2);
            Assert.Equal(3, retorno.QuantidadesPorTipoPap.Count);
            Assert.Collection(retorno.QuantidadesPorTipoPap,
                item =>
                {
                    Assert.Equal(TipoPap.Pap2Ano, item.TipoPap);
                    Assert.Equal(10, item.TotalAlunos);
                    Assert.Equal(2, item.TotalAlunosComFrequenciaInferiorLimite);
                    Assert.Equal(1, item.TotalAlunosDificuldadeOutras);
                    Assert.Equal(5, item.TotalAlunosDificuldadeTop1);
                    Assert.Equal(2, item.TotalAlunosDificuldadeTop2);
                    Assert.Equal(3, item.TotalTurmas);
                },
                item =>
                {
                    Assert.Equal(TipoPap.PapColaborativo, item.TipoPap);
                    Assert.Equal(20, item.TotalAlunos);
                    Assert.Equal(12, item.TotalAlunosComFrequenciaInferiorLimite);
                    Assert.Equal(11, item.TotalAlunosDificuldadeOutras);
                    Assert.Equal(5, item.TotalAlunosDificuldadeTop1);
                    Assert.Equal(11, item.TotalAlunosDificuldadeTop2);
                    Assert.Equal(13, item.TotalTurmas);
                },
                item =>
                {
                    Assert.Equal(TipoPap.RecuperacaoAprendizagens, item.TipoPap);
                    Assert.Equal(22, item.TotalAlunos);
                    Assert.Equal(10, item.TotalAlunosComFrequenciaInferiorLimite);
                    Assert.Equal(10, item.TotalAlunosDificuldadeOutras);
                    Assert.Equal(5, item.TotalAlunosDificuldadeTop1);
                    Assert.Equal(1, item.TotalAlunosDificuldadeTop2);
                    Assert.Equal(14, item.TotalTurmas);
                });

            mediatorMock.Verify(m => m.Send(It.IsAny<ObterIndicadoresPapQuery>(), default), Times.Once);
        }
    }
}
