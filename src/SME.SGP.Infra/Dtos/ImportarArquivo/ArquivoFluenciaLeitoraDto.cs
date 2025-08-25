using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SME.SGP.Infra.Dtos.ImportarArquivo
{
    public class ArquivoFluenciaLeitoraDto : ArquivoImportacaoBaseDto
    {
        public ArquivoFluenciaLeitoraDto(string codigoEOLTurma, string codigoEOLAluno, int anoLetivo, int fluencia, string periodo)
        {
            CodigoEOLTurma = codigoEOLTurma;
            CodigoEOLAluno = codigoEOLAluno;
            AnoLetivo = anoLetivo;
            Fluencia = fluencia;
            Periodo = periodo;
        }
        public string CodigoEOLTurma { get; set; }
        public string CodigoEOLAluno{ get; set; }
        public int AnoLetivo { get; set; }
        public int Fluencia { get; set; }
        public string Periodo { get; set; }
    }
}
