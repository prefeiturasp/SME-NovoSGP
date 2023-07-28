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
        private DateTime DataInicio;
        private DateTime DataFim;

        public ConsolidarFrequenciaPorTurmaMensalUseCase(IMediator mediator) : base(mediator)
        {
        }

        protected override TipoConsolidadoFrequencia TipoConsolidado => TipoConsolidadoFrequencia.Mensal;

        protected override (DateTime?, DateTime?) Periodos => (DataInicio, DataFim);

        protected override async Task<IEnumerable<FrequenciaAlunoDto>> ObterFrequenciaConsideradas(string codigoTurma)
        {
            DataInicio = new DateTime(Filtro.Data.Year, Filtro.Data.Month, 01);
            DataFim = DataInicio.AddMonths(1).AddDays(-1);
            var alunos = await mediator.Send(new ObterAlunosDentroPeriodoQuery(codigoTurma, (DataInicio, DataFim)));
            var frequenciaTurma = await mediator.Send(new ObterFrequenciaPorTurmaPeriodoQuery(codigoTurma, DataInicio, DataFim));

            return from ft in frequenciaTurma
                   join a in alunos on ft.AlunoCodigo equals a.CodigoAluno
                   select ft;
        }
    }
}
