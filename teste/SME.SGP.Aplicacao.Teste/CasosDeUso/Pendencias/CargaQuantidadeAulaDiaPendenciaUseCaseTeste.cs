using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class CargaQuantidadeAulaDiaPendenciaUseCaseTeste
    {
        private readonly CargaQuantidadeAulaDiaPendenciaUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public CargaQuantidadeAulaDiaPendenciaUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new CargaQuantidadeAulaDiaPendenciaUseCase(mediator.Object);
        }

        [Fact(DisplayName = "Deve Atualizar totalizadores Na Tabela de Pendencia")]
        public async Task Deve_atualizar_totalizador_pendencia()
        {
            mediator.Setup(a => a.Send(It.IsAny<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);
            
            var dadosCarga = new AulasDiasPendenciaDto {PendenciaId = 1, QuantidadeAulas = 1,QuantidadeDias = 1};
            var retornoUsecase = await useCase.Executar(new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(dadosCarga) });
            
            mediator.Verify(x => x.Send(It.IsAny<CargaPendenciasQuantidadeDiasQuantidadeAulasCommand>(), It.IsAny<CancellationToken>()), Times.Once);
            Assert.True(retornoUsecase);
        }
    }
}