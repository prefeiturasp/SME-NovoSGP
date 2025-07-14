using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class HistoricoReinicioSenha: EntidadeBase
    {
        public string UsuarioRf { get; set; }
        public string DreCodigo { get; set; }
        public string UeCodigo { get; set; }
    }
}
