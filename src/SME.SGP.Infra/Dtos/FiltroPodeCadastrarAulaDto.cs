using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPodeCadastrarAulaDto
    {
        public FiltroPodeCadastrarAulaDto(long aulaId, string turmaCodigo, long componenteCurricular, DateTime dataAula, bool ehRegencia = false)
        {
            AulaId = aulaId;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
            DataAula = dataAula;
            EhRegencia = ehRegencia;
        }

        public long AulaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public DateTime DataAula { get; set; }
        public bool EhRegencia { get; set; }
    }
}
