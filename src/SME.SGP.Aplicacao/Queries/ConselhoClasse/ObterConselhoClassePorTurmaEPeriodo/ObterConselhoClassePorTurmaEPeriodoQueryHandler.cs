using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterConselhoClassePorTurmaEPeriodoQueryHandler : IRequestHandler<ObterConselhoClassePorTurmaEPeriodoQuery, ConselhoClasse>
    {
        private readonly IRepositorioConselhoClasseConsulta repositorioConselhoClasse;
        public ObterConselhoClassePorTurmaEPeriodoQueryHandler(IRepositorioConselhoClasseConsulta repositorioConselhoClasse)
        {
            this.repositorioConselhoClasse = repositorioConselhoClasse;
        }
        public Task<ConselhoClasse> Handle(ObterConselhoClassePorTurmaEPeriodoQuery request, CancellationToken cancellationToken)
        {
            return repositorioConselhoClasse.ObterPorTurmaEPeriodoAsync(request.TurmaId, request.PeriodoEscolarId);          
        }
    }
}
