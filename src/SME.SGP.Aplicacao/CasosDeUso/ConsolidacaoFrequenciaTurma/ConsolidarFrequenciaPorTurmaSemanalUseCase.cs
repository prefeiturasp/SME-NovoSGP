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
        public ConsolidarFrequenciaPorTurmaSemanalUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override TipoConsolidadoFrequencia TipoConsolidado => TipoConsolidadoFrequencia.Semanal;

        protected override async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            var periodo = ObterPeriodoInicioFimDaSemana(Filtro.Data);
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(codigoTurma, (periodo.Item2, periodo.Item2)));
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaPorTurmaPeriodoQuery(codigoTurma, periodo.Item1, periodo.Item2));

            return from ft in frequenciaTurma
                   join a in alunos on ft.AlunoCodigo equals a.CodigoAluno
                   select ft;
        }

        private (DateTime, DateTime) ObterPeriodoInicioFimDaSemana(DateTime data)
        {
            var diaDaSemana = data.DayOfWeek;

            if (diaDaSemana == DayOfWeek.Sunday || diaDaSemana == DayOfWeek.Saturday)
                return diaDaSemana == DayOfWeek.Sunday ? (data.AddDays(-6), data) : (data.AddDays(-5), data);

            var dataSemanaPassada = data.AddDays(-7);

            while (dataSemanaPassada.DayOfWeek != DayOfWeek.Monday)
            {
                dataSemanaPassada = dataSemanaPassada.AddDays(-1);
            }

            return (dataSemanaPassada, dataSemanaPassada.AddDays(6));
        }
    }
}
