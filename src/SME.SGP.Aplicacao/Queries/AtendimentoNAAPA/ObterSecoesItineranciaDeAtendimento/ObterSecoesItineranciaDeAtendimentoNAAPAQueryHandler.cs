using MediatR;
using SME.SGP.Dominio.Interfaces;
using SME.SGP.Infra;
using SME.SGP.Infra.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Queries.Evento.ObterDataPossuiEventoLiberacaoExcepcional
{
    public class ObterSecoesItineranciaDeAtendimentoNAAPAQueryHandler : ConsultasBase, IRequestHandler<ObterSecoesItineranciaDeAtendimentoNAAPAQuery, PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>>
    {
        private readonly IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA;

        public ObterSecoesItineranciaDeAtendimentoNAAPAQueryHandler(IContextoAplicacao contextoAplicacao, IRepositorioSecaoEncaminhamentoNAAPA repositorioSecaoEncaminhamentoNAPPA) : base(contextoAplicacao)
        {
            this.repositorioSecaoEncaminhamentoNAPPA = repositorioSecaoEncaminhamentoNAPPA ?? throw new System.ArgumentNullException(nameof(repositorioSecaoEncaminhamentoNAPPA));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>> Handle(ObterSecoesItineranciaDeAtendimentoNAAPAQuery request, CancellationToken cancellationToken)
        {
            return await repositorioSecaoEncaminhamentoNAPPA.ObterSecoesItineranciaDtoPaginado(request.EncaminhamentoNAAPAId, Paginacao);
        }     

    }
}
