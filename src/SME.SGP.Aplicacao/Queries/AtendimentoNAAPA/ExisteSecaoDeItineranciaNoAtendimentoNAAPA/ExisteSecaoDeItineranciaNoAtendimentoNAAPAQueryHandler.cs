using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ExisteSecaoDeItineranciaNoAtendimentoNAAPAQueryHandler : IRequestHandler<ExisteSecaoDeItineranciaNoAtendimentoNAAPAQuery, bool>
    {
        private readonly IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public ExisteSecaoDeItineranciaNoAtendimentoNAAPAQueryHandler(IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao) 
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new System.ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<bool> Handle(ExisteSecaoDeItineranciaNoAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPASecao.ExisteSecoesDeItineracia(request.Id);
        }
    }
}
