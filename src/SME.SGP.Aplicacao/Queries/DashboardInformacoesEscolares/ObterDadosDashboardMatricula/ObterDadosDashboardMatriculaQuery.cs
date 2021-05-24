using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDadosDashboardMatriculaQuery : IRequest<IEnumerable<GraficoBaseDto>>
    {
        public ObterDadosDashboardMatriculaQuery(int anoLetivo, long dreId, long ueId, AnoItinerarioPrograma[] anos, Modalidade modalidade, int? semestre)
        {
            AnoLetivo = anoLetivo;
            DreId = dreId;
            UeId = ueId;
            Anos = anos;
            Modalidade = modalidade;
            Semestre = semestre;
        }

        public int AnoLetivo { get; set; }
        public long DreId { get; set; }
        public long UeId { get; set; }
        public AnoItinerarioPrograma[] Anos { get; set; }
        public Modalidade Modalidade { get; set; }
        public int? Semestre { get; set; }
    }

    public class ObterDadosDashboardMatriculaQueryValidator : AbstractValidator<ObterDadosDashboardMatriculaQuery>
    {
        public ObterDadosDashboardMatriculaQueryValidator()
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
