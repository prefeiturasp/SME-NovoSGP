using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidacaoDashBoardFrequenciaPorDataETipoUseCase : AbstractUseCase, IConsolidacaoDashBoardFrequenciaPorDataETipoUseCase
    {
        public ConsolidacaoDashBoardFrequenciaPorDataETipoUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagemRabbit)
        {
            var filtro = mensagemRabbit.ObterObjetoMensagem<FiltroConsolidadoDashBoardFrequenciaDto>();

            if (filtro == null)
                return false;

            try
            {               

                // excluir consolidação por tipo e data

                // inserir consolidação por tipo e data
                await mediator.Send(new InserirConsolidacaoDashBoardFrequenciaCommand(filtro.TurmaId, filtro.DataAula, filtro.TipoPeriodo));

                var turmaTratada = await mediator.Send(new TrataSincronizacaoInstitucionalTurmaCommand(turmaEOL, turmaSGP));

                if (!turmaTratada)
                {
                    throw new Exception($"Não foi possível realizar o tratamento da turma id {filtro.CodigoTurma}.");
                }
            }
            catch (Exception ex)
            {
                SentrySdk.CaptureMessage($"Não foi possível realizar o tratamento da turma id {filtro.CodigoTurma}.", Sentry.Protocol.SentryLevel.Error);
                SentrySdk.CaptureException(ex);
                throw;
            }
            return true;
        }
    }
}
