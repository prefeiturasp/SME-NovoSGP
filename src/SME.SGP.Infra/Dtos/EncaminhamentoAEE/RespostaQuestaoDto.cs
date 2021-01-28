using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class RespostaQuestaoDto
    {
        public long? Id { get; set; }
        public long? OpcaoRespostaId { get; set; }
        public Arquivo Arquivo { get; set; }
        public string Texto { get; set; }
    }
}
