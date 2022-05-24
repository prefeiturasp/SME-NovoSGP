﻿using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaDreUeUseCase : AbstractUseCase, IPendenciaAulaDreUeUseCase
    {
        public PendenciaAulaDreUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<Ue>();

            if(filtro != null)
            {
                await VerificaPendenciasDiarioDeBordo(filtro);
                await VerificaPendenciasAvaliacao(filtro);
                await VerificaPendenciasFrequencia(filtro);
                await VerificaPendenciasPlanoAula(filtro);

                return true;
            }

            return false;
        }

        private async Task VerificaPendenciasDiarioDeBordo(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaDiarioBordo, DateTimeExtension.HorarioBrasilia().Year));
            
            if(dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaDiarioBordo, new DreUeDto(ue.DreId, ue.CodigoUe)));
        }

        private async Task VerificaPendenciasAvaliacao(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaAvaliacao, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaAvaliacao, new DreUeDto(ue.DreId, ue.Id)));
        }

        private async Task VerificaPendenciasFrequencia(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaFrequencia, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaFrequencia, new DreUeDto(ue.DreId, ue.Id)));
        }

        private async Task VerificaPendenciasPlanoAula(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaPlanoAula, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaPlanoAula, new DreUeDto(ue.DreId, ue.Id)));
        }
    }
}
