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
            var codigoTurma = long.Parse(mensagemRabbit.Mensagem.ToString());

            if (codigoTurma == 0) return true;

            try
            {
                var turmaEOL = await mediator.Send(new ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery(codigoTurma));
                if (turmaEOL == null)
                    throw new NegocioException($"Não foi possível realizar o tratamento da turma id {codigoTurma}. Turma não encontrada no Eol.");

                var turmaSGP = await mediator.Send(new ObterTurmaPorCodigoQuery(codigoTurma.ToString()));

                var turmaTratada = await mediator.Send(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEOL, turmaSGP));

                if (!turmaTratada)
                {
                    throw new Exception($"Não foi possível realizar o tratamento da turma id {codigoTurma}.");
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Não foi possível realizar o tratamento da turma id {codigoTurma}.");
                SentrySdk.CaptureException(ex);
                throw;
            }
            return true;
        }
    }
}
