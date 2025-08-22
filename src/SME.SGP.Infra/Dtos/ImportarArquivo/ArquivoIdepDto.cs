using SME.SGP.Infra.Enumerados;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoIdepDto {
        public ArquivoIdepDto(SerieAnoArquivoIdebIdepEnum serieAno, string codigoEOLEscola, decimal nota)
        {
            SerieAno = serieAno;
            CodigoEOLEscola = codigoEOLEscola;
            Nota = nota;
        }
        public SerieAnoArquivoIdebIdepEnum SerieAno { get; set; }
        public string CodigoEOLEscola { get; set; }
        public decimal Nota { get; set; }
    }
}
