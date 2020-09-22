using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioPlanoAulaDto
    {
        public long PlanoAulaId { get; set; }
        public Usuario Usuario { get; set; }
    }
}
