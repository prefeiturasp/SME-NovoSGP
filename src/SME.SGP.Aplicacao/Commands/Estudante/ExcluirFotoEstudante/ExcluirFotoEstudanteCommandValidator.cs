using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFotoEstudanteCommandValidator : AbstractValidator<ExcluirFotoEstudanteCommand>
    {
        public ExcluirFotoEstudanteCommandValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do estudante deve ser enviado");
        }
    }
}
