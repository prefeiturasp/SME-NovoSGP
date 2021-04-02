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
            if (param.Ano == 0)
                param.Ano = DateTime.Now.Year;
            return await mediator.Send(new ObterEncaminhamentoAEESituacoesQuery(param.Ano, param.DreId, param.UeId));
        }
    }
}
