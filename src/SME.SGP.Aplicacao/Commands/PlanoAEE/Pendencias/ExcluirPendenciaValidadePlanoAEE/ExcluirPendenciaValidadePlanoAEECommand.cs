using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPendenciaValidadePlanoAEECommand : IRequest<bool>
    {
        public ExcluirPendenciaValidadePlanoAEECommand(long planoAEEId)
        {
            PlanoAEEId = planoAEEId;
        }

        public long PlanoAEEId { get; }
    }

    public class ExcluirPendenciaValidadePlanoAEECommandValidator : AbstractValidator<ExcluirPendenciaValidadePlanoAEECommand>
    {
        public ExcluirPendenciaValidadePlanoAEECommandValidator()
        {
            RuleFor(a => a.PlanoAEEId)
                .NotEmpty()
                .WithMessage("O id do plano AEE deve ser informado para exclusão de suas pendências");
        }
    }
}
