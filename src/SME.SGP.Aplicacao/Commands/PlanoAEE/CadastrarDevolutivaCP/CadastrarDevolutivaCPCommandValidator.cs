using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class CadastrarDevolutivaCPCommandValidator : AbstractValidator<CadastrarDevolutivaCPCommand>
    {
        public CadastrarDevolutivaCPCommandValidator()
        {
            RuleFor(x => x.PlanoAEEId)
                   .GreaterThan(0)
                   .WithMessage("O PlanoAEE deve ser informado!");

            RuleFor(x => x.ParecerCoordenacao)
                    .NotEmpty()
                    .WithMessage("O parecer da coordenação deve ser informado!");
        }
    }
}
