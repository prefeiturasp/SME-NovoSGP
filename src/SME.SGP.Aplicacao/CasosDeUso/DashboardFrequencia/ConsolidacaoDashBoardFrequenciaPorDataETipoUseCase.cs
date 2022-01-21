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

            if (!await mediator.Send(new InserirConsolidacaoDashBoardFrequenciaCommand(filtro.TurmaId, filtro.DataAula, filtro.TipoPeriodo)))                
                throw new Exception($"Não foi possível realizar a consolidação da turma id {filtro.TurmaId} na data {filtro.DataAula}.");                

            return true;
        }
    }
}
