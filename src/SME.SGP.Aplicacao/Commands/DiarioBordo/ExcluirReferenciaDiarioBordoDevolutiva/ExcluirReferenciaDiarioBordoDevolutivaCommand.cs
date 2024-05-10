using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirReferenciaDiarioBordoDevolutivaCommand : IRequest<bool>
    {
        public ExcluirReferenciaDiarioBordoDevolutivaCommand(long devolutivaId)
        {
            DevolutivaId = devolutivaId;
        }

        public long DevolutivaId { get; set; }
    }

    public class ExcluirReferenciaDiarioBordoDevolutivaCommandValidator : AbstractValidator<ExcluirReferenciaDiarioBordoDevolutivaCommand>
    {
        public ExcluirReferenciaDiarioBordoDevolutivaCommandValidator()
        {
            RuleFor(a => a.DevolutivaId)
                .NotEmpty()
                .WithMessage("Necessário informar o Id da devolutiva para exclusão");
        }
    }
}
