using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TratarRegistroFrequenciaAlunoProcessamentoUseCase : AbstractUseCase, ITratarRegistroFrequenciaAlunoProcessamentoUseCase
    {
        public TratarRegistroFrequenciaAlunoProcessamentoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var listaDeRegistro = mensagemRabbit.ObterObjetoMensagem<List<RegistroFrequenciaAulaParcialDto>>();
     
            return await mediator.Send(new ProcessarCargaReferenciaAulaRegistroFrequenciaAlunoCommand(listaDeRegistro));

        }
    }
}
