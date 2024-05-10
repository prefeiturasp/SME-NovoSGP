using FluentValidation;
using MediatR;

namespace SME.SGP.Aplicacao
{
    public class ObterNovosTiposUEPorAnoQuery : IRequest<string>
    {
        public ObterNovosTiposUEPorAnoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class ObterNovosTiposUEPorAnoQueryValidator : AbstractValidator<ObterNovosTiposUEPorAnoQuery>
    {
        public ObterNovosTiposUEPorAnoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O Ano letivo deve ser informado para consulta dos novos tipos de UE");
        }
    }
}
