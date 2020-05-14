using SME.SGP.Dominio.Enumerados;
using System;

namespace SME.SGP.Dominio
{
    public class HistoricoEmailUsuario : EntidadeBase
    {
        public long UsuarioId { get; set; }
        public string Email { get; set; }
        public AcaoHistoricoEmailUsuario Acao { get; set; }
    }
}
