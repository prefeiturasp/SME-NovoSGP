using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class HistoricoEmailUsuario : EntidadeBase
    {
        public long UsuarioId { get; set; }
        public string Email { get; set; }
        public AcaoHistoricoEmailUsuario Acao { get; set; }
    }
}
