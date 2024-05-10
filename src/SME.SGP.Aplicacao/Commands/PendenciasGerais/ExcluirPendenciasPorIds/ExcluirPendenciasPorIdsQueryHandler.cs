using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciasPorIdsCommandHandler : IRequestHandler<ExcluirPendenciasPorIdsCommand, bool>
    {
        private readonly IRepositorioPendencia repositorioPendencia;
        private readonly IMediator mediator;

        public ExcluirPendenciasPorIdsCommandHandler(IRepositorioPendencia repositorioPendencia, IMediator mediator)
        {
            this.repositorioPendencia = repositorioPendencia ?? throw new ArgumentNullException(nameof(repositorioPendencia));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPendenciasPorIdsCommand request, CancellationToken cancellationToken)
        {
            try
            {
                await repositorioPendencia.ExclusaoLogicaPendenciaIds(request.PendenciasIds);
                return true;
            }
            catch(Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Erro ao excluir as pendências informadas", LogNivel.Critico, LogContexto.Pendencia, ex.Message));
                return false;
            }
        }
    }
}
