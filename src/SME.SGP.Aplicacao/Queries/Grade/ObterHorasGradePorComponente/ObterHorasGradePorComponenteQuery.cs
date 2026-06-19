using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterHorasGradePorComponenteQuery: IRequest<int>
    {
        public ObterHorasGradePorComponenteQuery(long gradeId, long componenteCurricular, int ano)
        {
            GradeId = gradeId;
            ComponenteCurricular = componenteCurricular;
            Ano = ano;
        }

        public long GradeId { get; set; }
        public long ComponenteCurricular { get; set; }
        public int Ano { get; set; }
    }
}
