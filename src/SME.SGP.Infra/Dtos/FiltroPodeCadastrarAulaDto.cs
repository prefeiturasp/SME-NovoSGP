using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class FiltroPodeCadastrarAulaDto
    {
        public FiltroPodeCadastrarAulaDto() { }
        public FiltroPodeCadastrarAulaDto(long aulaId, string turmaCodigo, long[] componentesCurriculares, DateTime dataAula, bool ehRegencia, TipoAula tipoAula)
        {
            AulaId = aulaId;
            TurmaCodigo = turmaCodigo;
            ComponentesCurriculares = componentesCurriculares;
            DataAula = dataAula;
            EhRegencia = ehRegencia;
            TipoAula = tipoAula;
        }

        public long AulaId { get; set; }
        public string TurmaCodigo { get; set; }
        public long[] ComponentesCurriculares { get; set; }
        public DateTime DataAula { get; set; }
        public bool EhRegencia { get; set; }
        public TipoAula TipoAula { get; set; }
    }
}
