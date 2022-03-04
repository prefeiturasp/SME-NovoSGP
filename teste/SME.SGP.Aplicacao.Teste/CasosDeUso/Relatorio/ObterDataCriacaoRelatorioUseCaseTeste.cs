using MediatR;
using Moq;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso
{
    public class ObterDataCriacaoRelatorioUseCaseTeste
    {
        private readonly Mock<IMediator> mediator;
        private readonly ObterDataCriacaoRelatorioUseCase useCase;

        public ObterDataCriacaoRelatorioUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterDataCriacaoRelatorioUseCase(mediator.Object);
        }

        [Fact]
        public async Task Consultar_Relatorio_Com_Mais_De_24Horas()
        {
            var relatorioCorrelacao = new DataCriacaoRelatorioDto
            {
                CriadoEm = DateTime.Parse("2022-03-01"),
            };
            mediator.Setup(a => a.Send(It.IsAny<ObterDataCriacaoRelatorioPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(relatorioCorrelacao);
            var retorno = await useCase.Executar(Guid.NewGuid());

            Assert.False(retorno);
        }

        [Fact]
        public async Task Consultar_Relatorio_Com_Menos_De_24Horas()
        {
            var relatorioCorrelacao = new DataCriacaoRelatorioDto
            {
                CriadoEm = DateTime.Now,
            };

            mediator.Setup(a => a.Send(It.IsAny<ObterDataCriacaoRelatorioPorCodigoQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(relatorioCorrelacao);
            var retorno = await useCase.Executar(Guid.NewGuid());

            Assert.True(retorno);
        }
        [Fact]
        public async Task Consultar_Relatorio_Nao_Encontrado_Na_Base()
        {
            var relatorioCorrelacao = new DataCriacaoRelatorioDto
            {
                CriadoEm = DateTime.Now,
            };

            var retorno = await useCase.Executar(Guid.NewGuid());

            Assert.False(retorno);
        }
    }
}
