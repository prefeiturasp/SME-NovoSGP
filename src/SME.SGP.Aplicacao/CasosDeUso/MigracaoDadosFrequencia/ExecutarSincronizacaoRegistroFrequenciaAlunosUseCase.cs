using MediatR;
using SME.SGP.Infra;
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
            var frequencias = mensagemRabbit.ObterObjetoMensagem<ParametroFrequenciasPersistirDto>();

            await mediator.Send(new InserirVariosRegistrosFrequenciaAlunoCommand(frequencias.FrequenciasPersistir));

            return true;
        }
    }
}
