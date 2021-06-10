using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using static SME.SGP.Dominio.DateTimeExtension;

namespace SME.SGP.Aplicacao
{
    public class VerificarRemoverProcessoEmExecucaoCommandHandler : IRequestHandler<VerificarRemoverProcessoEmExecucaoCommand, bool>
    {
        private readonly IRepositorioProcessoExecutando repositorio;
        private readonly IMediator mediator;
        public VerificarRemoverProcessoEmExecucaoCommandHandler(IRepositorioProcessoExecutando repositorio, IMediator mediator)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(VerificarRemoverProcessoEmExecucaoCommand request, CancellationToken cancellationToken)
        {
            var processosEmExecucao = await repositorio.ObterProcessosEmExecucaoAsync(request.TurmaId, request.DisciplinaId, request.Bimestre, request.TipoProcesso);
            bool processoExecutando = false;
            foreach(ProcessoExecutando pro in processosEmExecucao)
            {
                var turma = await mediator.Send(new ObterTurmaPorCodigoQuery(request.TurmaId));
                var paramTempoLimiteExecucao = await mediator.Send(new ObterParametroSistemaPorTipoEAnoQuery(TipoParametroSistema.TempoValidadeProcessoExecutandoEmSegundos, turma.AnoLetivo));
                if(paramTempoLimiteExecucao != null)
                {
                    var tempo = int.Parse(paramTempoLimiteExecucao.Valor);
                    if (pro.CriadoEm.AddSeconds(tempo) < HorarioBrasilia())
                        await repositorio.RemoverAsync(pro);
                    else
                        processoExecutando = true;
                }
            }
            return processoExecutando;
        }
    }
}
