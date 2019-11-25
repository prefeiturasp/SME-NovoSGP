using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SME.SGP.Aplicacao
{
    public interface IServicoTokenJwt
    {
        string GerarToken(IEnumerable<Claim> permissionamentos);

        bool TemPerfilNoToken(string guid);
    }
}