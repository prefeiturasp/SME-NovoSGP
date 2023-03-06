using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarRegistroFrequenciaAlunoTurmaUseCase : AbstractUseCase, ITratarRegistroFrequenciaAlunoTurmaUseCase
    {
        public TratarRegistroFrequenciaAlunoTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var filtro = param.ObterObjetoMensagem<FiltroTratarRegistroFrequenciaDto>();
            var turmas = await mediator.Send(new ObterCodigosTurmasPorUeAnoQuery(filtro.AnoLetivo, filtro.UeId.ToString()));

            foreach(string codigoTurma in turmas)
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoAula, codigoTurma, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
