using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaPorAulaIdCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaPorAulaIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        
        public ExcluirCompensacaoAusenciaPorAulaIdCommandHandler(IUnitOfWork unitOfWork,IMediator mediator, IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo, IRepositorioPendencia repositorioPendencia)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(ExcluirCompensacaoAusenciaPorAulaIdCommand request, CancellationToken cancellationToken)
        {
            var compensacoesAusenciasAlunosAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoEAulaPorAulaIdQuery(request.AulaId), cancellationToken);
            
            if (compensacoesAusenciasAlunosAulas.Any())
            {
                unitOfWork.IniciarTransacao();
                try
                {
                    await mediator.Send(new ExcluirCompensacaoAusenciaAlunoPorIdCommand(compensacoesAusenciasAlunosAulas.FirstOrDefault().CompensacaoAusenciaAlunoId));
                    await mediator.Send(new ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand(compensacoesAusenciasAlunosAulas.Select(s=> s.CompensacaoAusenciaAlunoAulaId).ToArray()));
                    
                    unitOfWork.PersistirTransacao();
                }
                catch
                {
                    unitOfWork.Rollback();
                    throw;
                }
            }
            return true;
        }
    }
}

