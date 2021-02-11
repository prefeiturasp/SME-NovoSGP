using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentosAEEUseCase : AbstractUseCase, IObterEncaminhamentosAEEUseCase
    {
        public ObterEncaminhamentosAEEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<PaginacaoResultadoDto<EncaminhamentoAEEResumoDto>> Executar(FiltroPesquisaEncaminhamentosAEEDto filtros)
        {
            return await mediator.Send(new ObterEncaminhamentosAEEQuery(filtros.DreId, filtros.UeId, filtros.TurmaId, filtros.AlunoCodigo, filtros.Situacao, filtros.ResponsavelRf, filtros.AnoLetivo));
        }
    }
}
