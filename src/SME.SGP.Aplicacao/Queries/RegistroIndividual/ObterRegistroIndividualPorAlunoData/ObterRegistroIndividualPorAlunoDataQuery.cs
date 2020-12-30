using MediatR;
using SME.SGP.Infra;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterRegistroIndividualPorAlunoDataQuery : IRequest<RegistroIndividualDto>
    {
        public ObterRegistroIndividualPorAlunoDataQuery(long turmaId, long alunoCodigo, long componenteCurricularId, DateTime data)
        {
            TurmaId = turmaId;
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
            Data = data;
        }

        public long TurmaId { get; set; }
        public long AlunoCodigo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public DateTime Data { get; set; }
    }
}
