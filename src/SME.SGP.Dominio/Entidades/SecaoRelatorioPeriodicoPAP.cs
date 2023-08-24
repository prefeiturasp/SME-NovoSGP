namespace SME.SGP.Dominio
{
    public class SecaoRelatorioPeriodicoPAP : EntidadeBase
    {
        public int QuestionarioId { get; set; }
        public Questionario Questionario { get; set; }
        public string NomeComponente { get; set; }
        public string Nome { get; set; }
        public int Ordem { get; set; }
        public int Etapa { get; set; }
        public bool Excluido { get; set; }
    }
}
