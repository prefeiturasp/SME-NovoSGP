using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirInformesCommandHandler : IRequestHandler<ExcluirInformesCommand, bool>
    {
        public IMediator mediator { get; }
        public IRepositorioInformativo repositorio { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirInformesCommandHandler(
                                IMediator mediator, 
                                IRepositorioInformativo repositorio, 
                                IUnitOfWork unitOfWork)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirInformesCommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await mediator.Send(new ExcluirInformesPerfilsPorIdInformesCommad(request.Id));
                    await mediator.Send(new ExcluirInformativosNotificacaoPorIdInformativoCommad(request.Id));
                    await repositorio.RemoverAsync(request.Id);
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
