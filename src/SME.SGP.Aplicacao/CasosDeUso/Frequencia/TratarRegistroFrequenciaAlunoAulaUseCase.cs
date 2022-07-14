using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarRegistroFrequenciaAlunoAulaUseCase : AbstractUseCase, ITratarRegistroFrequenciaAlunoAulaUseCase
    {
        public TratarRegistroFrequenciaAlunoAulaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var codigoTurma = mensagemRabbit.Mensagem.ToString();

            var listaDeRegistroAula = await mediator.Send(new ObterListaDeRegistroFrequenciaAulaPorTurmaQuery(codigoTurma));

            if (listaDeRegistroAula.Any())
            {
                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgpFrequencia.RotaTratarCargaRegistroFrequenciaAlunoProcessamento, listaDeRegistroAula, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
