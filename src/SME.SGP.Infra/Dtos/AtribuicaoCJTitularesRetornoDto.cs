using System;
using System.Collections.Generic;

namespace SME.SGP.Infra
{
    public class AtribuicaoCJTitularesRetornoDto
    {
        public AtribuicaoCJTitularesRetornoDto()
        {
            Itens = new List<AtribuicaoCJTitularesRetornoItemDto>();
        }

        public DateTime? AlteradoEm { get; set; }
        public string AlteradoPor { get; set; }
        public string AlteradoRF { get; set; }
        public DateTime CriadoEm { get; set; }
        public string CriadoPor { get; set; }
        public string CriadoRF { get; set; }
        public List<AtribuicaoCJTitularesRetornoItemDto> Itens { get; set; }
    }
}