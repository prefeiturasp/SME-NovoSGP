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
    public class ExcluirSecaoEncaminhamentoNAAPACommandHandler : IRequestHandler<ExcluirSecaoEncaminhamentoNAAPACommand, bool>
    {
        public IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao { get; }
        public IMediator mediator { get; }
        public IUnitOfWork unitOfWork { get; }

        public ExcluirSecaoEncaminhamentoNAAPACommandHandler(IMediator mediator, IRepositorioEncaminhamentoNAAPASecao repositorioEncaminhamentoNAAPASecao, IUnitOfWork unitOfWork)
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<bool> Handle(ExcluirSecaoEncaminhamentoNAAPACommand request, CancellationToken cancellationToken)
        {
            using (var transacao = unitOfWork.IniciarTransacao())
            {
                try
                {
                    await mediator.Send(new ExcluirQuestaoEncaminhamentoNAAPAPorSecaoIdCommand(request.EncaminhamentoNAAPASecaoId));
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
