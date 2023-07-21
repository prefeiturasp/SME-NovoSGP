using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasMensalUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasMensalUseCase
    {
        public ConsolidarFrequenciaTurmasMensalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno, new FiltroAnoDto(DateTime.Now, TipoConsolidadoFrequencia.Mensal), Guid.NewGuid(), null));

            return true;
        }
    }
}
