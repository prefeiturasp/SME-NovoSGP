using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoDevolutivasCommand : IRequest<bool>
    {
        public LimparConsolidacaoDevolutivasCommand(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }

    public class LimparConsolidacaoDevolutivasCommandValidator : AbstractValidator<LimparConsolidacaoDevolutivasCommand>
    {
        public LimparConsolidacaoDevolutivasCommandValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para limpar a consolidação de devolutivas das turmas");
        }
    }
}
