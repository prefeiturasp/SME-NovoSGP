using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarRegistroFrequenciaAlunoUeUseCase : AbstractUseCase, ITratarRegistroFrequenciaAlunoUeUseCase
    {
        public TratarRegistroFrequenciaAlunoUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroTratarRegistroFrequenciaDto>();
            var ues = await mediator.Send(new ObterUesCodigosPorDreQuery(filtro.DreId));

            foreach(string UeId in ues)
            {
                filtro.UeId = UeId;
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoTurma, filtro, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
