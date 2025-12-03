using MediatR;
using SME.SGP.Aplicacao.Interfaces.CasosDeUso;
using SME.SGP.Infra.Dtos.Questionario;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ObterQuestoesRelatorioDinamicoAtendimentoNaapaPorModalidadesUseCase : IObterQuestoesRelatorioDinamicoAtendimentoNAAPAPorModalidadesUseCase
    {
        private readonly IMediator mediator;
        public ObterQuestoesRelatorioDinamicoAtendimentoNaapaPorModalidadesUseCase(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        public async Task<IEnumerable<SecaoQuestoesDTO>> Executar(int[] modalidadesId)
        {
            return
                await mediator
                .Send(new ObterQuestoesRelatorioDinamicoEncaminhamentoNAAPAPorModalidadesQuery(modalidadesId));
        }
    }
}
