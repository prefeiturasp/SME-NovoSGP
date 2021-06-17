using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoFrequenciaTurmaPorAnoQuery : IRequest<bool>
    {
        public ExisteConsolidacaoFrequenciaTurmaPorAnoQuery(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }

    public class ExisteConsolidacaoFrequenciaTurmaPorAnoQueryValidator : AbstractValidator<ExisteConsolidacaoFrequenciaTurmaPorAnoQuery>
    {
        public ExisteConsolidacaoFrequenciaTurmaPorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para verificar a existência de consolidação de frequência das turmas");
        }
    }
}
