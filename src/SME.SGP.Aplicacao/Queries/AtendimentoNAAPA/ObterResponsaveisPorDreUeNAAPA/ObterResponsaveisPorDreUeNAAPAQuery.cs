using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterResponsaveisPorDreUeNAAPAQuery : IRequest<IEnumerable<FuncionarioUnidadeDto>>
    {
        public ObterResponsaveisPorDreUeNAAPAQuery(string codigoDre, string codigoUe)
        {
            CodigoDre = codigoDre;
            CodigoUe = codigoUe;
        }

        public string CodigoDre { get; set; } 
        public string CodigoUe { get; set; }
    }

    public class ObterResponsaveisPorDreUeNAAPAQueryValidator : AbstractValidator<ObterResponsaveisPorDreUeNAAPAQuery>
    {
        public ObterResponsaveisPorDreUeNAAPAQueryValidator()
        {
            RuleFor(c => c.CodigoDre)
                .NotEmpty().When(x => string.IsNullOrEmpty(x.CodigoUe))
                .WithMessage("O código da dre deve ser informado para a obter os responsáveis naapa");

            RuleFor(c => c.CodigoUe)
                .NotEmpty().When(x => string.IsNullOrEmpty(x.CodigoDre))
                .WithMessage("O código da ue deve ser informado para a obter os responsáveis naapa");
        }
    }
}
