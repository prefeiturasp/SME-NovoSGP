using MediatR;
using Moq;
using SME.SGP.Dominio;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.DashboardDevolutivas
{
    public class ObterUltimaConsolidacaoDevolutivaUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterUltimaConsolidacaoDevolutivaUseCase useCase;

        public ObterUltimaConsolidacaoDevolutivaUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterUltimaConsolidacaoDevolutivaUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Executar_Quando_ParametroPossuiValor_Deve_Retornar_Data()
        {
            var anoLetivo = 2025;
            var dataEsperada = DateTime.Today;
            var parametro = new ParametrosSistema
            {
                Tipo = TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma,
                Ano = anoLetivo,
                Valor = dataEsperada.ToString("yyyy-MM-dd")
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametro);

            var resultado = await useCase.Executar(anoLetivo);

            Assert.NotNull(resultado);
            Assert.Equal(dataEsperada, resultado.Value.Date);
            mediatorMock.Verify(m => m.Send(It.Is<ObterParametroSistemaPorTipoEAnoQuery>(q =>
                q.TipoParametroSistema == TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma &&
                q.Ano == anoLetivo
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Executar_Quando_ParametroNaoPossuiValor_Deve_Retornar_Null()
        {
            var anoLetivo = 2025;
            var parametro = new ParametrosSistema
            {
                Tipo = TipoParametroSistema.ExecucaoConsolidacaoDevolutivasTurma,
                Ano = anoLetivo,
                Valor = string.Empty
            };

            mediatorMock.Setup(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync(parametro);

            var resultado = await useCase.Executar(anoLetivo);

            Assert.Null(resultado);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterParametroSistemaPorTipoEAnoQuery>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
