using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardAcompanhamentoAprendizagem
{
    public class ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCaseTeste
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase _useCase;

        public ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCaseTeste()
        {
            _mediatorMock = new Mock<IMediator>();
            _useCase = new ObterUltimaConsolidacaoAcompanhamentoAprendizagemUseCase(_mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_Parametro_Existe_E_Tem_Valor_Deve_Retornar_DateTime()
        {
            var anoLetivo = 2025;
            var dataString = "2025-09-30T13:28:00";
            var dataEsperada = DateTime.Parse(dataString);
            var parametroRetorno = new ParametrosSistema { Valor = dataString };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.Ano == anoLetivo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroRetorno);

            var resultado = await _useCase.Executar(anoLetivo);

            Assert.NotNull(resultado);
            Assert.Equal(dataEsperada, resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public async Task Executar_Quando_Parametro_Nao_Tem_Valor_Deve_Retornar_Null(string valorInvalido)
        {
            var anoLetivo = 2025;
            var parametroRetorno = new ParametrosSistema { Valor = valorInvalido };

            _mediatorMock.Setup(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q => q.Ano == anoLetivo), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(parametroRetorno);

            var resultado = await _useCase.Executar(anoLetivo);

            Assert.Null(resultado);
            _mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
