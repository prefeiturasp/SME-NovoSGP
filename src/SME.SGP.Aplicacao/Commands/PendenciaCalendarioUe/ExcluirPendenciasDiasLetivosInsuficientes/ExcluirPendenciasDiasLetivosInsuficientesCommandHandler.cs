using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasDiasLetivosInsuficientesCommandHandler : IRequestHandler<ExcluirPendenciasDiasLetivosInsuficientesCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe;

        public ExcluirPendenciasDiasLetivosInsuficientesCommandHandler(IMediator mediator, IRepositorioPendenciaCalendarioUe repositorioPendenciaCalendarioUe)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaCalendarioUe = repositorioPendenciaCalendarioUe ?? throw new ArgumentNullException(nameof(repositorioPendenciaCalendarioUe));
        }

        public async Task<bool> Handle(ExcluirPendenciasDiasLetivosInsuficientesCommand request, CancellationToken cancellationToken)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorIdQuery(request.TipoCalendarioId));
            var diasLetivosParametro = await ObterParametroDiasLetivos(tipoCalendario);
            if (diasLetivosParametro == 0)
                return true;

            var periodosEscolares = await mediator.Send(new ObterPeridosEscolaresPorTipoCalendarioIdQuery(request.TipoCalendarioId));
            var diasLetivosENaoLetivos = await mediator.Send(new ObterDiasPorPeriodosEscolaresComEventosLetivosENaoLetivosQuery(periodosEscolares, request.TipoCalendarioId));

            foreach(var ue in await ObterUesEvento(request.DreCodigo, request.UeCodigo, tipoCalendario))
            {
                if (await mediator.Send(new ExistePendenciaDiasLetivosCalendarioUeQuery(tipoCalendario.Id, ue.Id)))
                {
                    var diasLetivos = await ObterDiasLetivosUe(ue, diasLetivosENaoLetivos);
                    if (diasLetivos >= diasLetivosParametro)
                        await ExcluirPendenciaCalendarioUe(ue.Id, tipoCalendario.Id);
                }
            }

            return true;
        }

        private async Task ExcluirPendenciaCalendarioUe(long ueId, long tipoCalendarioId)
        {
            var pendenciaCalendario = await repositorioPendenciaCalendarioUe.ObterPendenciaPorCalendarioUe(tipoCalendarioId, ueId, TipoPendencia.CalendarioLetivoInsuficiente);
            repositorioPendenciaCalendarioUe.Remover(pendenciaCalendario);

            await mediator.Send(new ExcluirPendenciaPorIdCommand(pendenciaCalendario.PendenciaId));
        }

        private async Task<int> ObterParametroDiasLetivos(TipoCalendario tipoCalendario)
        {
            return tipoCalendario.Modalidade == ModalidadeTipoCalendario.EJA ?
                int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(Dominio.TipoParametroSistema.EjaDiasLetivos, tipoCalendario.AnoLetivo))) :
                tipoCalendario.Modalidade == ModalidadeTipoCalendario.FundamentalMedio ?
                int.Parse(await mediator.Send(new ObterValorParametroSistemaTipoEAnoQuery(Dominio.TipoParametroSistema.FundamentalMedioDiasLetivos, tipoCalendario.AnoLetivo))) :
                0;
        }

        private async Task<int> ObterDiasLetivosUe(Ue ue, List<Infra.DiaLetivoDto> diasLetivosENaoLetivos)
        {
            return await mediator.Send(new ObterDiasLetivosDaUeQuery(diasLetivosENaoLetivos, ue.Dre.CodigoDre, ue.CodigoUe));
        }

        private async Task<IEnumerable<Ue>> ObterUesEvento(string dreCodigo, string ueCodigo, TipoCalendario tipoCalendario)
        {
            if (!string.IsNullOrEmpty(ueCodigo))
                return new List<Ue>() { await mediator.Send(new ObterUeComDrePorCodigoQuery(ueCodigo)) };

            if (!string.IsNullOrEmpty(dreCodigo))
                return await mediator.Send(new ObterUesComDrePorCodigoEModalidadeQuery(dreCodigo, tipoCalendario.ObterModalidadeTurma()));

            return await mediator.Send(new ObterUEsPorModalidadeCalendarioQuery(tipoCalendario.Modalidade));
        }
    }
}
