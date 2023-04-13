using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;
        
        public ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommandHandler(IUnitOfWork unitOfWork,IMediator mediator, IRepositorioPendenciaDiarioBordo repositorioPendenciaDiarioBordo, IRepositorioPendencia repositorioPendencia)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<bool> Handle(ExcluirCompensacaoAusenciaAlunoEAulaPorAulaIdCommand request, CancellationToken cancellationToken)
        {
            var compensacaoAusenciaAlunoAulas = await mediator.Send(new ObterCompensacaoAusenciaAlunoAulaPorAulaIdQuery(request.AulaId, request.NumeroAula), cancellationToken);
            if (compensacaoAusenciaAlunoAulas.Any())
            {
                var conpensacaoAusenciaAlunos = await mediator.Send(new ObterCompensacaoAusenciaAlunoPorIdsQuery(compensacaoAusenciaAlunoAulas.Select(t => t.CompensacaoAusenciaAlunoId).Distinct().ToArray()));

                unitOfWork.IniciarTransacao();
                try
                {
                    foreach(var compensacaoAusenciaAluno in conpensacaoAusenciaAlunos)
                    {
                        var aluasAluno = compensacaoAusenciaAlunoAulas.Where(t => t.CompensacaoAusenciaAlunoId == compensacaoAusenciaAluno.Id);
                        compensacaoAusenciaAluno.QuantidadeFaltasCompensadas -= aluasAluno.Count();

                        if (compensacaoAusenciaAluno.QuantidadeFaltasCompensadas <= 0)
                            await mediator.Send(new ExcluirCompensacaoAusenciaAlunoPorIdCommand(compensacaoAusenciaAluno.Id));
                        else
                            await mediator.Send(new SalvarCompensacaoAusenciaAlunoCommand(compensacaoAusenciaAluno));

                        await mediator.Send(new ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand(aluasAluno.Select(s => s.Id).ToArray()));
                    }

                    await mediator.Send(new ExcluirCompensacaoAusenciaPorIdsCommand(conpensacaoAusenciaAlunos.Select(s => s.CompensacaoAusenciaId).ToArray()));

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

