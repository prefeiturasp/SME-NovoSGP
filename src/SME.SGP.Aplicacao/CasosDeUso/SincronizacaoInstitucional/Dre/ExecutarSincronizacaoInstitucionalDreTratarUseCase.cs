using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalDreTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalDreTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalDreTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var dreCodigo = param.Mensagem?.ToString();

            if (string.IsNullOrEmpty(dreCodigo))
                throw new NegocioException("Não foi possível localizar o código da Dre.");

            await mediator.Send(new TrataSincronizacaoInstitucionalDreCommand(long.Parse(dreCodigo)));

            return true;

        }
    }
}
