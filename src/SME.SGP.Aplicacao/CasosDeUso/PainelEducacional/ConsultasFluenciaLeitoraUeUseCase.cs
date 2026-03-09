using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterFluenciaLeitoraUe;
using SME.SGP.Infra.Dtos.PainelEducacional.ConsolidacaoDistorcaoIdade;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasFluenciaLeitoraUeUseCase : ConsultasBase, IConsultasFluenciaLeitoraUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasFluenciaLeitoraUeUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalFluenciaLeitoraUeDto>> ObterFluenciaLeitoraUe(FiltroPainelEducacionalFluenciaLeitoraUe filtro)
        {
            return await mediator.Send(new ObterFluenciaLeitoraUeQuery(filtro));
        }
    }
}
