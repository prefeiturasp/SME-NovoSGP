namespace SME.SGP.Infra
{
    public class UsuarioAutenticacaoRetornoDto
    {
        public UsuarioAutenticacaoRetornoDto()
        {
            Autenticado = false;
            ModificarSenha = false;
        }

        public bool Autenticado { get; set; }
        public bool ModificarSenha { get; set; }
        public PerfisPorPrioridadeDto PerfisUsuario { get; set; }
        public string Token { get; set; }
    }
}