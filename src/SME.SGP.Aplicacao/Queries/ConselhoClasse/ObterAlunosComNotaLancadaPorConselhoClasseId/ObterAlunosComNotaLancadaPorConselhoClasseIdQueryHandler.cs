using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosComNotaLancadaPorConselhoClasseIdQueryHandler : IRequestHandler<ObterAlunosComNotaLancadaPorConselhoClasseIdQuery, IEnumerable<string>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterAlunosComNotaLancadaPorConselhoClasseIdQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<IEnumerable<string>> Handle(ObterAlunosComNotaLancadaPorConselhoClasseIdQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterAlunosComNotaLancadaPorConselhoClasseId(request.ConselhoClasseId);            
        }
    }
}
