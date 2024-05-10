using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaPorTurmaSemanalUseCase : ConsolidarFrequenciaPorTurmaAbstractUseCase, IConsolidarFrequenciaPorTurmaSemanalUseCase
    {
        private (DateTime DataInicio, DateTime DataFim) PeriodoInicioFim;
        public ConsolidarFrequenciaPorTurmaSemanalUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override TipoConsolidadoFrequencia TipoConsolidado => TipoConsolidadoFrequencia.Semanal;

        protected override (DateTime? DataInicio, DateTime? DataFim) Periodos => PeriodoInicioFim;

        protected override async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            PeriodoInicioFim = ObterPeriodoInicioFimDaSemana(Filtro.Data);
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(codigoTurma, (PeriodoInicioFim.DataFim, PeriodoInicioFim.DataFim)));
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaPorTurmaPeriodoQuery(codigoTurma, PeriodoInicioFim.DataInicio, PeriodoInicioFim.DataFim));

            return from ft in frequenciaTurma
                   join a in alunos on ft.AlunoCodigo equals a.CodigoAluno
                   select ft;
        }

        private (DateTime DataInicio, DateTime DataFim) ObterPeriodoInicioFimDaSemana(DateTime data)
        {
            var diaDaSemana = data.DayOfWeek;

            if (diaDaSemana == DayOfWeek.Sunday || diaDaSemana == DayOfWeek.Saturday)
                return diaDaSemana == DayOfWeek.Sunday ? (data.AddDays(-6), data) : (data.AddDays(-5), data);

            while (data.DayOfWeek != DayOfWeek.Monday)
                data = data.AddDays(-1);

            return (data, data.AddDays(6));
        }
    }
}
