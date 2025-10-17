using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoSmeDre;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoSmeDre;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasUseCase : ConsultasBase, IConsultasNotasUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNotasUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IEnumerable<PainelEducacionalNotasVisaoSmeDreDto>> ObterNotasVisaoSmeDre(string codigoDre, int anoLetivo, int bimestre, int anoTurma)
        {
            return await mediator.Send(new ObterNotaVisaoSmeDreQuery(codigoDre, anoLetivo, bimestre, anoTurma));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>> ObterNotasVisaoUe(string codigoDre, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            return await mediator.Send(new ObterNotaVisaoUeQuery(this.Paginacao, codigoDre, anoLetivo, bimestre, modalidade));
        }
    }
}
