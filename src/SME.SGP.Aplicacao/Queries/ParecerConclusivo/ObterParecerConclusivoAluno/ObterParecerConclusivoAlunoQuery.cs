using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoAlunoQuery : IRequest<ConselhoClasseParecerConclusivo>
    {
        public ObterParecerConclusivoAlunoQuery(string alunoCodigo, string turmaCodigo, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma, bool consideraHistorico = false)
        {
            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
            PareceresDaTurma = pareceresDaTurma;
            ConsideraHistorico = consideraHistorico;
        }

        public string AlunoCodigo { get; }
        public string TurmaCodigo { get; }
        public IEnumerable<ConselhoClasseParecerConclusivo> PareceresDaTurma { get; }
        public bool ConsideraHistorico { get; set; }
    }
}
