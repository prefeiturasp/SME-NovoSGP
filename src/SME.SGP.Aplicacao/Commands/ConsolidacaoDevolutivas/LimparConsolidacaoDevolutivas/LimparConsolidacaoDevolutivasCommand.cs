using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoDevolutivasCommand : IRequest<bool>
    {
        public LimparConsolidacaoDevolutivasCommand(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class LimparConsolidacaoDevolutivasCommandValidator : AbstractValidator<LimparConsolidacaoDevolutivasCommand>
    {
        public LimparConsolidacaoDevolutivasCommandValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para limpar a consolidação de devolutivas das turmas");
        }
    }
}
