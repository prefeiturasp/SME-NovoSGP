using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDashBoardPorPerfilQuery : IRequest<IEnumerable<DashBoard>>
    {
        private static ObterDashBoardPorPerfilQuery _instance;
        public static ObterDashBoardPorPerfilQuery Instance => _instance ??= new();
    }
}
