using SME.SGP.Dominio;
using SME.SGP.Dominio.Enumerados;

namespace SME.SGP.Infra.Dtos
{
    public class NotificacaoEncaminhamentoAEEDto
    {
        public string UsuarioRF { get; set; }
        public string UsuarioNome { get; set; }
        public long EncaminhamentoAEEId { get; set; }
        public string Motivo { get; set; }

    }
}
