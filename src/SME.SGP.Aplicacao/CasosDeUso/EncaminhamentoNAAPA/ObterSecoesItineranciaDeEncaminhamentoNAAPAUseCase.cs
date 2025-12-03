using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Aplicacao.Queries;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase : IObterSecoesItineranciaDeAtendimentoNAAPAUseCase
    {
        private readonly IMediator mediator;

        public ObterSecoesItineranciaDeEncaminhamentoNAAPAUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<AtendimentoNAAPASecaoItineranciaDto>> Executar(long encaminhamentoNAAPAId)
        {
            return await mediator.Send(new ObterSecoesItineranciaDeEncaminhamentoNAAPAQuery(encaminhamentoNAAPAId));
        }
    }
}
