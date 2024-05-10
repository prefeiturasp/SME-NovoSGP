using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class LimparConsolidacaoFrequenciaTurmasPorAnoCommand : IRequest<bool>
    {
        public LimparConsolidacaoFrequenciaTurmasPorAnoCommand(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }

    public class LimparConsolidacaoFrequenciaTurmasPorAnoCommandValidator : AbstractValidator<LimparConsolidacaoFrequenciaTurmasPorAnoCommand>
    {
        public LimparConsolidacaoFrequenciaTurmasPorAnoCommandValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para limpar a consolidação de frequências das turmas");
        }
    }
}
