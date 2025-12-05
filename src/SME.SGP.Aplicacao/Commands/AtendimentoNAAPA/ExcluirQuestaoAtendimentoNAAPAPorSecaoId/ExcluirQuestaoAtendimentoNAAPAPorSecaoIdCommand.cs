using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommand : IRequest<bool>
    {
        public ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommand(long encaminhamentoNAAPASecaoId)
        {
            EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId;
        }

        public long EncaminhamentoNAAPASecaoId { get; }
    }

    public class ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommandValidator : AbstractValidator<ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommand>
    {
        public ExcluirQuestaoAtendimentoNAAPAPorSecaoIdCommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do atendimento NAAPA deve ser informado para exclusão de suas questões.");
        }
    }
}
