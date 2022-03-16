using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosReprocessamentoConsolidacaoConselhoQueryHandler : IRequestHandler<ObterAlunosReprocessamentoConsolidacaoConselhoQuery, IEnumerable<objConsolidacaoConselhoAluno>>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterAlunosReprocessamentoConsolidacaoConselhoQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<IEnumerable<objConsolidacaoConselhoAluno>> Handle(ObterAlunosReprocessamentoConsolidacaoConselhoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterAlunosReprocessamentoConsolidacaoConselho(request.DreId);            
        }
    }
}
