using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalRIsPorDreQuery : IRequest<IEnumerable<GraficoBaseDto>>
    {
        public ObterTotalRIsPorDreQuery(int anoLetivo, string ano)
        {
            AnoLetivo = anoLetivo;
            Ano = ano;
        }

        public int AnoLetivo { get; }
        public string Ano { get; }
    }

    public class ObterTotalRIsPorDreQueryValidator : AbstractValidator<ObterTotalRIsPorDreQuery>
    {
        public ObterTotalRIsPorDreQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de total de Registros Individual por DRE");
        }
    }
}
