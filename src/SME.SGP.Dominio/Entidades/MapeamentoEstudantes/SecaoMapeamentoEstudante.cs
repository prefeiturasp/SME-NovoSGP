namespace SME.SGP.Dominio
{
    public class SecaoMapeamentoEstudante : EntidadeBase
    {
        public Questionario Questionario { get; set; }
        public long QuestionarioId { get; set; }

        public string Nome { get; set; }
        public int Ordem { get; set; }
        public int Etapa { get; set; }
        public bool Excluido { get; set; }
        public string? NomeComponente { get; set; }
        public MapeamentoEstudanteSecao MapeamentoEstudanteSecao { get; set; }
    }
}
