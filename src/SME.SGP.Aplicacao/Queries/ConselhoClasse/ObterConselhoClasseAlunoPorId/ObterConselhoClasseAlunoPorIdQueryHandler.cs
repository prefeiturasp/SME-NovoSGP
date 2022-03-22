using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClasseAlunoPorIdQueryHandler : IRequestHandler<ObterConselhoClasseAlunoPorIdQuery, ConselhoClasseAluno>
    {
        private readonly IRepositorioConselhoClasseAlunoConsulta repositorio;

        public ObterConselhoClasseAlunoPorIdQueryHandler(IRepositorioConselhoClasseAlunoConsulta repositorioConselhoClasseAlunoConsulta)
        {
            this.repositorio = repositorioConselhoClasseAlunoConsulta ?? throw new ArgumentNullException(nameof(repositorioConselhoClasseAlunoConsulta));
        }
        public async Task<ConselhoClasseAluno> Handle(ObterConselhoClasseAlunoPorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterConselhoClasseAlunoPorId(request.ConselhoClasseAlunoId);
        }
    }
}