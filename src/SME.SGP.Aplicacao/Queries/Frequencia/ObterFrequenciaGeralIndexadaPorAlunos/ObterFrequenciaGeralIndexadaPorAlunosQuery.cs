using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterFrequenciaGeralIndexadaPorAlunosQuery : IRequest<Dictionary<string, FrequenciaAluno>>
    {
        public string[] CodigosAlunos { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }

        public ObterFrequenciaGeralIndexadaPorAlunosQuery(string[] codigosAlunos, string turmaCodigo, string componenteCurricularCodigo)
        {
            CodigosAlunos = codigosAlunos;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }
    }
}
