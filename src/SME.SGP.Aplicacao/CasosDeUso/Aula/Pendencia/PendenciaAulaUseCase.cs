﻿using MediatR;
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
            var dadosParametroGeracaoPendencias = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.DataInicioGeracaoPendencias, DateTimeExtension.HorarioBrasilia().Year));
            
            if(dadosParametroGeracaoPendencias != null)
            {
                DateTime dataDefinida = Convert.ToDateTime(dadosParametroGeracaoPendencias.Valor);

                if (DateTimeExtension.HorarioBrasilia() >= dataDefinida)
                {
                    var dres = await mediator.Send(new ObterIdsDresQuery());

                    foreach (var dreId in dres)
                    {
                        await VerificaPendenciasDiarioDeBordo(dreId);
                        await VerificaPendenciasAvaliacao(dreId);
                        await VerificaPendenciasFrequencia(dreId);
                        await VerificaPendenciasPlanoAula(dreId);
                    }

                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Metodos Privados
        private async Task VerificaPendenciasDiarioDeBordo(long dreId)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaDiarioBordo, DateTimeExtension.HorarioBrasilia().Year));
            
            if(dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaDiarioBordo, new DreUeDto(dreId)));
        }

        private async Task VerificaPendenciasAvaliacao(long dreId)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaAvaliacao, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaAvaliacao, new DreUeDto(dreId)));
        }

        private async Task VerificaPendenciasFrequencia(long dreId)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaFrequencia, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaFrequencia, new DreUeDto(dreId)));
        }

        private async Task VerificaPendenciasPlanoAula(long dreId)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaPlanoAula, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaPlanoAula, new DreUeDto(dreId)));
        }
        #endregion
    }
}
