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
        private DateTime _dataFim;

        public ConsolidarFrequenciaPorTurmaMensalUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override TipoConsolidadoFrequencia TipoConsolidado => TipoConsolidadoFrequencia.Mensal;

        protected override (DateTime?, DateTime?) Periodos => (_dataInicio, _dataFim);

        protected override async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            var dataMesAnterior = Filtro.Data.AddMonths(-1);
            _dataInicio = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, 01);
            _dataFim = new DateTime(dataMesAnterior.Year, dataMesAnterior.Month, DateTime.DaysInMonth(dataMesAnterior.Year, dataMesAnterior.Month));
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(codigoTurma, (_dataInicio, _dataFim)));
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaPorTurmaPeriodoQuery(codigoTurma, _dataInicio, _dataFim));

            return from ft in frequenciaTurma
                   join a in alunos on ft.AlunoCodigo equals a.CodigoAluno
                   select ft;
        }
    }
}
