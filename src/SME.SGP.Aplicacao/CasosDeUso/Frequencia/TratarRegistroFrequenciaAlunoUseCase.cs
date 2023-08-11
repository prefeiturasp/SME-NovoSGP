using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarRegistroFrequenciaAlunoUseCase : AbstractUseCase, ITratarRegistroFrequenciaAlunoUseCase
    {
        public TratarRegistroFrequenciaAlunoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            int.TryParse(mensagemRabbit.Mensagem.ToString(), out int ano);

            if (ano > 0)
            {
                var dres = await mediator.Send(ObterIdsDresQuery.Instance);

                foreach(long dreId in dres)
                {
                    var dto = new FiltroTratarRegistroFrequenciaDto() { AnoLetivo = ano, DreId = dreId };
                    await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoUe, dto, Guid.NewGuid(), null));
                }

                return true;
            }

            return false;
        }
    }
}
