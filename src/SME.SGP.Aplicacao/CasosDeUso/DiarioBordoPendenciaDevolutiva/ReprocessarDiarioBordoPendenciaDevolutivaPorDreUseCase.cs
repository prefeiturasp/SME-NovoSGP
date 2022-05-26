using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase : AbstractUseCase, IReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase
    {
        public ReprocessarDiarioBordoPendenciaDevolutivaPorDreUseCase(IMediator mediator) : base(mediator)
        {

        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            try
            {
                var filtro = param.ObterObjetoMensagem<FiltroDiarioBordoPendenciaDevolutivaDto>();
                var dres = await mediator.Send(new ObterIdsDresQuery());
                foreach (var dreId in dres)
                {
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorUe,
                                          new FiltroDiarioBordoPendenciaDevolutivaDto(dreId: dreId, anoLetivo: filtro.AnoLetivo), Guid.NewGuid(), null));
                }

                return true;
            }
            catch (Exception ex)
            {
                await mediator.Send(new SalvarLogViaRabbitCommand($"Não foi possível executar a verificação de pendencias de devolutivas por DRE", LogNivel.Critico, LogContexto.Devolutivas, ex.Message));
                return false;
            }
        }
    }
}
