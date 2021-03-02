using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPlanoAEECommandHandler : IRequestHandler<ExcluirPendenciaPlanoAEECommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE;

        public ExcluirPendenciaPlanoAEECommandHandler(IMediator mediator, IUnitOfWork unitOfWork, IRepositorioPendenciaPlanoAEE repositorioPendenciaPlanoAEE)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPendenciaPlanoAEE = repositorioPendenciaPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPendenciaPlanoAEE));
        }

        public async Task<bool> Handle(ExcluirPendenciaPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var pendenciasPlano = await repositorioPendenciaPlanoAEE.ObterPorPlanoId(request.PlanoAEEId);

            foreach(var pendenciaPlano in pendenciasPlano)
                await ExcluirPendencia(pendenciaPlano);

            return true;
        }

        private async Task ExcluirPendencia(PendenciaPlanoAEE pendenciaPlano)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await mediator.Send(new ExcluirPendenciasUsuariosPorPendenciaIdCommand(pendenciaPlano.PendenciaId));
                    await mediator.Send(new ExcluirPendenciaPorIdCommand(pendenciaPlano.PendenciaId));

                    repositorioPendenciaPlanoAEE.Remover(pendenciaPlano);

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
        }
    }
}
