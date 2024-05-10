using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardFrequenciaPorAnoQuery : IRequest<IEnumerable<GraficoFrequenciaGlobalPorAnoDto>>
    {
        public ObterDadosDashboardFrequenciaPorAnoQuery(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semestre { get; set; }
    }

    public class ObterDadosDashboardFrequenciaPorAnoQueryValidator : AbstractValidator<ObterDadosDashboardFrequenciaPorAnoQuery>
    {
        public ObterDadosDashboardFrequenciaPorAnoQueryValidator()
        {

            RuleFor(c => c.AnoLetivo)
                .NotNull()
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
            RuleFor(c => c.Modalidade)
                .NotNull()
                .NotEmpty()
                .WithMessage("A modalidade deve ser informada.");
        }
    }
}
