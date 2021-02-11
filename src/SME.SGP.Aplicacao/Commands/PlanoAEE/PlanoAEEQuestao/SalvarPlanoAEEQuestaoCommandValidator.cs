using FluentValidation;

namespace SME.SGP.Aplicacao
{
    public class SalvarPlanoAEEQuestaoCommandValidator : AbstractValidator<SalvarPlanoAEEQuestaoCommand>
    {
        public SalvarPlanoAEEQuestaoCommandValidator()
        {
            RuleFor(x => x.PlanoId)
                   .GreaterThan(0)
                    .WithMessage("O Id do Plano do deve ser informado!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Plano deve ser informado!");
        }
    }
}
