using System.Collections.Generic;

namespace SME.SGP.Dto
{
    public class UsuarioAutenticacaoRetornoDto
    {
        public UsuarioAutenticacaoRetornoDto()
        {
            Autenticado = false;
            ModificarSenha = false;
            Perfis = new List<PerfilPrioritarioDto>();
        }

        public bool Autenticado { get; set; }
        public bool ModificarSenha { get; set; }
        public IList<PerfilPrioritarioDto> Perfis { get; set; }
        public string Token { get; set; }
    }
}