using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExcluirCompensacaoAusenciaPorAulaIdUseCase : AbstractUseCase, IExcluirCompensacaoAusenciaPorAulaIdUseCase
    {
        public ExcluirCompensacaoAusenciaPorAulaIdUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroIdDto>();
            
            await mediator.Send(new ExcluirCompensacaoAusenciaPorAulaIdCommand(filtro.Id));
            
            return true;
        }
    }
}
