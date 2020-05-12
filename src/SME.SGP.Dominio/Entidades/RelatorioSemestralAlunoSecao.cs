namespace SME.SGP.Dominio
{
    public class RelatorioSemestralAlunoSecao
    {
        public long Id { get; set; }
        public long RelatorioSemestralAlunoId { get; set; }
        public RelatorioSemestralAluno RelatorioSemestralAluno { get; set; }
        public long SecaoRelatorioSemestralId { get; set; }
        public SecaoRelatorioSemestral SecaoRelatorioSemestral { get; set; }
        public string Valor { get; set; }
        public int Ordem { get; set; }
    }
}