using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery : IRequest<bool>
    {
        public PossuiAulaCadastradaPorPeriodoTurmaDisciplinaQuery(DateTime periodoInicio, DateTime periodoFim, string turmaCodigo, string disciplinaId)
        {
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
            TurmaCodigo = turmaCodigo;
            DisciplinaId = disciplinaId;
        }

        public DateTime PeriodoInicio { get; }
        public DateTime PeriodoFim { get; }
        public string TurmaCodigo { get; }
        public string DisciplinaId { get; }
    }
}
