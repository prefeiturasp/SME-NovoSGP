using MediatR;
using Sentry;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalDreSyncUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalDreSyncUseCase
    {
        public ExecutarSincronizacaoInstitucionalDreSyncUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
            var codigosDre = await mediator.Send(new ObterCodigosDresQuery());

            if (codigosDre == null || !codigosDre.Any())
            {
                throw new NegocioException("Não foi possível localizar as Dres no EOL para a sincronização instituicional");
            }

            foreach (var codigoDre in codigosDre)
            {
                try
                {
                    var publicarTratamentoDre = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.SincronizaEstruturaInstitucionalDreTratar, codigoDre, param.CodigoCorrelacao, null));
                    if (!publicarTratamentoDre)
                    {
                        var mensagem = $"Não foi possível inserir a Dre : {publicarTratamentoDre} na fila de sync.";
                        SentrySdk.CaptureMessage(mensagem);
                    }
                }
                catch (Exception ex)
                {
                    SentrySdk.CaptureException(ex);
                }
            }

            return true;
        }
    }
}
