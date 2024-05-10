using MediatR;
using SME.SGP.Aplicacao.Interfaces;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
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

            var turmas = await mediator.Send(new ObterTurmasComModalidadePorAnoUEQuery(filtro.Data.Year, filtro.UeId));
            foreach (var turma in turmas)
            {
                var filtroTurma = new FiltroConsolidacaoFrequenciaTurma(turma.TurmaId, turma.TurmaCodigo, turma.ModalidadeInfantil ? filtro.PercentualMinimoInfantil : filtro.PercentualMinimo, filtro.Data);

                await mediator.Send(new PublicarFilaSgpCommand(ObterRota(filtro.TipoConsolidado), filtroTurma, Guid.NewGuid(), null));
            }

            return true;
        }

        private string ObterRota(TipoConsolidadoFrequencia tipo)
        {
            var dicionario = new Dictionary<TipoConsolidadoFrequencia, string>()
            { { TipoConsolidadoFrequencia.Anual, RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurma },
              { TipoConsolidadoFrequencia.Mensal, RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurmaMensal },
              { TipoConsolidadoFrequencia.Semanal, RotasRabbitSgpFrequencia.ConsolidarFrequenciasPorTurmaSemanal } };

            return dicionario[tipo];
        }
    }
}
