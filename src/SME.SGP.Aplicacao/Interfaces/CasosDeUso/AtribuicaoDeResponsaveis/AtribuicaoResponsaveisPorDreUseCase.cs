using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.Interfaces.CasosDeUso.AtribuicaoDeResponsaveis
{
    public class AtribuicaoResponsaveisPorDreUseCase : AbstractUseCase, IAtribuicaoResponsaveisPorDreUseCase
    {
        public AtribuicaoResponsaveisPorDreUseCase(IMediator mediator) : base(mediator) { }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dres = await mediator.Send(new ObterIdsDresQuery());

            if (dres.Any())
            {
                foreach (var dreId in dres)
                {
                    await ProcessarAtribuicaoDeResponsavelSupervisor(dreId);
                    await ProcessarAtribuicaoDeResponsavelPAAI(dreId);
                    await ProcessarAtribuicaoDeResponsavelASPP(dreId);
                }
                return true;
            }
            else
                return false;
        }

        private async Task ProcessarAtribuicaoDeResponsavelSupervisor(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtribuicaoDeResponsaveisSupervisorPorDre, new DreUeDto(dreId)));
        }
        private async Task ProcessarAtribuicaoDeResponsavelPAAI(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtribuicaoDeResponsaveisPAAIPorDre, new DreUeDto(dreId)));
        }
        private async Task ProcessarAtribuicaoDeResponsavelASPP(long dreId)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtribuicaoDeResponsaveisASPPorDre, new DreUeDto(dreId)));
        }


    }
}
