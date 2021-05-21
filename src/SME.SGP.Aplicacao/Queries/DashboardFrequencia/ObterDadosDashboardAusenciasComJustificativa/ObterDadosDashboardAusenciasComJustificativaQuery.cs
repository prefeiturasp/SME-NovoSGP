using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardAusenciasComJustificativaQuery : IRequest<IEnumerable<GraficoAusenciasComJustificativaResultadoDto>>
    {
        public ObterDadosDashboardAusenciasComJustificativaQuery(int anoLetivo, long dreId, long ueId, Modalidade modalidade, int semestre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Modalidade = modalidade;
            Semstre = semestre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public Modalidade Modalidade { get; set; }
        public int Semstre { get; set; }
    }

    public class ObterDadosDashboardAusenciasComJustificativaQueryValidator : AbstractValidator<ObterDadosDashboardFrequenciaPorAnoQuery>
    {
        public ObterDadosDashboardAusenciasComJustificativaQueryValidator()
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
