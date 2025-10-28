using FluentAssertions;
using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterReclassificacao;
using SME.SGP.Infra.Dtos.PainelEducacional.Reclassificacao;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasReclassificacaoPainelEducacionalUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ConsultasReclassificacaoPainelEducacionalUseCase useCase;

        public ConsultasReclassificacaoPainelEducacionalUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ConsultasReclassificacaoPainelEducacionalUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task ObterReclassificacao_Quando_Solicitado_Deve_Enviar_Query_E_Retornar_Resultado()
        {
            var codigoDre = "108300";
            var codigoUe = "019402";
            var anoLetivo = 2023;
            var anoTurma = 9;

            var respostaEsperada = new List<PainelEducacionalReclassificacaoDto>
            {
                new PainelEducacionalReclassificacaoDto
                {
                    Modalidades = new List<ModalidadeReclassificacaoDto>
                    {
                        new ModalidadeReclassificacaoDto
                        {
                            Modalidade = new ModalidadeReclassificacaoArrayDto
                            {
                                AnoTurma = 9,
                                QuantidadeAlunos = 10
                            }
                        }
                    }
                }
            };

            mediatorMock.Setup(m => m.Send(
                It.Is<ObterReclassificacaoQuery>(q =>
                    q.CodigoDre == codigoDre &&
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.AnoTurma == anoTurma),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(respostaEsperada);

            var resultado = await useCase.ObterReclassificacao(codigoDre, codigoUe, anoLetivo, anoTurma);

            resultado.Should().NotBeNull();
            resultado.Should().BeSameAs(respostaEsperada);

            mediatorMock.Verify(m => m.Send(
                It.Is<ObterReclassificacaoQuery>(q =>
                    q.CodigoDre == codigoDre &&
                    q.CodigoUe == codigoUe &&
                    q.AnoLetivo == anoLetivo &&
                    q.AnoTurma == anoTurma),
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
