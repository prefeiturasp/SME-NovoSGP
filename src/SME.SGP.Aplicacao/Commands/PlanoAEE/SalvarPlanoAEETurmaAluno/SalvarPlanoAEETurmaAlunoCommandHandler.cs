using MediatR;
using SME.SGP.Dados;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEETurmaAlunoCommandHandler : IRequestHandler<SalvarPlanoAEETurmaAlunoCommand, bool>
    {
        private readonly IMediator mediator;
        private readonly IRepositorioPlanoAEETurmaAluno repositorioPlanoAEETurmaAluno;

        public SalvarPlanoAEETurmaAlunoCommandHandler(IMediator mediator, IRepositorioPlanoAEETurmaAluno repositorioPlanoAEETurmaAluno)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            this.repositorioPlanoAEETurmaAluno = repositorioPlanoAEETurmaAluno ?? throw new ArgumentNullException(nameof(repositorioPlanoAEETurmaAluno));
        }

        public async Task<bool> Handle(SalvarPlanoAEETurmaAlunoCommand request, CancellationToken cancellationToken)
        {
            var planoAEETurmasAluno = await mediator.Send(new ObterPlanoAEETurmaAlunoPorIdQuery(request.PlanoAEEId), cancellationToken);

            var turmasRegularESrmdoAluno = await mediator.Send(new ObterTurmaRegularESrmPorAlunoQuery(long.Parse(request.AlunoCodigo)), cancellationToken);

            var turmaCodigos = turmasRegularESrmdoAluno.Where(s => !s.Inativo).Select(s => s.CodigoTurma.ToString()).ToArray();
            var turmas = await mediator.Send(new ObterTurmasPorCodigosQuery(turmaCodigos), cancellationToken);

            foreach (var turma in turmas)
                if (!planoAEETurmasAluno.Any(t => t.TurmaId == turma.Id))
                    repositorioPlanoAEETurmaAluno.Salvar(new Dominio.PlanoAEETurmaAluno()
                    {
                        PlanoAEEId = request.PlanoAEEId,
                        AlunoCodigo = request.AlunoCodigo,
                        TurmaId = turma.Id
                    });

            var planoAEETurmasAlunoRemover = planoAEETurmasAluno.Where(t => !turmas.Any(x => x.Id == t.TurmaId));
            if (planoAEETurmasAlunoRemover.Any())
                foreach (var remover in planoAEETurmasAlunoRemover)
                    repositorioPlanoAEETurmaAluno.Remover(remover);

            return true;
        }
    }
}
