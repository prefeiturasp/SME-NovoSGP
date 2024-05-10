using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAulasPrevistasPorCodigoUeQuery : IRequest<IEnumerable<AulaPrevista>>
    {
        public ObterAulasPrevistasPorCodigoUeQuery(long codigoUe, bool obterPorCache = true)
        {
            CodigoUe = codigoUe;
            ObterPorCache = obterPorCache;
        }

        public long CodigoUe { get; set; }
        public bool ObterPorCache { get; set; }
    }

    public class ObterAulasPrevistasPorCodigoUeQueryValidator : AbstractValidator<ObterAulasPrevistasPorCodigoUeQuery>
    {
        public ObterAulasPrevistasPorCodigoUeQueryValidator()
        {
            RuleFor(x => x.CodigoUe).NotNull().WithMessage("É necessário informar o código da UE para consultar as aulas previstas.");
        }
    }
}