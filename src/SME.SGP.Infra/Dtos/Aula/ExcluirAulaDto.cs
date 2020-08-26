using SME.SGP.Dominio;

namespace SME.SGP.Infra
{
    public class ExcluirAulaDto
    {
        public long AulaId { get; set; }
        public RecorrenciaAula RecorrenciaAula { get; set; }
        public string ComponenteCurricularNome { get; set; }
    }
}
