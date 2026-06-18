using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarFotoEstudanteCommandValidator : AbstractValidator<SalvarFotoEstudanteCommand>
    {
        public SalvarFotoEstudanteCommandValidator()
        {
            RuleFor(a => a.File)
                .NotEmpty()
                .WithMessage("A imagem do estudante deve ser enviada");
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do estudante deve ser enviado");
        }
    }
}
