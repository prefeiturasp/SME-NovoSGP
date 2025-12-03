using MediatR;
using SME.SGP.Dominio.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries
{
    public class ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQueryHandler : IRequestHandler<ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQuery, bool>
    {
        private readonly IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao;

        public ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQueryHandler(IRepositorioAtendimentoNAAPASecao repositorioEncaminhamentoNAAPASecao) 
        {
            this.repositorioEncaminhamentoNAAPASecao = repositorioEncaminhamentoNAAPASecao ?? throw new System.ArgumentNullException(nameof(repositorioEncaminhamentoNAAPASecao));
        }

        public async Task<bool> Handle(ExisteSecaoDeItineranciaNoEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioEncaminhamentoNAAPASecao.ExisteSecoesDeItineracia(request.Id);
        }
    }
}
