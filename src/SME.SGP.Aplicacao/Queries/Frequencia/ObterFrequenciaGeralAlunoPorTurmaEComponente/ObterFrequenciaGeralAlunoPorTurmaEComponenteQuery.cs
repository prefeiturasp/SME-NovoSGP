using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{ 
    public class ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery : IRequest<FrequenciaAluno>
    {
        public string AlunoCodigo { get; set; }
        public string TurmaCodigo { get; set; }
        public string ComponenteCurricularCodigo { get; set; }

        public ObterFrequenciaGeralAlunoPorTurmaEComponenteQuery(string alunoCodigo, string turmaCodigo, string componenteCurricularCodigo = "") 
        {
            AlunoCodigo = alunoCodigo;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricularCodigo = componenteCurricularCodigo;
        }
    }
}
