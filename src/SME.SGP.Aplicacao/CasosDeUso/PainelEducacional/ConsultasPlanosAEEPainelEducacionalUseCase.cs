using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterPlanoAEE;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoPlanoAEE;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasPlanosAEEPainelEducacionalUseCase : ConsultasBase, IConsultasPlanosAEEPainelEducacionalUseCase
    {
        private readonly IMediator mediator;

        public ConsultasPlanosAEEPainelEducacionalUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalPlanoAEEDto>> ObterPlanosAEE(FiltroPainelEducacionalPlanosAEE filtro)
        {
            return await mediator.Send(new ObterConsolidacaoPlanosAEEQuery(filtro));
        }

    }
}
