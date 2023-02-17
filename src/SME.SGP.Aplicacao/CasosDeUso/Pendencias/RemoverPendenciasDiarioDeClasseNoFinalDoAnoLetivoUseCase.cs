using MediatR;
using Newtonsoft.Json;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase : AbstractUseCase, IRemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase
    {
        public RemoverPendenciasDiarioDeClasseNoFinalDoAnoLetivoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = JsonConvert.DeserializeObject<FiltroRemoverPendenciaFinalAnoLetivoDto>(param.Mensagem.ToString());

            if (filtro.UeId > 0) 
            { 

            }

            return true;
        }
    }
}
