using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecaoAtendimentoNAAPACommandHandler : IRequestHandler<ExcluirSecaoAtendimentoNAAPACommand, bool>
    {
        public IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao { get; }
        public IMediator mediator { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirSecaoAtendimentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao, IUnitOfWork unitOfWork)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirSecaoAtendimentoNAAPACommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await mediator.Send(new ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommand(request.EncaminhamentoNAAPASecaoId));
                    await repositorioEncaminhamentoNAAPASecao.RemoverLogico(request.EncaminhamentoNAAPASecaoId);
                   
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
}
