using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdsPendenciaDevolutivaPorAnoLetivoQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdsPendenciaDevolutivaPorAnoLetivoQuery(int anoLetivo, string codigoUe)
        {
            AnoLetivo = anoLetivo;
            CodigoUe = codigoUe;
        }

        public int AnoLetivo { get; set; }
        public string CodigoUe { get; set; }
    }

    public class ObterIdsPendenciaDevolutivaPorAnoLetivoQueryValidator : AbstractValidator<ObterIdsPendenciaDevolutivaPorAnoLetivoQuery>
    {
        public ObterIdsPendenciaDevolutivaPorAnoLetivoQueryValidator()
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
