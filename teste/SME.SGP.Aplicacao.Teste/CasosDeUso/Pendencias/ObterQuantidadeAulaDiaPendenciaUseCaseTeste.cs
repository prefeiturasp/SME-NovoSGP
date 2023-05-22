using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class ObterQuantidadeAulaDiaPendenciaUseCaseTeste
    {
        private readonly ObterQuantidadeAulaDiaPendenciaUseCase obterQuantidadeAulaDiaPendenciaUseCase;
        private readonly Mock<IMediator> mediator;

        public ObterQuantidadeAulaDiaPendenciaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            obterQuantidadeAulaDiaPendenciaUseCase = new ObterQuantidadeAulaDiaPendenciaUseCase(mediator.Object);
        }
        
        [Fact(DisplayName = "Deve Executar Carga de Totalizadores Na Tabela de Pendencia")]
        public async Task Deve_obter_pendencias_para_realizar_carga()
        {
            var idsUes = new List<long> {1};

            mediator.Setup(a => a.Send(It.IsAny<ObterTodasUesIdsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(idsUes);

            var retornoUsecase = await obterQuantidadeAulaDiaPendenciaUseCase.Executar(new MensagemRabbit(""));
            Assert.True(retornoUsecase);
        }
    }
}