using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesPorEtapaDeEncaminhamentoQueryHandler : IRequestHandler<ObterSecoesPorEtapaDeEncaminhamentoQuery, IEnumerable<SecaoQuestionarioDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoAEE repositorioSecaoEncaminhamentoAEE;

        public ObterSecoesPorEtapaDeEncaminhamentoQueryHandler(IRepositorioSecaoEncaminhamentoAEE repositorioSecaoEncaminhamentoAEE)
        {
            this.repositorioSecaoEncaminhamentoAEE = repositorioSecaoEncaminhamentoAEE ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoAEE));
        }

        public async Task<IEnumerable<SecaoQuestionarioDto>> Handle(ObterSecoesPorEtapaDeEncaminhamentoQuery request, CancellationToken cancellationToken)
        {
            // Se tiver id 



            return await repositorioSecaoEncaminhamentoAEE.ObterSecaoEncaminhamentoPorEtapa(request.Etapa); 
        }
    }
}
