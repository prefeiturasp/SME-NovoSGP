using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesRegenciaPorAnoQuery : IRequest<IEnumerable<ComponenteCurricularEol>>
    {
        public ObterComponentesRegenciaPorAnoQuery(int anoTurma)
        {
            this.AnoTurma = anoTurma;
        }
        public int AnoTurma { get; set; }
    }
}