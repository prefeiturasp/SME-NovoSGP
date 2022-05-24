using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase : AbstractUseCase, IReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase
    {
        public ReprocessarDiarioBordoPendenciaDevolutivaPorUeUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();
            var ues = await mediator.Send(new ObterUesCodigosPorDreQuery(filtro.DreCodigo));
            foreach (var ueCodigo in ues)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorUe, new FiltroDiarioBordoPendenciaDevolutivaDto(dreCodigo:filtro.DreCodigo,ueCodigo: ueCodigo,anoLetivo:filtro.AnoLetivo), Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
