using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;
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

            if (filtro.NaoEhNulo())
            {
                var turmasUe = await mediator.Send(new ObterTurmasInfantilPorUEQuery(DateTimeExtension.HorarioBrasilia().Year, filtro.CodigoUe));
                
                foreach (var turma in turmasUe)
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpAula.RotaExecutaPendenciasAulaDiarioBordoTurma, turma.TurmaCodigo));
            }                

            return true;
        }
    }
}
