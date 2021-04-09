using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao.CasosDeUso
{
    public class TrataSincronizacaoInstitucionalUeUseCase : AbstractUseCase, ITrataSincronizacaoInstitucionalUeUseCase
    {
        public TrataSincronizacaoInstitucionalUeUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            SentrySdk.AddBreadcrumb($"Mensagem TrataSincronizacaoInstitucionalUeUseCase", "Rabbit - TrataSincronizacaoInstitucionalUeUseCase");

            var ueCodigo = mensagemRabbit.Mensagem.ToString();

            if (string.IsNullOrEmpty(ueCodigo))
                throw new NegocioException("Não foi possível localizar o código da Ue para tratar o Sync.");

            var ueEol = await mediator.Send(new ObterUeDetalhesParaSincronizacaoInstitucionalQuery(ueCodigo));

            var ueSgp = await mediator.Send(new ObterUeComDrePorCodigoQuery(ueCodigo));

            await mediator.Send(new TrataSincronizacaoInstitucionalUeCommand(ueEol, ueSgp));

            return true;
        }
    }
}
