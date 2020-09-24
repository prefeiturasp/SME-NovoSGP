using MediatR;
using Moq;
using SME.SGP.Dominio;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.Queries
{
    public class ObterAnosLetivosPAPQueryHandlerTeste
    {
        private readonly ObterAnosLetivosPAPQueryHandler query;
        private readonly Mock<IMediator> mediator;

        public ObterAnosLetivosPAPQueryHandlerTeste()
        {
            mediator = new Mock<IMediator>();
            query = new ObterAnosLetivosPAPQueryHandler(mediator.Object);
        }

        [Theory]
        [InlineData(2020, 1)]
        [InlineData(2021, 2)]
        [InlineData(2022, 3)]
        [InlineData(2023, 4)]
        public async Task Deve_Obter_Anos_Letivos(int anoAtual, int qntAnos)
        {
            // Arrange
            var parametrosRetorno = new List<ParametrosSistema>();
            parametrosRetorno.Add(new ParametrosSistema() { Valor = 2020.ToString() });

            mediator.Setup(x => x.Send(It.IsAny<ObterParametrosSistemaPorTiposQuery>(), It.IsAny<CancellationToken>()))
              .ReturnsAsync(parametrosRetorno);

            // Act
            var anos = await query.Handle(new ObterAnosLetivosPAPQuery() { AnoAtual = anoAtual }, new CancellationToken());

            //Assert
            Assert.True(anos.Count() == qntAnos);
            Assert.Contains(anos, a => a.Ano == anoAtual && a.EhSugestao == true);

        }
    }
}
