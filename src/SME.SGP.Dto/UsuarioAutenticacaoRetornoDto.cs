using System.Collections.Generic;

namespace SME.SGP.Dto
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
        public IEnumerable<PerfilDto> Perfis { get; set; }
        public string Token { get; set; }
    }
}