using MediatR;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class SyncSincronizacaoInstitucionalTipoEscolaUseCase : AbstractUseCase, ITrataSincronizacaoInstitucionalDreUseCase
    {
        public SyncSincronizacaoInstitucionalTipoEscolaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit param)
        {
                        
            var tiposEscola = await mediator.Send(new ObterTiposEscolaQuery());

            if (tiposEscola == null || !tiposEscola.Any())
            {
                throw new NegocioException("Não foi possível localizar tipos de escolas para a sincronização instituicional");
            }


            foreach (var tipoEscola in tiposEscola)
            {
                try
                {
                    var publicarTraamentoTipoEscola = await mediator.Send(new PublicarFilaSgpCommand(RotasRabbit.SincronizaEstruturaInstitucionalUeTratar, tipoEscola, Guid.NewGuid(), null, fila: RotasRabbit.SincronizaEstruturaInstitucionalUeTratar));
                    if (!publicarTraamentoTipoEscola)
                    {
                        var mensagem = $"Não foi possível inserir o Tipo de Escola : {tipoEscola} na fila de sync.";
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
