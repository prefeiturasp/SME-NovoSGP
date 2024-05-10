using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirDevolutivaCommand : IRequest<bool>
    {
        public ExcluirDevolutivaCommand(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
    }

    public class ExcluirDevolutivaCommandValidator : AbstractValidator<ExcluirDevolutivaCommand>
    {
        public ExcluirDevolutivaCommandValidator()
        {
            RuleFor(a => a.DevolutivaId)
                .NotEmpty()
                .WithMessage("O id da devolutiva deve ser informado para exclusão");
        }
    }
}
