using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAtendimentoNAAPACommand : IRequest<bool>
    {
        public ExcluirAtendimentoNAAPACommand(long encaminhamentoNAAPAId)
        {
            EncaminhamentoNAAPAId = encaminhamentoNAAPAId;
        }

        public long EncaminhamentoNAAPAId { get; }
    }

    public class ExcluirAtendimentoNAAPACommandValidator : AbstractValidator<ExcluirAtendimentoNAAPACommand>
    {
        public ExcluirAtendimentoNAAPACommandValidator()
        {

            RuleFor(c => c.EncaminhamentoNAAPAId)
            .NotEmpty()
            .WithMessage("O id do atendimento NAAPA deve ser informado para exclusão.");

        }
    }
}
