using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommandHandler : IRequestHandler<CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommand, bool>
    {
        private readonly IRepositorioParametrosSistema repositorioParametrosSistema;
        private readonly IMediator mediator;

        public CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommandHandler(IRepositorioParametrosSistema repositorioParametrosSistema,
                                                           IMediator mediator)
        {
            this.repositorioParametrosSistema = repositorioParametrosSistema ?? throw new ArgumentNullException(nameof(repositorioParametrosSistema));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(CriarPeriodosConfiguracaoRelatorioPAPAnoAtualCommand request, CancellationToken cancellationToken)
        {
            var tipoCalendario = await mediator.Send(new ObterTipoCalendarioPorIdQuery(request.TipoCalendarioId));
            if (tipoCalendario.NaoEhNulo()
                && tipoCalendario.Modalidade == ModalidadeTipoCalendario.FundamentalMedio)
            {
                await repositorioParametrosSistema.CriarParametrosPeriodosConfiguracaoRelatorioPeriodicoPAPAnoAtualAsync(tipoCalendario.AnoLetivo);
                return true;
            }
            return false;
        }
    }
}
