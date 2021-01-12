using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirEncaminhamentoAEECommand : IRequest<bool>
    {
        public ExcluirEncaminhamentoAEECommand(long encaminhamentoAeeId)
        {
            EncaminhamentoAeeId = encaminhamentoAeeId;
        }

        public long EncaminhamentoAeeId { get; }
    }

    public class ExcluirEncaminhamentoAEECommandValidator : AbstractValidator<ExcluirEncaminhamentoAEECommand>
    {
        public ExcluirEncaminhamentoAEECommandValidator()
        {

            RuleFor(c => c.EncaminhamentoAeeId)
            .NotEmpty()
            .WithMessage("O id do encaminhamento AEE deve ser informado para exclusão.");

        }
    }
}
