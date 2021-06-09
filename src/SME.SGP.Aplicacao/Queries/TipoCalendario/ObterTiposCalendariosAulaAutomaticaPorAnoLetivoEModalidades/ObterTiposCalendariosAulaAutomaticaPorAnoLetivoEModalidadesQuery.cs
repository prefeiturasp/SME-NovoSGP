using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQuery : IRequest<IEnumerable<TipoCalendarioAulasAutomaticasDto>>
    {
        public ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQuery(int anoLetivo, Modalidade[] modalidades)
        {
            AnoLetivo = anoLetivo;
            Modalidades = modalidades;
        }

        public int AnoLetivo { get; set; }
        public Modalidade[] Modalidades { get; set; }
    }

    public class ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQueryValidator : AbstractValidator<ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQuery>
    {
        public ObterTiposCalendariosAulaAutomaticaPorAnoLetivoEModalidadesQueryValidator()
        {
            RuleFor(c => c.Modalidades)
                .NotNull()
                .NotEmpty()
                .WithMessage("As modalidades devem ser informadas.");

            RuleFor(c => c.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");
        }
    }
}
