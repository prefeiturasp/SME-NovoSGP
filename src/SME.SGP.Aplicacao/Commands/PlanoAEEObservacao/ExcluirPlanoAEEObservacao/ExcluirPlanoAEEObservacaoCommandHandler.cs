using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAEEObservacaoCommandHandler : IRequestHandler<ExcluirPlanoAEEObservacaoCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao;
        private readonly IMediator mediator;

        public ExcluirPlanoAEEObservacaoCommandHandler(IUnitOfWork unitOfWork, IRepositorioPlanoAEEObservacao repositorioPlanoAEEObservacao, IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.repositorioPlanoAEEObservacao = repositorioPlanoAEEObservacao ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObservacao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPlanoAEEObservacaoCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await repositorioPlanoAEEObservacao.RemoverLogico(request.Id);
                    await mediator.Send(new ExcluirNotificacaoPlanoAEEObservacaoCommand(request.Id));

                    unitOfWork.PersistirTransacao();
                }
                catch (Exception e)
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }

            return true;
        }
    }
}
