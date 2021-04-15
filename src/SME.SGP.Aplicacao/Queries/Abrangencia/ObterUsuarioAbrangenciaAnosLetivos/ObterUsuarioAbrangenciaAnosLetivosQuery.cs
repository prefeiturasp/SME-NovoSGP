using MediatR;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioAbrangenciaAnosLetivosQuery : IRequest<IEnumerable<int>>
    {
        public ObterUsuarioAbrangenciaAnosLetivosQuery(string login, bool consideraHistorico, Guid perfil, int anoMinimo)
        {
            Login = login;
            ConsideraHistorico = consideraHistorico;
            Perfil = perfil;
            AnoMinimo = anoMinimo;
        }

        public string Login { get; set; }
        public bool ConsideraHistorico { get; set; }
        public Guid Perfil { get; set; }

        public int AnoMinimo { get; set; }
    }
}
