using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorDreQuery : IRequest<IEnumerable<GraficoFrequenciaGlobalPorDREDto>>
    {
        public int AnoLetivo { get; set; }

        public ObterDadosDashboardFrequenciaPorDreQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
    }

    public class ObterDadosDashboardFrequenciaPorDreQueryValidator : AbstractValidator<ObterDadosDashboardFrequenciaPorDreQuery>
    {
        public ObterDadosDashboardFrequenciaPorDreQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para a busca de frequência global por DRE.");
        }
    }
}