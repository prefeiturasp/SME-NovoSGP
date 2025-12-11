using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class AtualizarSituacaoAtendimentoNAAPACommand : IRequest<bool>
    {
        public AtualizarSituacaoAtendimentoNAAPACommand(long id)
        {
            Id = id;
        }
        
        public long Id { get; }
    }

    public class AtualizarSituacaoAtendimentoNAAPACommandValidator : AbstractValidator<AtualizarSituacaoAtendimentoNAAPACommand>
    {
        public AtualizarSituacaoAtendimentoNAAPACommandValidator()
        {
            RuleFor(c => c.Id)
                .GreaterThan(0)
                .WithMessage("O Id do atendimento NAAPA deve ser informado para a atualização.");
        }
    }
}