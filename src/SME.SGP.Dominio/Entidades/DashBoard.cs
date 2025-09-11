using System.Diagnostics.CodeAnalysis;

namespace SME.SGP.Dominio
{
    [ExcludeFromCodeCoverage]
    public class DashBoard
    {
        public string Descricao { get; set; }
        public bool UsuarioTemPermissao { get; set; }
        public bool TurmaObrigatoria { get; set; }
        public string Rota { get; set; }
        public string Icone { get; set; }
    }

}
