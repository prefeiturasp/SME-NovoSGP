using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuariosIdPorCodigoUeQueryValidator : AbstractValidator<ObterUsuariosIdPorCodigoUeQuery>
    {
        public ObterUsuariosIdPorCodigoUeQueryValidator()
        {
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da UE deve ser informado.");
        }
    }
}
