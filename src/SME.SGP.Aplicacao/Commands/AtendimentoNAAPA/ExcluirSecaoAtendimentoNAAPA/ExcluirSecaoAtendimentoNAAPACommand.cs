using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirSecaoAtendimentoNAAPACommand : IRequest<bool>
    {
        public ExcluirSecaoAtendimentoNAAPACommand(long encaminhamentoNAAPASecaoId)
        {
            EncaminhamentoNAAPASecaoId = encaminhamentoNAAPASecaoId;
        }

        public long EncaminhamentoNAAPASecaoId { get; }
    }

    public class ExcluirSecaoAtendimentoNAAPACommandValidator : AbstractValidator<ExcluirSecaoAtendimentoNAAPACommand>
    {
        public ExcluirSecaoAtendimentoNAAPACommandValidator()
        {
            RuleFor(c => c.EncaminhamentoNAAPASecaoId)
            .NotEmpty()
            .WithMessage("O id da seção do atendimento NAAPA deve ser informado para exclusão.");

        }
    }
}
