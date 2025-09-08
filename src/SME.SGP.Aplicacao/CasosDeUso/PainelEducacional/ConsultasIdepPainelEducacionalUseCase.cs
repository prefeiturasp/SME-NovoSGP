using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterIdepPorAnoEtapa;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasIdepPainelEducacionalUseCase : IConsultasIdepPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasIdepPainelEducacionalUseCase(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<IEnumerable<PainelEducacionalConsolidacaoIdep>> ObterIdepPorAnoEtapa(int ano, int etapa)
        {
            return await mediator.Send(new ObterIdepPorAnoEtapaQuery(ano, etapa));
        }
    }
}
