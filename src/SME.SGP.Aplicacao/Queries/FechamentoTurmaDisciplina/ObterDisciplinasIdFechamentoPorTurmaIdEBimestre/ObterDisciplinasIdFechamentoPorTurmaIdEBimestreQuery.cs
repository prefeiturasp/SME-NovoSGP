using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery : IRequest<IEnumerable<int>>
    {
        public ObterDisciplinasIdFechamentoPorTurmaIdEBimestreQuery(long turmaId, int bimestre)
        {
            TurmaId = turmaId;
            Bimestre = bimestre;
        }

        public long TurmaId { get; set; }
        public int Bimestre { get; set; }
    }
}
