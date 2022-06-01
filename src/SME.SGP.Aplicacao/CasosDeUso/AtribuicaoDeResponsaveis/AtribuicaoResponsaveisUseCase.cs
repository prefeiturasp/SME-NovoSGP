using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class AtribuicaoResponsaveisUseCase : AbstractUseCase, IAtribuicaoResponsaveisUseCase
    {
        public AtribuicaoResponsaveisUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dres = await mediator.Send(new ObterIdsDresQuery());

            foreach (var dreId in dres)
            {
                await ProcessarAtribuicaoDeResponsavelSupervisor(dreId);
                await ProcessarAtribuicaoDeResponsavelPAAI(dreId);
                await ProcessarAtribuicaoDeResponsavelASPP(dreId);
            }
            return true;

        }

        private async Task ProcessarAtribuicaoDeResponsavelSupervisor(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverAtribuicaoResponsaveisSupervisorPorDre, dreId));
        }
        private async Task ProcessarAtribuicaoDeResponsavelPAAI(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverAtribuicaoResponsaveisPAAIPorDre, dreId));
        }
        private async Task ProcessarAtribuicaoDeResponsavelASPP(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RemoverAtribuicaoResponsaveisASPPorDre, dreId));
        }


    }
}
