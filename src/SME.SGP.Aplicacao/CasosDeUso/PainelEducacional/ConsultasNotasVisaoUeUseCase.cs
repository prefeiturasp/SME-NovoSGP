using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.PainelEducacional;
using SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotas.VisaoUe;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.PainelEducacional.Notas.VisaoUe;
using SME.SGP.Infra.Interfaces;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.PainelEducacional
{
    public class ConsultasNotasVisaoUeUseCase : ConsultasBase, IConsultasNotasVisaoUeUseCase
    {
        private readonly IMediator mediator;

        public ConsultasNotasVisaoUeUseCase(IContextoAplicacao contextoAplicacao, IMediator mediator) : base(contextoAplicacao)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<PainelEducacionalNotasVisaoUeDto>> ObterNotasVisaoUe(string codigoUe, int anoLetivo, int bimestre, Modalidade modalidade)
        {
            return await mediator.Send(new ObterNotaVisaoUeQuery(this.Paginacao, codigoUe, anoLetivo, bimestre, modalidade));
        }
    }
}
