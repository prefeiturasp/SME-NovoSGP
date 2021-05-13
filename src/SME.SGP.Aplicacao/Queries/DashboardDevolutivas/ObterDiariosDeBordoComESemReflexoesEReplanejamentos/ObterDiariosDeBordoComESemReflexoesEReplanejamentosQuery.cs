using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDiariosDeBordoComESemReflexoesEReplanejamentosQuery : IRequest<IEnumerable<GraficoDiariosDeBordoComESemReflexoesEReplanejamentosDto>>
    {
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public DateTime DataAula { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }

        public ObterDiariosDeBordoComESemReflexoesEReplanejamentosQuery(int anoLetivo, Modalidade modalidade, DateTime dataAula, long? dreId, long? ueId)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DataAula = dataAula;
            DreId = dreId;
            UeId = ueId;
        }
    }

    public class ObterDiariosDeBordoComESemReflexoesEReplanejamentosQueryValidator : AbstractValidator<ObterDiariosDeBordoComESemReflexoesEReplanejamentosQuery>
    {
        public ObterDiariosDeBordoComESemReflexoesEReplanejamentosQueryValidator()
        {
            RuleFor(x => x.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado.");

            RuleFor(x => x.DreId)
                .GreaterThan(0)
                .When(x => x.DreId.HasValue)
                .WithMessage("O código da DRE informado é inválido.");

            RuleFor(x => x.UeId)
                .GreaterThan(0)
                .When(x => x.UeId.HasValue)
                .WithMessage("O código da UE informado é inválido.");
        }
    }
}