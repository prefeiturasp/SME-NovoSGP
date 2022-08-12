using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPodeCadastrarAulaDto
    {
        public FiltroPodeCadastrarAulaDto(long aulaId, string turmaCodigo, long componenteCurricular, DateTime dataAula, bool ehRegencia, TipoAula tipoAula)
        {
            AulaId = aulaId;
            TurmaCodigo = turmaCodigo;
            ComponenteCurricular = componenteCurricular;
            DataAula = dataAula;
            EhRegencia = ehRegencia;
            TipoAula = tipoAula;
        }

        public long AulaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long ComponenteCurricular { get; set; }
        public DateTime DataAula { get; set; }
        public bool EhRegencia { get; set; }
        public TipoAula TipoAula { get; set; }
    }
}
