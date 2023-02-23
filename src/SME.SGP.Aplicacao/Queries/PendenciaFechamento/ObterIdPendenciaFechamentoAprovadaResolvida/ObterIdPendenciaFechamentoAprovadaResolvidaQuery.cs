using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterIdPendenciaFechamentoAprovadaResolvidaQuery : IRequest<IEnumerable<long>>
    {
        public ObterIdPendenciaFechamentoAprovadaResolvidaQuery(long fechamentoId, TipoPendencia tipoPendencia)
        {
            FechamentoId = fechamentoId;
            TipoPendencia = tipoPendencia;
        }

        public long FechamentoId { get; set; }
        public TipoPendencia TipoPendencia { get; set; }
    }

    public class ObterIdPendenciaFechamentoAprovadaResolvidaQueryValidator : AbstractValidator<ObterIdPendenciaFechamentoAprovadaResolvidaQuery>
    {
        public ObterIdPendenciaFechamentoAprovadaResolvidaQueryValidator()
        {
            RuleFor(c => c.FechamentoId)
            .NotEmpty()
            .WithMessage("O id da fechamento deve ser informado.");

            RuleFor(c => c.TipoPendencia)
            .NotEmpty()
            .WithMessage("O tipo de pendência deve ser informado.");
        }
    }
}
