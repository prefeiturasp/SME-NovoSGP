using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoCommand : IRequest<long>
    {
        public long SecaoId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarEncaminhamentoAEESecaoQuestaoCommand(long secaoId, long questaoId)
        {
            SecaoId = secaoId;
            QuestaoId = questaoId;
        }
    }
    public class RegistrarEncaminhamentoAEESecaoQuestaoCommandValidator : AbstractValidator<RegistrarEncaminhamentoAEESecaoQuestaoCommand>
    {
        public RegistrarEncaminhamentoAEESecaoQuestaoCommandValidator()
        {
            RuleFor(x => x.SecaoId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Seção do Encaminhamento deve ser informada!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento deve ser informada!");
        }
    }
}
