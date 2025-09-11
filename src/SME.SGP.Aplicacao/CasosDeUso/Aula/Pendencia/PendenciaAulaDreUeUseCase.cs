using MediatR;
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

            if (filtro.NaoEhNulo())
            {
                var ignorarGeracaoPendencia = await mediator.Send(new ObterTipoUeIgnoraGeracaoPendenciasQuery(filtro.TipoEscola, filtro.CodigoUe));
                if (!ignorarGeracaoPendencia)
                {
                    await VerificaPendenciasDiarioDeBordo(filtro);
                    await VerificaPendenciasFrequencia(filtro);
                }

                await VerificaPendenciasAvaliacao(filtro);
                await VerificaPendenciasPlanoAula(filtro);
                await VerificaPendenciasDiarioClasseFechamento(filtro);
                await VerificaPendenciasTurmasComponentesSemAulas(filtro);

                return true;
            }

            return false;
        }

        private async Task VerificaPendenciasTurmasComponentesSemAulas(Ue ue)
        {
            var executar = await mediator.Send(PodeExecutarPendenciaComponenteSemAulaQuery.Instance);
            if (executar)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasTurmasComponenteSemAulaUe, new DreUeDto(ue.DreId, ue.Id, ue.CodigoUe)));
        }

        private async Task VerificaPendenciasDiarioClasseFechamento(Ue ue)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaAvaliarPendenciasAulaDiarioClasseFechamento, new DreUeDto(ue.DreId, ue.Id, ue.CodigoUe)));
        }

        private async Task VerificaPendenciasDiarioDeBordo(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaDiarioBordo, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordo, new DreUeDto(ue.DreId, ue.Id,ue.CodigoUe)));
        }

        private async Task VerificaPendenciasAvaliacao(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaAvaliacao, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaAvaliacao, new DreUeDto(ue.DreId, ue.Id)));
        }

        private async Task VerificaPendenciasFrequencia(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaAulaFrequencia, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaFrequencia, new DreUeDto(ue.DreId, ue.Id)));
        }

        private async Task VerificaPendenciasPlanoAula(Ue ue)
        {
            var dadosParametro = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecutaPendenciaPlanoAula, DateTimeExtension.HorarioBrasilia().Year));

            if (dadosParametro?.Ativo == true)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaPlanoAula, new DreUeDto(ue.DreId, ue.Id)));
        }
    }
}
