using MediatR;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoIdQueryHandler : IRequestHandler<ObterConselhoClasseAlunoIdQuery, long>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;
        public ObterConselhoClasseAlunoIdQueryHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }
        public async Task<long> Handle(ObterConselhoClasseAlunoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAluno.ObterConselhoClasseAlunoIdAsync(request.ConselhoClasseId, request.AlunoCodigo);
        }
    }
}
