using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesItineranciaDeEncaminhamentoNAAPAQueryHandler : IRequestHandler<ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery, IEnumerable<EncaminhamentoNAAPASecaoItineranciaDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;

        public ObterSecoesItineranciaDeEncaminhamentoNAAPAQueryHandler(IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<IEnumerable<EncaminhamentoNAAPASecaoItineranciaDto>> Handle(ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            var secoes = await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesItineranciaEncaminhamentoDto(request.EncaminhamentoNAAPAId);
            return secoes;
        }

    }
}
