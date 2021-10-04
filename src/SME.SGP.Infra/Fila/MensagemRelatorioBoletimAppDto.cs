using System;

namespace SME.SGP.Infra
{
    public class MensagemRelatorioBoletimAppDto
    {
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
        public int Semestre { get; set; }
        public string TurmaCodigo { get; set; }
        public int AnoLetivo { get; set; }
        public int Modalidade { get; set; }
        public int ModalidadeCodigo { get; set; }
        public string AlunosCodigo { get; set; }
        public bool ConsideraHistorico { get; set; }
        public Guid CodigoArquivo { get; set; }
    }
}
