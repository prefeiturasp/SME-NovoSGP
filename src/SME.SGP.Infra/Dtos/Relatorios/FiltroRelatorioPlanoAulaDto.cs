using SME.SGP.Dominio;

namespace SME.SGP.Infra.Dtos.Relatorios
{
    public class FiltroRelatorioPlanoAulaDto
    {
        public long PlanoAulaId { get; set; }
        public string UsuarioNome { get; set; }
        public string UsuarioRf { get; set; }
    }
}
