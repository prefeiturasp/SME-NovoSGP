using Dapper.Contrib.Extensions;

namespace SME.SGP.Dominio
{
    public class RelatorioSemestralPAPAlunoSecao
    {
        [Key]
        public long Id { get; set; }
        public long RelatorioSemestralPAPAlunoId { get; set; }
        [Computed]
        public RelatorioSemestralPAPAluno RelatorioSemestralPAPAluno { get; set; }
        public long SecaoRelatorioSemestralPAPId { get; set; }
        [Computed]
        public SecaoRelatorioSemestralPAP SecaoRelatorioSemestralPAP { get; set; }
        public string Valor { get; set; }
    }
}