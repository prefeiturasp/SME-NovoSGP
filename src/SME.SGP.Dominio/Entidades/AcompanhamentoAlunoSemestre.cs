namespace SME.SGP.Dominio
{
    public class AcompanhamentoAlunoSemestre : EntidadeBase
    {
        public AcompanhamentoAluno AcompanhamentoAluno { get; set; }
        public long AcompanhamentoAlunoId { get; set; }

        public int Semestre { get; set; }
        public string Observacoes { get; set; }
        public string PercursoIndividual { get; set; }

        public bool Excluido { get; set; }
    }
}
