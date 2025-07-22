using MediatR;
using Moq;
using SME.SGP.Dominio;
using SME.SGP.Infra.Dtos.EscolaAqui.Anos;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.EscolaAqui.Anos
{
    public class ObterAnosPorCodigoUeModalidadeEscolaAquiUseCaseTeste
    {
        private readonly Mock<IMediator> mediatorMock;
        private readonly ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase useCase;

        public ObterAnosPorCodigoUeModalidadeEscolaAquiUseCaseTeste()
        {
            mediatorMock = new Mock<IMediator>();
            useCase = new ObterAnosPorCodigoUeModalidadeEscolaAquiUseCase(mediatorMock.Object);
        }

        [Fact]
        public async Task Deve_Retornar_Todos_Quando_Modalidade_For_Invalida()
        {
            var codigoUe = "123456";
            var modalidades = new[] { 999 }; 

            var resultado = await useCase.Executar(codigoUe, modalidades);

            var item = Assert.Single(resultado);
            Assert.Equal("-99", item.Ano);
            Assert.Equal("Todos", item.Descricao);
            mediatorMock.Verify(m => m.Send(It.IsAny<ObterAnosPorCodigoUeModalidadeQuery>(), default), Times.Never);
        }

        [Fact]
        public async Task Deve_Retornar_Anos_Quando_Existir_Retorno_Da_Query()
        {
            var codigoUe = "123456";
            var modalidades = new[] { (int)Modalidade.Fundamental };

            var retornoEsperado = new List<AnosPorCodigoUeModalidadeEscolaAquiResult>
            {
                new AnosPorCodigoUeModalidadeEscolaAquiResult { Ano = "5" },
                new AnosPorCodigoUeModalidadeEscolaAquiResult { Ano = "9" }
            };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAnosPorCodigoUeModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(retornoEsperado);

            var resultado = await useCase.Executar(codigoUe, modalidades);

            Assert.Equal(2, resultado.Count());
            Assert.Contains(resultado, x => x.Ano == "5");
            Assert.Contains(resultado, x => x.Ano == "9");
        }

        [Fact]
        public async Task Deve_Retornar_Todos_Quando_Query_Retornar_Vazio()
        {
            var codigoUe = "123456";
            var modalidades = new[] { (int)Modalidade.Fundamental };

            mediatorMock
                .Setup(m => m.Send(It.IsAny<ObterAnosPorCodigoUeModalidadeQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<AnosPorCodigoUeModalidadeEscolaAquiResult>());

            var resultado = await useCase.Executar(codigoUe, modalidades);

            var item = Assert.Single(resultado);
            Assert.Equal("-99", item.Ano);
            Assert.Equal("Todos", item.Descricao);
        }
    }
}
