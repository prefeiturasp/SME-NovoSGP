using MediatR;
using SME.SGP.Infra;
using System;
using System.Threading;
using System.Threading.Tasks;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Constantes.MensagensNegocio;

namespace SME.SGP.Aplicacao
{
    public class IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommandHandler : IRequestHandler<IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand, bool>
    {
        private readonly IMediator mediator;

        public IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(IncluirFilaConsolidacaoSemanalMensalDashBoardFrequenciaCommand request, CancellationToken cancellationToken)
        {
            var parametro = await mediator
                .Send(new ObterParametroSistemaPorTipoEAnoQuery(request.EhModalidadeInfantil ? TipoParametroSistema.PercentualFrequenciaMinimaInfantil : TipoParametroSistema.PercentualFrequenciaCritico, request.AnoLetivo));

            var filtroTurma = new FiltroConsolidacaoFrequenciaTurma(request.TurmaId, request.CodigoTurma, double.Parse(parametro.Valor),request.DataAula);
                
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurmaSemanal, filtroTurma, Guid.NewGuid(), null));
                
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurmaMensal, filtroTurma, Guid.NewGuid(), null));

            return true;
        }
    }
}
