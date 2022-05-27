using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterFuncionariosPorDreEolQueryValidator : AbstractValidator<ObterFuncionariosPorDreEolQuery>
    {
        public ObterFuncionariosPorDreEolQueryValidator()
        {
            RuleFor(c => c.Perfil)
                .NotNull()
                .WithMessage("O perfil deve ser informado.");

            RuleFor(c => c.DreCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da Dre deve ser informado.");

            RuleFor(c => c.UeCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código da Ue deve ser informado.");

            RuleFor(c => c.RfCodigo)
                .NotEmpty()
                .NotNull()
                .WithMessage("O código do RF deve ser informado.");

            RuleFor(c => c.NomeServidor)
                .NotEmpty()
                .NotNull()
                .WithMessage("O nome do servidor deve ser informado.");
        }
    }
}
