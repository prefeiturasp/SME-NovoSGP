using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTipoCalendarioPorIdQuery : IRequest<TipoCalendario>
    {
        public ObterTipoCalendarioPorIdQuery(long id)
        {
            Id = id;
        }

        public ObterTipoCalendarioPorIdQuery()
        {

        }

        public long Id { get; set; }
    }
}
