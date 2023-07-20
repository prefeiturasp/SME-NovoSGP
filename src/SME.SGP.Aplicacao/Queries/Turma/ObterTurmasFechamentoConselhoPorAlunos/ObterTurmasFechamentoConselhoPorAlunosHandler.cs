using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasFechamentoConselhoPorAlunosQueryHandler : IRequestHandler<ObterTurmasFechamentoConselhoPorAlunosQuery, IEnumerable<TurmaAlunoDto>>
    {
        private readonly IRepositorioTurmaConsulta turmaRepository;

        public ObterTurmasFechamentoConselhoPorAlunosQueryHandler(IRepositorioTurmaConsulta turmaRepository)
        {
            this.turmaRepository = turmaRepository ?? throw new System.ArgumentNullException(nameof(turmaRepository));
        }

        public Task<IEnumerable<TurmaAlunoDto>> Handle(ObterTurmasFechamentoConselhoPorAlunosQuery request, CancellationToken cancellationToken)
        {
            return turmaRepository.ObterPorAlunos(request.AlunosCodigos, request.AnoLetivo);
        }

    }
}
