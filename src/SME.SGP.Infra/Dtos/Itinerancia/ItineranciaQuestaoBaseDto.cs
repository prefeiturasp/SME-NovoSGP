using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ItineranciaQuestaoBaseDto
    {
        public long Id { get; set; }
        public string Nome { get; set; }
        public int Ordem { get; set; }
        public TipoQuestionario Tipo { get; set; }
        public bool Obrigatorio { get; set; }
    }
}
