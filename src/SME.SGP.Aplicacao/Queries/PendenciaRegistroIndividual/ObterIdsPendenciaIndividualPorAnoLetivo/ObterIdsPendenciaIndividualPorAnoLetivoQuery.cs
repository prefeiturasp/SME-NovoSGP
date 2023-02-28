using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsPendenciaIndividualPorAnoLetivoQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsPendenciaIndividualPorAnoLetivoQuery(int anoLetivo, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterIdsPendenciaIndividualPorAnoLetivoQueryValidator : AbstractValidator<ObterIdsPendenciaIndividualPorAnoLetivoQuery>
    {
        public ObterIdsPendenciaIndividualPorAnoLetivoQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
            .NotEmpty()
            .WithMessage("O ano letivo deve ser informado para realizar a consulta.");

            RuleFor(c => c.CodigoUe)
            .NotEmpty()
            .WithMessage("O código da ue deve ser informado para realizar a consulta.");
        }
    }
}
