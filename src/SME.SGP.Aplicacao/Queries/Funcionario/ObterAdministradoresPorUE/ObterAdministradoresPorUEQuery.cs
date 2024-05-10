using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterAdministradoresPorUEQuery : IRequest<string[]>
    {
        public ObterAdministradoresPorUEQuery(string codigoUe)
        {
            CodigoUe = codigoUe;
        }

        public string CodigoUe { get; set; }
    }

    public class ObterAdministradoresPorUEQueryValidator : AbstractValidator<ObterAdministradoresPorUEQuery>
    {
        public ObterAdministradoresPorUEQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado.");
        }
    }
}
