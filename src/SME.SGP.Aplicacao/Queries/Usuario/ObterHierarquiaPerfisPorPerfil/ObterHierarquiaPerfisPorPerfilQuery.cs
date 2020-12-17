using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterHierarquiaPerfisPorPerfilQuery : IRequest<IEnumerable<PrioridadePerfil>>
    {
        public ObterHierarquiaPerfisPorPerfilQuery(Guid perfilUsuario)
        {
            PerfilUsuario = perfilUsuario;
        }

        public Guid PerfilUsuario { get; set; }
    }
}
