using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoDevolutivasCommand : IRequest<bool>
    {
        public LimparConsolidacaoDevolutivasCommand(long[] turmasIds)
        {
            TurmasIds = turmasIds;
        }

        public long[] TurmasIds { get; }
    }

    public class LimparConsolidacaoDevolutivasCommandValidator : AbstractValidator<LimparConsolidacaoDevolutivasCommand>
    {
        public LimparConsolidacaoDevolutivasCommandValidator()
        {
            RuleFor(a => a.TurmasIds)
                .NotEmpty()
                .WithMessage("A turma deve ser informada para limpar a consolidação de devolutivas");
        }
    }
}
