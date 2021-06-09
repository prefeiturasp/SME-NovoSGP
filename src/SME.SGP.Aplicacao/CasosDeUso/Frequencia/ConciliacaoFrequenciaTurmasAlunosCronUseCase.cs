using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConciliacaoFrequenciaTurmasAlunosCronUseCase : AbstractUseCase, IConciliacaoFrequenciaTurmasAlunosCronUseCase
    {
        public ConciliacaoFrequenciaTurmasAlunosCronUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task Executar()
        {
            var mensagem = new FiltroCalculoFrequenciaDataRereferenciaDto(DateTime.Today);
            await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.RotaConciliacaoFrequenciaTurmasAlunosSync, mensagem, Guid.NewGuid(), null, false));
        }

    }


}
