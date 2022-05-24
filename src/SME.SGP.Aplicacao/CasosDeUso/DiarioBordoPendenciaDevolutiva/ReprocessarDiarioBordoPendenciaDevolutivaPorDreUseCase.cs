using MediatR;
using SME.SGP.Aplicacao.Interfaces;
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

        public async Task Executar(int anoLetivo)
        {
            var dres = await mediator.Send(new ObterIdsDresQuery());
            foreach (var dreId in dres)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaReprocessarDiarioBordoPendenciaDevolutivaPorDre, 
                                                                new FiltroDiarioBordoPendenciaDevolutivaDto(dreCodigo:dreId,anoLetivo: anoLetivo),Guid.NewGuid(),null));
            }
        }
    }
}
