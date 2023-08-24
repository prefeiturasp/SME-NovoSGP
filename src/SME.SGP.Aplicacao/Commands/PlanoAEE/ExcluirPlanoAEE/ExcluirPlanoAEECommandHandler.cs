using MediatR;
using SME.SGP.Dados;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAEECommandHandler : IRequestHandler<ExcluirPlanoAEECommand, bool>
    {
        private readonly IRepositorioPlanoAEE repositorioPlanoAEE;
        private readonly IRepositorioPlanoAEEObservacao repositorioPlanoAEEObs;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ExcluirPlanoAEECommandHandler(IRepositorioPlanoAEE repositorioPlanoAEE, IRepositorioPlanoAEEObservacao repositorioPlanoAEEObs, 
                                            IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.repositorioPlanoAEE = repositorioPlanoAEE ?? throw new ArgumentNullException(nameof(repositorioPlanoAEE));
            this.repositorioPlanoAEEObs = repositorioPlanoAEEObs ?? throw new ArgumentNullException(nameof(repositorioPlanoAEEObs));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirPlanoAEECommand request, CancellationToken cancellationToken)
        {
            var usuarioLogado = await mediator.Send(ObterUsuarioLogadoQuery.Instance); 
            var planoAEEObservacoes = (await repositorioPlanoAEEObs.ObterObservacoesPlanoPorId(request.PlanoAEEId, usuarioLogado.CodigoRf));

            unitOfWork.IniciarTransacao();            
            try
            {
                await repositorioPlanoAEE.RemoverLogico(request.PlanoAEEId);
                foreach (var obs in planoAEEObservacoes)
                    await mediator.Send(new ExcluirPlanoAEEObservacaoCommand(obs.Id));

                await mediator.Send(new ExcluirPendenciaPlanoAEECommand(request.PlanoAEEId));

                unitOfWork.PersistirTransacao();

                return true;
            }
            catch (Exception e)
            {
                unitOfWork.Rollback();
                throw;
            }
        }
    }
}
