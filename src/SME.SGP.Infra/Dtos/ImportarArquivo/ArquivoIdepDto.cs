using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoIdepDto : ArquivoImportacaoBaseDto {
        public ArquivoIdepDto(int serieAno, string codigoEOLEscola, decimal nota, int anoLetivo)
        {
            SerieAno = serieAno;
            AnoLetivo = anoLetivo;
            CodigoEOLEscola = codigoEOLEscola;
            Nota = nota;
        }
        public int SerieAno { get; set; }
        public int AnoLetivo { get; set; }
        public decimal Nota { get; set; }
    }
}
