using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasSemanalUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasSemanalUseCase
    {
        public ConsolidarFrequenciaTurmasSemanalUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.ConsolidarFrequenciasTurmasNoAno, new FiltroAnoDto(DateTime.Now, TipoConsolidadoFrequencia.Semanal), Guid.NewGuid(), null));

            return true;
        }
    }
}
