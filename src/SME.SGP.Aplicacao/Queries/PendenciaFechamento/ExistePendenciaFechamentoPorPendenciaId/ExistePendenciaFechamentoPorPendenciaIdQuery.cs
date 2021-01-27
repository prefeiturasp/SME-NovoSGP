using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExistePendenciaFechamentoPorPendenciaIdQuery : IRequest<bool>
    {
        public ExistePendenciaFechamentoPorPendenciaIdQuery(long pendenciaId)
        {
            PendenciaId = pendenciaId;
        }

        public long PendenciaId { get; set; }
    }

    public class ExistePendenciaFechamentoPorPendenciaIdQueryValidator : AbstractValidator<ExistePendenciaFechamentoPorPendenciaIdQuery>
    {
        public ExistePendenciaFechamentoPorPendenciaIdQueryValidator()
        {
            RuleFor(c => c.PendenciaId)
               .NotEmpty()
               .WithMessage("O id da pendência deve ser informado para verificar a existência de pendência de fechamento.");
        }
    }
}
