using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class ExecutarSincronizacaoInstitucionalUeTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalUeTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalUeTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var ueCodigo = mensagemRabbit.Mensagem.ToString();

            if (string.IsNullOrEmpty(ueCodigo))
                throw new NegocioException("Não foi possível localizar o código da Ue para tratar o Sync.");

            var ueEol = await mediator.Send(new ObterUeDetalhesParaSincronizacaoInstitucionalQuery(ueCodigo));

            var ueSgp = await mediator.Send(new ObterUeComDrePorCodigoQuery(ueCodigo));

            

            if (await mediator.Send(new TrataSincronizacaoInstitucionalUeCommand(ueEol, ueSgp)))
                return await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalTurmasSync, ueCodigo, mensagemRabbit.CodigoCorrelacao, null));
            else
                throw new NegocioException($"Não foi possível sincronizar a UE de código {ueCodigo}");

        }
    }
}
