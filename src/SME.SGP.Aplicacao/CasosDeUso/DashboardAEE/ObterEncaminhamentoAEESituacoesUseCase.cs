using MediatR;
using SME.SGP.Infra.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterEncaminhamentoAEESituacoesUseCase : AbstractUseCase, IObterEncaminhamentoAEESituacoesUseCase
    {
        public ObterEncaminhamentoAEESituacoesUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<IEnumerable<AEESituacaoEncaminhamentoDto>> Executar(FiltroDashboardAEEDto param)
        {
            if (param.AnoLetivo == 0)
                param.AnoLetivo = DateTime.Now.Year;
            return await mediator.Send(new ObterEncaminhamentoAEESituacoesQuery(param.AnoLetivo, param.DreId, param.UeId));
        }
    }
}
