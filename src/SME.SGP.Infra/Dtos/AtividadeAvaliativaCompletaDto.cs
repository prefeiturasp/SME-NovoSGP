using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AtividadeAvaliativaCompletaDto : AtividadeAvaliativaDto
    {
        public DateTime? AlteradoEm { get; set; }
        
        public string AlteradoPor { get; set; }
        
        public string AlteradoRF { get; set; }
        
        public List<AtividadeAvaliativaRegenciaDto> AtividadesRegencia { get; set; }
        
        public string Categoria { get; set; }
        
        public DateTime CriadoEm { get; set; }
        
        public bool DentroPeriodo { get; set; }
        
        public string CriadoPor { get; set; }
        
        public string CriadoRF { get; set; }
        
        public long Id { get; set; }
        
        public bool Importado { get; set; }

        public bool PodeEditarAvaliacao { get; set; }
    }
}