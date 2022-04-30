using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class PendenciaAulaDiarioBordoUseCase : AbstractUseCase, IPendenciaAulaDiarioBordoUseCase
    {
        public PendenciaAulaDiarioBordoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<DreUeDto>();
            var uesDre = await mediator.Send(new ObterUesCodigosPorDreQuery(filtro.DreId //aki));

            var codigoTurmas = new List<string>();
            foreach (var ue in uesDre)
            {
                var turmasUe = await mediator.Send(new ObterTurmasInfantilPorUEQuery(DateTimeExtension.HorarioBrasilia().Year, ue));
                if (turmasUe.Any())
                    codigoTurmas.AddRange(turmasUe.Select(t => t.TurmaCodigo));
            }

            foreach (var turma in codigoTurmas)         
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaExecutaPendenciasAulaDiarioBordoTurma, turma));    
            
            return true;
        }
    }
}
