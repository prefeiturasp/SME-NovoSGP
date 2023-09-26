using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarEncaminhamentoAEETurmaAlunoCommandHandler : IRequestHandler<SalvarEncaminhamentoAEETurmaAlunoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioEncaminhamentoAEETurmaAluno repositorioEncaminhamentoAEETurmaAluno;

        public SalvarEncaminhamentoAEETurmaAlunoCommandHandler(IMediator mediator, IRepositorioEncaminhamentoAEETurmaAluno repositorioEncaminhamentoAEETurmaAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioEncaminhamentoAEETurmaAluno = repositorioEncaminhamentoAEETurmaAluno ?? throw new ArgumentNullException(nameof(repositorioEncaminhamentoAEETurmaAluno));
        }

        public async Task<bool> Handle(SalvarEncaminhamentoAEETurmaAlunoCommand request, CancellationToken cancellationToken)
        {
            var encaminhamentoAEETurmasAluno = await mediator.Send(new ObterEncaminhamentoAEETurmaAlunoPorIdQuery(request.EncaminhamentoAEEId), cancellationToken);

            var turmasRegularESrmdoAluno = await mediator.Send(new ObterTurmaRegularESrmPorAlunoQuery(long.Parse(request.AlunoCodigo)), cancellationToken);
            var turmaCodigos = turmasRegularESrmdoAluno.Where(s => !s.Inativo).Select(s => s.CodigoTurma.ToString()).ToArray();
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmaCodigos), cancellationToken);

            foreach (var turma in turmas)
                if (!encaminhamentoAEETurmasAluno.Any(t => t.TurmaId == turma.Id))
                    repositorioEncaminhamentoAEETurmaAluno.Salvar(new Dominio.EncaminhamentoAEETurmaAluno()
                    {
                        EncaminhamentoAEEId = request.EncaminhamentoAEEId,
                        AlunoCodigo = request.AlunoCodigo,
                        TurmaId = turma.Id
                    });

            var encaminhamentoAEETurmasAlunoRemover = encaminhamentoAEETurmasAluno.Where(t => !turmas.Any(x => x.Id == t.TurmaId));
            if (encaminhamentoAEETurmasAlunoRemover.Any())
                foreach (var remover in encaminhamentoAEETurmasAlunoRemover)
                    repositorioEncaminhamentoAEETurmaAluno.Remover(remover);

            return true;
        }
    }
}
