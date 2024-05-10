using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

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
