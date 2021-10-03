using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterParecerConclusivoAlunoQuery : IRequest<ConselhoClasseParecerConclusivo>
    {
        public ObterParecerConclusivoAlunoQuery(string alunoCodigo, string turmaCodigo, IEnumerable<ConselhoClasseParecerConclusivo> pareceresDaTurma)
        {
            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
            PareceresDaTurma = pareceresDaTurma;
        }

        public string AlunoCodigo { get; }
        public string TurmaCodigo { get; }
        public IEnumerable<ConselhoClasseParecerConclusivo> PareceresDaTurma { get; }
    }
}
