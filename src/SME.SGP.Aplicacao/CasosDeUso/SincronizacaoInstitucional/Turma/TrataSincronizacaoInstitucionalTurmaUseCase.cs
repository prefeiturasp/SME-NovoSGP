using MediatR;
using Newtonsoft.Json;
using Sentry;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class TrataSincronizacaoInstitucionalTurmaUseCase : AbstractUseCase, ITrataSincronizacaoInstitucionalTurmaUseCase
    {
        public TrataSincronizacaoInstitucionalTurmaUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var codigoTurma = JsonConvert.DeserializeObject<long>(mensagemRabbit.Mensagem.ToString());
            if (codigoTurma == 0) return true;

            try
            {
                var turmaParaTratar = await mediator.Send(new ObterTurmaEOLParaSyncEstruturaInstitucionalPorTurmaIdQuery(codigoTurma));
                if (turmaParaTratar == null)
                {
                    SentrySdk.CaptureMessage($"Não foi possível realizar o tratamento da turma id {codigoTurma}. Turma não encontrada no Eol.");
                    return true;
                }
                var turmaTratada = await mediator.Send(new TrataSincronizacaoInstitucionalTurmaCommand(turmaParaTratar));
                if (!turmaTratada)
                {
                    SentrySdk.CaptureMessage($"Não foi possível realizar o tratamento da turma id {codigoTurma}.");
                    return true;
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureException(ex);
            }
            return true;
        }
    }
}
