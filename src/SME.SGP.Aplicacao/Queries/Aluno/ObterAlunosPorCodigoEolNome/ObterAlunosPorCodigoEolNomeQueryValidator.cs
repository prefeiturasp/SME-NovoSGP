using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorCodigoEolNomeQueryValidator : AbstractValidator<ObterAlunosPorCodigoEolNomeQuery>
    {
        public ObterAlunosPorCodigoEolNomeQueryValidator()
        {
            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
            RuleFor(c => c.CodigoUe)
                .NotEmpty()
                .WithMessage("O código da ue deve ser informado.");
            RuleFor(c => c.Nome)
                .NotEmpty()
                .WithMessage("O Nome do estudante deve ser informado.");
            RuleFor(c => c.CodigoEOL)
                .NotEmpty()
                .WithMessage("O Código do estudante deve ser informado.");
        }
    }
}
