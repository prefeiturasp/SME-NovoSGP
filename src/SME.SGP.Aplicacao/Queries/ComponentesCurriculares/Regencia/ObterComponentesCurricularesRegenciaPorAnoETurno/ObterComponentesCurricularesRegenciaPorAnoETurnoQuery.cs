using MediatR;
using System.Collections.Generic;
using SME.SGP.Infra;
using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesRegenciaPorAnoETurnoQuery : IRequest<IEnumerable<DisciplinaDto>>
    {
        public ObterComponentesCurricularesRegenciaPorAnoETurnoQuery(long ano, long turno)
        {
            Ano = ano;
            Turno = turno;
        }

        public long Ano { get; set; }
        public long Turno { get; set; }
    }

}
