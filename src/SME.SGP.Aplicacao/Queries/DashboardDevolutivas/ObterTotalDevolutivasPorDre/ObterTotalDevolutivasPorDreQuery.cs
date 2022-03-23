using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterTotalDevolutivasPorDreQuery : IRequest<IEnumerable<GraficoBaseDto>>
    {
        public ObterTotalDevolutivasPorDreQuery(int anoLetivo, string ano)
        {
            AnoLetivo = anoLetivo;
            Ano = ano;
        }

        public int AnoLetivo { get; }
        public string Ano { get; }
    }

    public class ObterTotalDevolutivasPorDreQueryValidator : AbstractValidator<ObterTotalDevolutivasPorDreQuery>
    {
        public ObterTotalDevolutivasPorDreQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta de devolutivas por DRE");
        }
    }
}
