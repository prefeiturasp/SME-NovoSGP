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
    }
}
