using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterDistorcaoIdade;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasDistorcaoIdadeUseCase : ConsultasBase, IConsultasDistorcaoIdadeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasDistorcaoIdadeUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalDistorcaoIdadeDto>> ObterDistorcaoIdade(FiltroPainelEducacionalDistorcaoIdade filtro)
        {
            return await mediator.Send(new ObterDistorcaoIdadeQuery(filtro));
        }
    }
}
