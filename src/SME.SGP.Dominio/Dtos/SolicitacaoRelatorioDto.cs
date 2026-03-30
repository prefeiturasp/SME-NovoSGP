using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio.Dtos
{
    public class SolicitacaoRelatorioDto
    {
        public Guid CodigoCorrelacao { get; set; }
        public string FiltrosUsados { get; set; }
        public TipoFormatoRelatorio ExtensaoRelatorio { get; set; }
        public TipoRelatorio TipoRelatorio { get; set; }
        public StatusSolicitacao StatusSolicitacao { get; set; }
        public string UsuarioQueSolicitou { get; set; }
        public DateTime SolicitadoEm { get; set; }
    }
}
