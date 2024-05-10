using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ExisteConsolidacaoDevolutivaTurmaPorAnoQuery : IRequest<bool>
    {
        public ExisteConsolidacaoDevolutivaTurmaPorAnoQuery(int ano)
        {
            Ano = ano;
        }

        public int Ano { get; }
    }

    public class ExisteConsolidacaoDevolutivaTurmaPorAnoQueryValidator : AbstractValidator<ExisteConsolidacaoDevolutivaTurmaPorAnoQuery>
    {
        public ExisteConsolidacaoDevolutivaTurmaPorAnoQueryValidator()
        {
            RuleFor(a => a.Ano)
                .NotEmpty()
                .WithMessage("O ano deve ser informado para verificar a existência de consolidação de devolutiva das turmas");
        }
    }
}
