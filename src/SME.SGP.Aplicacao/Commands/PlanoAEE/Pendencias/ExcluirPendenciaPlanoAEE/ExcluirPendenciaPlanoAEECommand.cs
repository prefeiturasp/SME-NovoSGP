using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaPlanoAEECommand : IRequest<bool>
    {
        public ExcluirPendenciaPlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class ExcluirPendenciaValidadePlanoAEECommandValidator : AbstractValidator<ExcluirPendenciaPlanoAEECommand>
    {
        public ExcluirPendenciaValidadePlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para exclusão de suas pendências");
        }
    }
}
