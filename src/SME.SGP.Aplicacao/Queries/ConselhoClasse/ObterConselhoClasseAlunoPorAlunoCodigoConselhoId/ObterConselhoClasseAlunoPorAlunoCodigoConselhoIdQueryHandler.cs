using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQueryHandler : IRequestHandler<ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery, ConselhoClasseAluno>
    {
        private readonly IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno;

        public ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQueryHandler(IRepositorioConselhoClasseAluno repositorioConselhoClasseAluno)
        {
            this.repositorioConselhoClasseAluno = repositorioConselhoClasseAluno ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAluno));
        }

        public async Task<ConselhoClasseAluno> Handle(ObterConselhoClasseAlunoPorAlunoCodigoConselhoIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasseAluno.ObterPorConselhoClasseAlunoCodigoAsync(request.ConselhoClasseId, request.AlunoCodigo);
        }
    }
}
