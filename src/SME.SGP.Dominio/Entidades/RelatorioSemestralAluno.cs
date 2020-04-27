namespace SME.SGP.Dominio
{
    public class RelatorioSemestralAluno : EntidadeBase
    {
        public long RelatorioSemestralId { get; set; }
        public RelatorioSemestral RelatorioSemestral { get; set; }
        public string AlunoCodigo { get; set; }

        public bool Migrado { get; set; }
        public bool Excluido { get; set; }
    }
}