using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery : IRequest<IEnumerable<FrequenciaAluno>>
    {
        public ObterFrequenciaAlunoTurmaPorComponenteCurricularPeriodosQuery(string alunoCodigo, string componenteCurricularId, string turmaCodigo, int[] bimestres)
        {
            AlunoCodigo = alunoCodigo;
            ComponenteCurricularId = componenteCurricularId;
            TurmaCodigo = turmaCodigo;
            Bimestres = bimestres;
        }

        public string AlunoCodigo { get; set; }
        public string ComponenteCurricularId { get; set; }
        public string TurmaCodigo { get; set; }
        public int[] Bimestres { get; set; }
    }
}
