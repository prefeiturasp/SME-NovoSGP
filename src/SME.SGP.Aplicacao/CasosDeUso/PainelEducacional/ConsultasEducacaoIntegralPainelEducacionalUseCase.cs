using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterEducacaoIntegral;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoEducacaoIntegral;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasEducacaoIntegralPainelEducacionalUseCase : ConsultasBase, IConsultasEducacaoIntegralPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasEducacaoIntegralPainelEducacionalUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalEducacaoIntegralDto>> ObterEducacaoIntegral(FiltroPainelEducacionalEducacaoIntegral filtro)
        {
            return await mediator.Send(new ObterEducacaoIntegralQuery(filtro));
        }
    }
}
