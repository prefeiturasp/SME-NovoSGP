using System;

namespace SME.SGP.Dominio
{
    public class SuporteUsuario
    {
        public long Id { get; set; }
        public string UsuarioAdministrador { get; set; }
        public string UsuarioSimulado { get; set; }
        public DateTime DataAcesso { get; set; }
        public string TokenAcesso { get; set; }
    }
}
