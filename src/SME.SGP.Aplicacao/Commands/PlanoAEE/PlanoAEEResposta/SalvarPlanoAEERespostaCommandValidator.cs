using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEERespostaCommandValidator : AbstractValidator<SalvarPlanoAEERespostaCommand>
    {
        public SalvarPlanoAEERespostaCommandValidator()
        {
            RuleFor(x => x.PlanoAEEQuestaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Questão deve ser informado!");
            RuleFor(x => x.Resposta)
                   .NotEmpty()
                   .WithMessage("A Resposta do Plano deve ser informada!");
            RuleFor(x => x.TipoQuestao)
                   .NotEmpty()
                   .WithMessage("O Tipo da Questão deve ser informado!");
        }
    }
}
