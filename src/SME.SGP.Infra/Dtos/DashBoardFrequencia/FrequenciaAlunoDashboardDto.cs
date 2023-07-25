using System;
using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra
{
    public class FrequenciaAlunoDashboardDto
    {        
        public string Descricao { get; set; }
        public string DreCodigo { get; set; }        
        public TipoFrequencia TipoFrequenciaAluno { get; set; }
        public int Presentes { get; set; } 
        public int Remotos { get; set; } 
        public int Ausentes { get; set; } 
        public long TotalAulas { get; set; }
        public long TotalFrequencias { get; set; }
        public int QuantidadeAcimaMinimoFrequencia { get; set; }
        public int QuantidadeAbaixoMinimoFrequencia { get; set; }
    }
}
