using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesItineranciaDeEncaminhamentoNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;

        public ObterSecoesItineranciaDeEncaminhamentoNAAPAQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA) : base(contextoAplicacao)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>> Handle(ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesItineranciaDtoPaginado(request.EncaminhamentoNAAPAId, Paginacao);
        }     

    }
}
