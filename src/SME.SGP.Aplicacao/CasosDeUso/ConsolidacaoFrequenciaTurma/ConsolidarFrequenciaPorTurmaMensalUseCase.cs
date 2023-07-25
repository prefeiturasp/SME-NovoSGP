using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SME.SGP.Aplicacao
{
    public class ConsolidarFrequenciaPorTurmaMensalUseCase : ConsolidarFrequenciaPorTurmaAbstractUseCase, IConsolidarFrequenciaPorTurmaMensalUseCase
    {
        private DateTime _dataInicio;

        public ConsolidarFrequenciaPorTurmaMensalUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override TipoConsolidadoFrequencia TipoConsolidado => TipoConsolidadoFrequencia.Mensal;

        protected override (DateTime?, DateTime?) Periodos => (_dataInicio, null);

        protected override async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            var dataMesAnterior = Filtro.Data.AddMonths(-1);
            _dataInicio = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, 01);
            var dataFim = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, DateTime.DaysInMonth(dataMesAnterior.Year, dataMesAnterior.Month));
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(codigoTurma, (dataFim, dataFim)));
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaPorTurmaPeriodoQuery(codigoTurma, _dataInicio, dataFim));

            return from ft in frequenciaTurma
                   join a in alunos on ft.AlunoCodigo equals a.CodigoAluno
                   select ft;
        }
    }
}
