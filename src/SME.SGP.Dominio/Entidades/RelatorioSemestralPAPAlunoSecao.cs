namespace SME.SGP.Dominio
{
    public class RelatorioSemestralPAPAlunoSecao
    {
        public long Id { get; set; }
        public long RelatorioSemestralPAPAlunoId { get; set; }
        public RelatorioSemestralPAPAluno RelatorioSemestralPAPAluno { get; set; }
        public long SecaoRelatorioSemestralPAPId { get; set; }
        public SecaoRelatorioSemestralPAP SecaoRelatorioSemestralPAP { get; set; }
        public string Valor { get; set; }
    }
}