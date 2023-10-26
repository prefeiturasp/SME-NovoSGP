using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaDreUseCase : AbstractUseCase, IPendenciaAulaDreUseCase
    {
        public PendenciaAulaDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreDto>();

            if (filtro.NaoEhNulo())
            {
                var uesDre = await CarregarUesPorDreCodigo(filtro.DreCodigo); 

                foreach (var ue in uesDre)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDreUe, ue));

                return true;
            }

            return false;
        }

        private Task<IEnumerable<Ue>> CarregarUesPorDreCodigo(long dreId)
           => mediator.Send(new ObterUesPorDreCodigoQuery(dreId));

    }
}
