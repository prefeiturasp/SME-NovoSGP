namespace SME.SGP.Infra
{
    public class OpcaoRespostaDto
    {

        public QuestaoDto QuestaoComplementar { get; set; }
        public long Id { get; set; }
        public int Ordem { get; set; }
        public string Nome { get; set; }
    }
}
