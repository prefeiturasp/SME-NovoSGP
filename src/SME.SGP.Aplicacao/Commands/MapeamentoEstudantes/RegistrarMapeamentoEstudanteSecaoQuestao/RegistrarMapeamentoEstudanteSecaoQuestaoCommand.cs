using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarMapeamentoEstudanteSecaoQuestaoCommand : IRequest<long>
    {
        public long SecaoId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarMapeamentoEstudanteSecaoQuestaoCommand(long secaoId, long questaoId)
        {
            SecaoId = secaoId;
            QuestaoId = questaoId;
        }
    }
    public class RegistrarMapeamentoEstudanteSecaoQuestaoCommandValidator : AbstractValidator<RegistrarMapeamentoEstudanteSecaoQuestaoCommand>
    {
        public RegistrarMapeamentoEstudanteSecaoQuestaoCommandValidator()
        {
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Seção do Mapeamento de Estudante deve ser informada para registrar a questão!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Mapeamento de Estudante deve ser informada para registrar a questão!");
        }
    }
}
