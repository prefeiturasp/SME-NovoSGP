using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Enumerados;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarConsolidacaoFrequenciaNoAnoCommandHandler : AsyncRequestHandler<ExecutarConsolidacaoFrequenciaNoAnoCommand>
    {
        private readonly IMediator mediator;

        public ExecutarConsolidacaoFrequenciaNoAnoCommandHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        protected override async Task Handle(ExecutarConsolidacaoFrequenciaNoAnoCommand request, CancellationToken cancellationToken)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno, new FiltroAnoDto(request.Data, TipoConsolidadoFrequencia.Anual), Guid.NewGuid(), null));
            await AtualizarDataExecucao(request.Data.Year);
        }

        private async Task AtualizarDataExecucao(int ano)
        {
            var parametroSistema = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.ExecucaoConsolidacaoFrequenciaTurma, ano));
            if (parametroSistema.NaoEhNulo())
            {
                parametroSistema.Valor = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff tt");

                await mediator.Send(new AtualizarParametroSistemaCommand(parametroSistema));
            }
        }
    }
}
