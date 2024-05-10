using System;

namespace SME.SGP.Infra
{
    public class UsuarioPerfilDto
    {
        public UsuarioPerfilDto(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }

        public string Login { get; }
        public Guid Perfil { get; }
    }
}
