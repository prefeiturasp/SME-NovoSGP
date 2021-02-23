using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaEncaminhamentoAEECommandHandler : IRequestHandler<ExcluirPendenciaEncaminhamentoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE;
        public readonly IUnitOfWork unitOfWork;

        public ExcluirPendenciaEncaminhamentoAEECommandHandler(IMediator mediator, IRepositorioPendenciaEncaminhamentoAEE repositorioPendenciaEncaminhamentoAEE, IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            this.repositorioPendenciaEncaminhamentoAEE = repositorioPendenciaEncaminhamentoAEE ?? throw new System.ArgumentNullException(nameof(repositorioPendenciaEncaminhamentoAEE));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirPendenciaEncaminhamentoAEECommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioPendenciaEncaminhamentoAEE.Excluir(request.PendenciaId);
                    await mediator.Send(new ExcluirPendenciasUsuariosPorPendenciaIdCommand(request.PendenciaId));
                    await mediator.Send(new ExcluirPendenciaPorIdCommand(request.PendenciaId));
                    unitOfWork.PersistirTransacao();

                    return true;
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return false;
        }
    }
}
