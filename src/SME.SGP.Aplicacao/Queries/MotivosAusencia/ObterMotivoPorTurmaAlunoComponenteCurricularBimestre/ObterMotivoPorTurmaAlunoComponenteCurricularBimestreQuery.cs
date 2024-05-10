using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery : IRequest<PaginacaoResultadoDto<JustificativaAlunoDto>>
    {
        public ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery(long turmaId, long componenteCurricularId, long alunoCodigo, int bimestre, int? semestre)
        {
            TurmaId = turmaId;
            ComponenteCurricularId = componenteCurricularId;
            AlunoCodigo = alunoCodigo;
            Bimestre = bimestre;
            Semestre = semestre;
        }

        public long TurmaId { get; set; }
        public long ComponenteCurricularId { get; set; }
        public long AlunoCodigo { get; set; }
        public int Bimestre { get; set; }
        public int? Semestre { get; set; }
    }
}
