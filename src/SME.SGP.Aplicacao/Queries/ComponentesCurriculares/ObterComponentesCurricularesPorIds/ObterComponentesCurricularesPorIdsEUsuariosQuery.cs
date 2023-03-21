using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesPorIdsEUsuariosQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesPorIdsEUsuariosQuery()
        { }

        public ObterComponentesCurricularesPorIdsEUsuariosQuery(long[] ids, bool? possuiTerritorio = false, string codigoTurma = null)
        {
            Ids = ids;
            PossuiTerritorio = possuiTerritorio;
            CodigoTurma = codigoTurma;
        }

        public long[] Ids { get; set; }

        public bool? PossuiTerritorio { get; set; }
        public string CodigoTurma { get; set; }
    }

}
