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

            foreach(var ue in await ObterUesEvento(request.DreCodigo, request.UeCodigo, tipoCalendario))
            {
                if (await mediator.Send(new ExistePendenciaDiasLetivosCalendarioUeQuery(tipoCalendario.Id, ue.Id)))
                {
                    var diasLetivos = await mediator.Send(new ObterQuantidadeDiasLetivosPorCalendarioQuery(request.TipoCalendarioId, ue.Dre.CodigoDre, ue.CodigoUe));
                    if (!diasLetivos.EstaAbaixoPermitido)
                        await mediator.Send(new ExcluirPendenciaCalendarioUeCommand(tipoCalendario.Id, ue.Id, TipoPendencia.CalendarioLetivoInsuficiente));
                }
            }

            return true;
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
