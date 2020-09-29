using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterJuremaIdsPorComponentesCurricularIdQuery : IRequest<long[]>
    {
        public ObterJuremaIdsPorComponentesCurricularIdQuery()
        { 
        }
        public ObterJuremaIdsPorComponentesCurricularIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

}
