using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorIdsQuery()
        { }

        public ObterComponentesCurricularesPorIdsQuery(long[] ids, bool? possuiTerritorio = false)
        {
            Ids = ids;
            PossuiTerritorio = possuiTerritorio;
        }

        public long[] Ids { get; set; }

        public bool? PossuiTerritorio { get; set; }
    }

}
