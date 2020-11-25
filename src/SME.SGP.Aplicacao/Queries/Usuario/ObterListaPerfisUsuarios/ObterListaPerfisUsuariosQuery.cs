using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterListaPerfisUsuariosQuery : IRequest<IEnumerable<PrioridadePerfil>>
    {
        public ObterListaPerfisUsuariosQuery(Guid perfilUsuario)
        {
            PerfilUsuario = perfilUsuario;
        }

        public Guid PerfilUsuario { get; set; }
    }
}
