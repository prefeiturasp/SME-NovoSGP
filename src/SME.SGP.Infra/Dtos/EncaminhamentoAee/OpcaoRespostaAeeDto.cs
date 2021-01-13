namespace SME.SGP.Infra
{
    public class OpcaoRespostaAeeDto
    {

        public QuestaoAeeDto QuestaoComplementar { get; set; }
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
    }
}
