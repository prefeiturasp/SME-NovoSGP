using MediatR;
using System;

namespace SME.SGP.Aplicacao.Commands.FilaRabbit.PublicarFilaSincronizarAbrangenciaUsuario
{
    public class PublicarFilaSincronizarAbrangenciaUsuarioCommand : IRequest<bool>
    {
        public PublicarFilaSincronizarAbrangenciaUsuarioCommand(string login, Guid perfil)
        {
            Login = login;
            Perfil = perfil;
        }

        public string Login { get; set; }
        public Guid Perfil { get; set; }
    }
}
