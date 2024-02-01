using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class RegistrarRegistroAcaoSecaoQuestaoCommand : IRequest<long>
    {
        public long SecaoId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarRegistroAcaoSecaoQuestaoCommand(long secaoId, long questaoId)
        {
            SecaoId = secaoId;
            QuestaoId = questaoId;
        }
    }
    public class RegistrarRegistroAcaoSecaoQuestaoCommandValidator : AbstractValidator<RegistrarRegistroAcaoSecaoQuestaoCommand>
    {
        public RegistrarRegistroAcaoSecaoQuestaoCommandValidator()
        {
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Seção do Registro de Ação deve ser informada para registrar o registro de ação!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Registro de Ação deve ser informada para registrar o registro de ação!");
        }
    }
}
