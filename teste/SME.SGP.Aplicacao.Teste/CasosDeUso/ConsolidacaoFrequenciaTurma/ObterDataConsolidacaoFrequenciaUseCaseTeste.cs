using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.ConsolidacaoFrequenciaTurma
{
    public class ObterDataConsolidacaoFrequenciaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterDataConsolidacaoFrequenciaUseCase useCase;

        public ObterDataConsolidacaoFrequenciaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterDataConsolidacaoFrequenciaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Data_Quando_ParametroPossuiValor()
        {
            var anoLetivo = 2025;
            var dataEsperada = new DateTime(2025, 07, 18);
            var parametroSistema = new ParametrosSistema { Valor = dataEsperada.ToString("yyyy-MM-dd") };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroSistema);

            var resultado = await useCase.Executar(anoLetivo);

            Assert.NotNull(resultado);
            Assert.Equal(dataEsperada, resultado.Value);
        }

        [Fact]
        public async Task Executar_Deve_Retornar_Null_Quando_ParametroSemValor()
        {
            var anoLetivo = 2025;
            var parametroSistema = new ParametrosSistema { Valor = null };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(parametroSistema);

            var resultado = await useCase.Executar(anoLetivo);

            Assert.Null(resultado);
        }
    }
}
