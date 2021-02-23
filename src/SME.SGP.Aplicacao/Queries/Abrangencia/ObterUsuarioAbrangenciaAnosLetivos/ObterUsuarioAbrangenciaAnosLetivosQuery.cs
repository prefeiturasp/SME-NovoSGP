using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioAbrangenciaAnosLetivosQuery : IRequest<IEnumerable<int>>
    {
        public ObterUsuarioAbrangenciaAnosLetivosQuery(string login, bool consideraHistorico, Guid perfil)
        {
            Login = login;
            ConsideraHistorico = consideraHistorico;
            Perfil = perfil;
        }

        public string Login { get; set; }
        public bool ConsideraHistorico { get; set; }
        public Guid Perfil { get; set; }
    }
}
