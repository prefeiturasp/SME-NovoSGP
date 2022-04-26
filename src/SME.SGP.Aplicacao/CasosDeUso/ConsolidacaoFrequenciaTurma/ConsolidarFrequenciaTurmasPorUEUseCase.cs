using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Infra;
using System;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaTurmasPorUEUseCase : AbstractUseCase, IConsolidarFrequenciaTurmasPorUEUseCase
    {
        public ConsolidarFrequenciaTurmasPorUEUseCase(IMediator mediator) : base(mediator)
        {
        }

        public async Task<bool> Executar(MensagemRabbit mensagem)
        {
            var filtro = mensagem.ObterObjetoMensagem<FiltroConsolidacaoFrequenciaTurmaPorUe>();

            var turmas = await mediator.Send(new ObterTurmasComModalidadePorAnoUEQuery(filtro.Ano, filtro.UeId));
            foreach (var turma in turmas)
            {
                var filtroTurma = new FiltroConsolidacaoFrequenciaTurma(turma.TurmaId, turma.TurmaCodigo, turma.ModalidadeInfantil ? filtro.PercentualMinimoInfantil : filtro.PercentualMinimo);

                await mediator.Send(new PublicarFilaSgpCommand(RotasRabbitSgp.ConsolidarFrequenciasPorTurma, filtroTurma, Guid.NewGuid(), null));
            }

            return true;
        }
    }
}
