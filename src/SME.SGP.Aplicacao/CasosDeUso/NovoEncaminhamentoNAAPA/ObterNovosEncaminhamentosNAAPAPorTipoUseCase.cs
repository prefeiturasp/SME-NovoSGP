using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso.NovoEncaminhamentoNAAPA;
using SME.SGP.Aplicacao.Queries.NovoEncaminhamentoNAAPA;
using SME.SGP.Infra;
using SME.SGP.Infra.Dtos.NovoEncaminhamentoNAAPA;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso.NovoEncaminhamentoNAAPA
{
    public class ObterNovosEncaminhamentosNAAPAPorTipoUseCase : IObterNovosEncaminhamentosNAAPAPorTipoUseCase
    {
        private readonly IMediator mediator;

        public ObterNovosEncaminhamentosNAAPAPorTipoUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<PaginacaoResultadoDto<NovoEncaminhamentoNAAPAResumoDto>> Executar(FiltroNovoEncaminhamentoNAAPADto filtro)
        {
            if (!string.IsNullOrEmpty(filtro.CodigoUe) && filtro.CodigoUe.Equals("-99"))
                filtro.CodigoUe = string.Empty;

            return await mediator.Send(new ObterNovosEncaminhamentosNAAPAPorTipoQuery(filtro));
        }
    }
}