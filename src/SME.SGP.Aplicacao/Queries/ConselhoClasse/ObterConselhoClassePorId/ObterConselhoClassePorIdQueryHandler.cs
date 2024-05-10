using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClassePorIdQueryHandler : IRequestHandler<ObterConselhoClassePorIdQuery, ConselhoClasse>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorio;

        public ObterConselhoClassePorIdQueryHandler(IRepositorioConselhoClasseConsulta repositorioTurmaConsulta)
        {
            this.repositorio = repositorioTurmaConsulta ?? throw new ArgumentNullException(nameof(repositorioTurmaConsulta));
        }
        public async Task<ConselhoClasse> Handle(ObterConselhoClassePorIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorio.ObterConselhoClassePorId(request.ConselhoClasseId);
        }
    }
}