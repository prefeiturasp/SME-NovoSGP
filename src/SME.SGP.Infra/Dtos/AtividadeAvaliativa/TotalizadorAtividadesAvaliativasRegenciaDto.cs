using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Infra
{
    public class TotalizadorAtividadesAvaliativasRegenciaDto
    {
        public TotalizadorAtividadesAvaliativasRegenciaDto() { 
        }
        public TotalizadorAtividadesAvaliativasRegenciaDto(int totalAtividades, int totalRegistros)
        {
            TotalAtividades = totalAtividades;
            TotalRegistros = totalRegistros;
        }

        public int TotalAtividades { get; set; }
        public int TotalRegistros { get; set; }
    }
}
