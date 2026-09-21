using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDashBoardPorPerfilQuery : IRequest<IEnumerable<DashBoard>>
    {
        private static ObterDashBoardPorPerfilQuery _instance;
        public static ObterDashBoardPorPerfilQuery Instance => _instance ??= new();
    }
}
