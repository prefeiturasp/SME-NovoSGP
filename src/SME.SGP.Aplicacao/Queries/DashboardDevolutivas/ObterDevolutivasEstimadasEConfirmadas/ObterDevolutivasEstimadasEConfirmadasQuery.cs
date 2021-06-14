using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDevolutivasEstimadasEConfirmadasQuery : IRequest<IEnumerable<GraficoDevolutivasEstimadasEConfirmadasDto>>
    {
        public int AnoLetivo { get; set; }
        public Modalidade Modalidade { get; set; }
        public long? DreId { get; set; }
        public long? UeId { get; set; }

        public ObterDevolutivasEstimadasEConfirmadasQuery(int anoLetivo, Modalidade modalidade, long? dreId, long? ueId)
        {
            AnoLetivo = anoLetivo;
            Modalidade = modalidade;
            DreId = dreId;
            UeId = ueId;
        }
    }

    public class ObterDevolutivasEstimadasEConfirmadasQueryValidator : AbstractValidator<ObterDevolutivasEstimadasEConfirmadasQuery>
    {
        public ObterDevolutivasEstimadasEConfirmadasQueryValidator()
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