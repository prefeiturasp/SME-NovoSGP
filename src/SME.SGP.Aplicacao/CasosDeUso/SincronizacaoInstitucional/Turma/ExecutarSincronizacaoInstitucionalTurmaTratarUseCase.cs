using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ExecutarSincronizacaoInstitucionalTurmaTratarUseCase : AbstractUseCase, IExecutarSincronizacaoInstitucionalTurmaTratarUseCase
    {
        public ExecutarSincronizacaoInstitucionalTurmaTratarUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<MensagemSyncTurmaDto>();

            if (filtro.CodigoTurma == 0) return true;

            try
            {
                var turmaEOL = await mediator.Send(new ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery(filtro.CodigoTurma, filtro.UeId));
                
                if (turmaEOL == null)
                    return true;

                var turmaSGP = await mediator.Send(new ObterTurmaPorCodigoQuery(filtro.CodigoTurma.ToString()));

                var turmaTratada = await mediator.Send(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEOL, turmaSGP));

                if (!turmaTratada)
                {
                    throw new Exception($"Não foi possível realizar o tratamento da turma id {filtro.CodigoTurma}.");
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Não foi possível realizar o tratamento da turma id {filtro.CodigoTurma}.", Sentry.Protocol.SentryLevel.Error );
                SentrySdk.CaptureException(ex);
                throw;
            }
            return true;
        }
    }
}
