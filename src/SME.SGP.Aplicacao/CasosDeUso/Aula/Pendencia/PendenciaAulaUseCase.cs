using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaUseCase : AbstractUseCase, IPendenciaAulaUseCase
    {
        public PendenciaAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        #region Metodos Publicos

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await VerificaPendenciasDiarioDeBordo();
            await VerificaPendenciasAvaliacao();
            await VerificaPendenciasFrequencia();
            await VerificaPendenciasPlanoAula();
            return true;
        }
        #endregion

        #region Metodos Privados
        private async Task VerificaPendenciasDiarioDeBordo()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaDiarioBordo, null));
        }

        private async Task VerificaPendenciasAvaliacao()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaAvaliacao, null));
        }

        private async Task VerificaPendenciasFrequencia()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaFrequencia, null));
        }

        private async Task VerificaPendenciasPlanoAula()
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaPlanoAula, null));
        }
        #endregion
    }
}
