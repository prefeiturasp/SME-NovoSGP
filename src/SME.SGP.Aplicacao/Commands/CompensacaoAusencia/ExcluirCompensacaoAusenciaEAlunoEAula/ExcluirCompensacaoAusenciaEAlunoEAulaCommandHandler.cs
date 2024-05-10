using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaEAlunoEAulaCommandHandler : IRequestHandler<ExcluirCompensacaoAusenciaEAlunoEAulaCommand, bool>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMediator mediator;

        public ExcluirCompensacaoAusenciaEAlunoEAulaCommandHandler(IUnitOfWork unitOfWork, IMediator mediator)
        {
            this.unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<bool> Handle(ExcluirCompensacaoAusenciaEAlunoEAulaCommand request, CancellationToken cancellationToken)
        {
            var conpensacaoAusenciaAlunos = await mediator.Send(new ObterCompensacaoAusenciaAlunoPorIdsQuery(request.CompensacaoAusenciaAlunoAulas.Select(t => t.CompensacaoAusenciaAlunoId).Distinct().ToArray()), cancellationToken);

            unitOfWork.IniciarTransacao();
            try
            {
                foreach (var compensacaoAusenciaAluno in conpensacaoAusenciaAlunos)
                {
                    var aluasAluno = request.CompensacaoAusenciaAlunoAulas.Where(t => t.CompensacaoAusenciaAlunoId == compensacaoAusenciaAluno.Id);
                    compensacaoAusenciaAluno.QuantidadeFaltasCompensadas -= aluasAluno.Count();

                    if (compensacaoAusenciaAluno.QuantidadeFaltasCompensadas <= 0)
                        await mediator.Send(new ExcluirCompensacaoAusenciaAlunoPorIdCommand(compensacaoAusenciaAluno.Id), cancellationToken);
                    else
                        await mediator.Send(new SalvarCompensacaoAusenciaAlunoCommand(compensacaoAusenciaAluno), cancellationToken);

                    await mediator.Send(new ExcluirCompensacaoAusenciaAlunoAulaPorIdsCommand(aluasAluno.Select(s => s.Id).ToArray()), cancellationToken);
                }

                await mediator.Send(new ExcluirCompensacaoAusenciaPorIdsCommand(conpensacaoAusenciaAlunos.Select(s => s.CompensacaoAusenciaId).ToArray()), cancellationToken);

                unitOfWork.PersistirTransacao();
            }
            catch
            {
                unitOfWork.Rollback();
                throw;
            }

            return true;
        }
    }
}
