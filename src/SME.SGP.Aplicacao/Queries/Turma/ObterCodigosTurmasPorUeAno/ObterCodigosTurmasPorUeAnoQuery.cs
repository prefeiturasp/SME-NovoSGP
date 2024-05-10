using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterCodigosTurmasPorUeAnoQuery : IRequest<IEnumerable<string>>
    {
        public ObterCodigosTurmasPorUeAnoQuery(int anoLetivo, string ueCodigo)
        {
            AnoLetivo = anoLetivo;
            UeCodigo = ueCodigo;
        }

        public int AnoLetivo { get; }
        public string UeCodigo { get; }
    }

    public class ObterCodigosTurmasPorUeAnoQueryValidator : AbstractValidator<ObterCodigosTurmasPorUeAnoQuery>
    {
        public ObterCodigosTurmasPorUeAnoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de turmas");

            RuleFor(a => a.UeCodigo)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado para consulta de turmas");
        }
    }
}
