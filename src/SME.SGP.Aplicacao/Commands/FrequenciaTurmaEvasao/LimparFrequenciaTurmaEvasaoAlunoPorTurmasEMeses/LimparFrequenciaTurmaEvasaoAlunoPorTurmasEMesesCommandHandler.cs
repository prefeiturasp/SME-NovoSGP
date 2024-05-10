using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommandHandler : IRequestHandler<LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand, bool>
    {
        private readonly IRepositorioFrequenciaTurmaEvasaoAluno repositorioFrequenciaTurmaEvasaoAluno;

        public LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommandHandler(IRepositorioFrequenciaTurmaEvasaoAluno repositorioFrequenciaTurmaEvasaoAluno)
        {
            this.repositorioFrequenciaTurmaEvasaoAluno = repositorioFrequenciaTurmaEvasaoAluno ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaTurmaEvasaoAluno));
        }

        public async Task<bool> Handle(LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMesesCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequenciaTurmaEvasaoAluno.LimparFrequenciaTurmaEvasaoAlunoPorTurmasEMeses(request.TurmasIds, request.Meses);
            return true;
        }
    }
}
