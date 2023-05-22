using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Newtonsoft.Json;
using SME.SGP.Infra;
using Xunit;

namespace SME.SGP.Aplicacao.Teste.CasosDeUso.Pendencias
{
    public class ObterQuantidadeAulaDiaPendenciaPorUeUseCaseTeste
    {
        private readonly ObterQuantidadeAulaDiaPendenciaPorUeUseCase useCase;
        private readonly Mock<IMediator> mediator;

        public ObterQuantidadeAulaDiaPendenciaPorUeUseCaseTeste()
        {
            mediator = new Mock<IMediator>();
            useCase = new ObterQuantidadeAulaDiaPendenciaPorUeUseCase(mediator.Object);
        }

        [Fact(DisplayName = "Deve Obter Pendencias Por ue para realziar a carga na tabela de pendencias")]
        public async Task Deve_obter_pendencias_por_ue_para_realizar_carga()
        {
            var pendencias = new List<AulasDiasPendenciaDto>() {new AulasDiasPendenciaDto{PendenciaId = 1,QuantidadeAulas = 1,QuantidadeDias = 1}};
            mediator.Setup(a => a.Send(It.IsAny<ObterPendenciasParaInserirAulasEDiasQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(pendencias);

            var filtro = new ObterQuantidadeAulaDiaPendenciaDto {AnoLetivo = null, UeId = 1};
            var retornoUsecase = await useCase.Executar(new MensagemRabbit() { Mensagem = JsonConvert.SerializeObject(filtro) });
            Assert.True(retornoUsecase);
        }
    }
}