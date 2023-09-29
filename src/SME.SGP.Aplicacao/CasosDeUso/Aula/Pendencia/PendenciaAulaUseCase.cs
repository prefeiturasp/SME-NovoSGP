using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaUseCase : AbstractUseCase, IPendenciaAulaUseCase
    {
        public PendenciaAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dadosParametroGeracaoPendencias = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.DataInicioGeracaoPendencias, DateTimeExtension.HorarioBrasilia().Year));
            
            if(dadosParametroGeracaoPendencias.NaoEhNulo())
            {
                DateTime dataDefinida = Convert.ToDateTime(dadosParametroGeracaoPendencias.Valor);

                if (DateTimeExtension.HorarioBrasilia() >= dataDefinida)
                {
                    var dres = await mediator.Send(ObterIdsDresQuery.Instance);

                    foreach (var dreId in dres)
                        await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDre, new DreDto(dreId)));

                    return true;
                }
            }

            return false;
        }
    }
}
