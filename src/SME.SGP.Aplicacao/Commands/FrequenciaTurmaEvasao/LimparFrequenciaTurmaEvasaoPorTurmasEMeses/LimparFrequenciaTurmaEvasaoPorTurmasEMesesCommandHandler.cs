using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommandHandler : IRequestHandler<LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand, bool>
    {
        private readonly IRepositorioFrequenciaTurmaEvasao repositorioFrequenciaTurmaEvasao;

        public LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommandHandler(IRepositorioFrequenciaTurmaEvasao repositorioFrequenciaTurmaEvasao)
        {
            this.repositorioFrequenciaTurmaEvasao = repositorioFrequenciaTurmaEvasao ?? throw new System.ArgumentNullException(nameof(repositorioFrequenciaTurmaEvasao));
        }

        public async Task<bool> Handle(LimparFrequenciaTurmaEvasaoPorTurmasEMesesCommand request, CancellationToken cancellationToken)
        {
            await repositorioFrequenciaTurmaEvasao.LimparFrequenciaTurmaEvasaoPorTurmasEMeses(request.TurmasIds, request.Meses);
            return true;
        }
    }
}
