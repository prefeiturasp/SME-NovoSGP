using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterPorTurmaEPeriodoQueryHandler : IRequestHandler<ObterPorTurmaEPeriodoQuery, ConselhoClasse>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterPorTurmaEPeriodoQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public async Task<ConselhoClasse> Handle(ObterPorTurmaEPeriodoQuery request, CancellationToken cancellationToken)
        {
            return await repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(request.TurmaId, request.PeriodoEscolarId);          
        }
    }
}
