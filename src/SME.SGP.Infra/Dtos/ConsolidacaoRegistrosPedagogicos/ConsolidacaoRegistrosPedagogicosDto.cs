using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class ConsolidacaoRegistrosPedagogicosDto
    {
        public long PeriodoEscolarId { get; set; }
        public long TurmaId { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public long ComponenteCurricularId { get; set; }
        public int QuantidadeAulas { get; set; }
        public int FrequenciasPendentes { get; set; }
        public DateTime DataUltimaFrequencia { get; set; }
        public DateTime DataUltimoPlanoAula { get; set; }
        public DateTime DataUltimoDiarioBordo { get; set; }
        public int DiarioBordoPendentes { get; set; }
        public int PlanoAulaPendentes { get; set; }
        public string NomeProfessor { get; set; }
        public string RFProfessor { get; set; }
    }
}
