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
            var dres = await mediator.Send(new ObterIdsDresQuery());

            foreach(var dreId in dres)
            {
                await VerificaPendenciasDiarioDeBordo(dreId);
                await VerificaPendenciasAvaliacao(dreId);
                await VerificaPendenciasFrequencia(dreId);
                await VerificaPendenciasPlanoAula(dreId);
            }

            return true;
        }
        #endregion

        #region Metodos Privados
        private async Task VerificaPendenciasDiarioDeBordo(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaDiarioBordo, new DreUeDto(dreId)));
        }

        private async Task VerificaPendenciasAvaliacao(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaAvaliacao, new DreUeDto(dreId)));
        }

        private async Task VerificaPendenciasFrequencia(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaFrequencia, new DreUeDto(dreId)));
        }

        private async Task VerificaPendenciasPlanoAula(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaPlanoAula, new DreUeDto(dreId)));
        }
        #endregion
    }
}
