using MediatR;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class ObterProfessorTitularPorTurmaEComponenteCurricularQuery : IRequest<ProfessorTitularDisciplinaEol>
    {
        public ObterProfessorTitularPorTurmaEComponenteCurricularQuery(string turmaCodigo, string componenteCurricularCodigo)
        {
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }

        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }
    }
}
