using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarAtendimentoNAAPASecaoQuestaoCommand : IRequest<long>
    {
        public long SecaoId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarAtendimentoNAAPASecaoQuestaoCommand(long secaoId, long questaoId)
        {
            SecaoId = secaoId;
            QuestaoId = questaoId;
        }
    }
    public class RegistrarAtendimentoNAAPASecaoQuestaoCommandValidator : AbstractValidator<RegistrarAtendimentoNAAPASecaoQuestaoCommand>
    {
        public RegistrarAtendimentoNAAPASecaoQuestaoCommandValidator()
        {
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Seção do Atendimento deve ser informada para registrar atendimento naapa!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Atendimento deve ser informada para registrar atendimento naapa!");
        }
    }
}
