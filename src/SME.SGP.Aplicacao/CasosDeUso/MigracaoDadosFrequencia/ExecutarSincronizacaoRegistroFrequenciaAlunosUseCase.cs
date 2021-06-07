using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase : AbstractUseCase, IExecutarSincronizacaoRegistroFrequenciaAlunosUseCase
    {
        public ExecutarSincronizacaoRegistroFrequenciaAlunosUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var registro = mensagemRabbit.ObterObjetoMensagem<RegistroFrequenciaAluno>();
            await mediator.Send(new SalvarRegistroFrequenciaAlunoCommand(registro));
            return true;
        }
    }
}
