using MediatR;
using Moq;
using SME.SGP.Aplicacao.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasVisaoUeUseCaseTeste
    {
        private readonly Mock<IMediator> mockMediator;
        private readonly Mock<IContextoAplicacao> mockContextoAplicacao;
        private readonly ConsultasNotasVisaoUeUseCase useCase;

        public ConsultasNotasVisaoUeUseCaseTeste()
        {
            mockMediator = new Mock<IMediator>();
            mockContextoAplicacao = new Mock<IContextoAplicacao>();
            useCase = new ConsultasNotasVisaoUeUseCase(mockContextoAplicacao.Object, mockMediator.Object);
        }

        [Fact]
        public void Executar_Quando_Instanciar_Mediator_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasNotasVisaoUeUseCase(mockContextoAplicacao.Object, null));
        }

        [Fact]
        public void Executar_Quando_Instanciar_Contexto_Aplicacao_Nulo_Deve_Lancar_Argument_Null_Exception()
        {
            Assert.Throws<ArgumentNullException>(() => new ConsultasNotasVisaoUeUseCase(null, mockMediator.Object));
        }

        [Fact]
        public async Task Executar_Quando_Obter_Notas_Visao_Ue_Com_Paginacao_Valida_Deve_Chamar_Mediator_E_Retornar_Resultado()
        {
            mockContextoAplicacao.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns("2");
            mockContextoAplicacao.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("20");

            var expectedResult = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>();
            ObterNotaVisaoUeQuery queryCapturada = null;

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterNotaVisaoUeQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>, CancellationToken>((query, token) => queryCapturada = query as ObterNotaVisaoUeQuery)
                .ReturnsAsync(expectedResult);

            var result = await useCase.ObterNotasVisaoUe("ue-id", 2024, 1, Modalidade.Fundamental);

            Assert.Same(expectedResult, result);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterNotaVisaoUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(queryCapturada);
            Assert.Equal("ue-id", queryCapturada.CodigoUe);
            Assert.Equal(2024, queryCapturada.AnoLetivo);
            Assert.Equal(1, queryCapturada.Bimestre);
            Assert.Equal(Modalidade.Fundamental, queryCapturada.Modalidade);
            Assert.Equal(20, queryCapturada.Paginacao.QuantidadeRegistros);
            Assert.Equal(20, queryCapturada.Paginacao.QuantidadeRegistrosIgnorados);
        }

        [Fact]
        public async Task Executar_Quando_Obter_Notas_Visao_Ue_Com_Paginacao_Invalida_Deve_Chamar_Mediator_Com_Paginacao_Padrao()
        {
            mockContextoAplicacao.Setup(c => c.ObterVariavel<string>("NumeroPagina")).Returns((string)null);
            mockContextoAplicacao.Setup(c => c.ObterVariavel<string>("NumeroRegistros")).Returns("10");

            var expectedResult = new PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>();
            ObterNotaVisaoUeQuery queryCapturada = null;

            mockMediator
                .Setup(m => m.Send(It.IsAny<ObterNotaVisaoUeQuery>(), It.IsAny<CancellationToken>()))
                .Callback<IRequest<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>>, CancellationToken>((query, token) => queryCapturada = query as ObterNotaVisaoUeQuery)
                .ReturnsAsync(expectedResult);

            var result = await useCase.ObterNotasVisaoUe("ue-id-2", 2024, 2, Modalidade.EJA);

            Assert.Same(expectedResult, result);
            mockMediator.Verify(m => m.Send(It.IsAny<ObterNotaVisaoUeQuery>(), It.IsAny<CancellationToken>()), Times.Once);

            Assert.NotNull(queryCapturada);
            Assert.Equal("ue-id-2", queryCapturada.CodigoUe);
            Assert.Equal(2024, queryCapturada.AnoLetivo);
            Assert.Equal(2, queryCapturada.Bimestre);
            Assert.Equal(Modalidade.EJA, queryCapturada.Modalidade);
            Assert.Equal(0, queryCapturada.Paginacao.QuantidadeRegistros);
            Assert.Equal(0, queryCapturada.Paginacao.QuantidadeRegistrosIgnorados);
        }
    }
}
