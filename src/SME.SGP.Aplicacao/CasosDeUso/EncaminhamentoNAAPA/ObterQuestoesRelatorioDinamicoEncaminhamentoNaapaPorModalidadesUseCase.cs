using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoEncaminhamentoNaapaPorModalidadesUseCase : IObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesUseCase
    {
        private readonly IMediator mediator;

        public ObterQuestoesRelatorioDinamicoEncaminhamentoNaapaPorModalidadesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<QuestaoDto>> Executar(int[] modalidadesIds)
        {
            return
                await mediator
                .Send(new ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery(modalidadesIds));
        }
    }
}
