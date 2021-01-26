using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistrosIndividuaisPorAlunoPeriodoQuery : IRequest<RegistrosIndividuaisPeriodoDto>
    {
        public ObterRegistrosIndividuaisPorAlunoPeriodoQuery(long turmaId, long alunoCodigo, long componenteCurricularId, DateTime dataInicio, DateTime dataFim)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
            DataInicio = dataInicio;
            DataFim = dataFim;
        }

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
