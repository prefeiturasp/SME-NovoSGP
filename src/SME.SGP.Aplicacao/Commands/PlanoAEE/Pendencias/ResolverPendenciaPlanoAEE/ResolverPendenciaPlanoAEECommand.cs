using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ResolverPendenciaPlanoAEECommand : IRequest<bool>
    {
        public ResolverPendenciaPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class ResolverPendenciaPlanoAEECommandValidator : AbstractValidator<ResolverPendenciaPlanoAEECommand>
    {
        public ResolverPendenciaPlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para resolver as pendências");
        }
    }
}
