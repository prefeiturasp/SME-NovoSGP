using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Enumerados;
using SME.SGP.Infra;

namespace SME.SGP.Aplicacao
{
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand : IRequest<long>
    {
        public long RespostaId { get; set; }
        public long QuestaoId { get; set; }

        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand(long respostaId, long questaoId)
        {
            RespostaId = respostaId;
            QuestaoId = questaoId;
        }
    }
    public class RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandValidator : AbstractValidator<RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommand>
    {
        public RegistrarEncaminhamentoAEESecaoQuestaoRespostaCommandValidator()
        {
            RuleFor(x => x.RespostaId)
                   .GreaterThan(0)
                    .WithMessage("O Id da Resposta do Encaminhamento deve ser informada!");
            RuleFor(x => x.QuestaoId)
                   .GreaterThan(0)
                   .WithMessage("O Id da Questão do Encaminhamento deve ser informada!");
        }
    }
}
