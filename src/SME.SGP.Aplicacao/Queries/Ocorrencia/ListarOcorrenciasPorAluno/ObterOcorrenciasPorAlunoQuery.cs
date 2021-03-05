using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterOcorrenciasPorAlunoQuery : IRequest<PaginacaoResultadoDto<OcorrenciasPorAlunoDto>>
    {
        public ObterOcorrenciasPorAlunoQuery(long turmaId, long alunoCodigo, DateTime periodoInicio, DateTime periodoFim)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            PeriodoInicio = periodoInicio;
            PeriodoFim = periodoFim;
        }

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public DateTime PeriodoInicio { get; set; }
        public DateTime PeriodoFim { get; set; }
    }
}
