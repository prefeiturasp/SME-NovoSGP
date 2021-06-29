using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalAlunosNoSemestreQueryHandler : IRequestHandler<ObterTotalAlunosNoSemestreQuery, int>
    {
        private readonly IRepositorioAcompanhamentoAluno repositorio;

        public ObterTotalAlunosNoSemestreQueryHandler(IRepositorioAcompanhamentoAluno repositorio)
        {
            this.repositorio = repositorio ?? throw new ArgumentNullException(nameof(repositorio));
        }

        public async Task<int> Handle(ObterTotalAlunosNoSemestreQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterTotalAlunosTurmaAnoLetivoESemestre(request.TurmaId, request.AnoLetivo, request.Semestre);
        }
    }
}
