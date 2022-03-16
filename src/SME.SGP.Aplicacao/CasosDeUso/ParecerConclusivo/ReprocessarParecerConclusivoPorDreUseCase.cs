using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ReprocessarParecerConclusivoPorDreUseCase : AbstractUseCase, IReprocessarParecerConclusivoPorDreUseCase
    {
        public ReprocessarParecerConclusivoPorDreUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroDreUeTurmaDto>();
            var ues = await mediator.Send(new ObterUesCodigosPorDreQuery(filtro.DreId));
            foreach (var ueCodigo in ues)
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaAtualizarParecerConclusivoAlunoPorUe
                    , new FiltroDreUeTurmaDto(filtro.AnoLetivo, filtro.DreId, ueCodigo), Guid.NewGuid(), null));

            return true;
        }
    }
}
