using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirNotificacaoDiarioBordoCommand(long diarioBordoId)
        {
            DiarioBordoId = diarioBordoId;
        }

        public long DiarioBordoId { get; set; }
    }

    public class ExcluirNotificacaoDiarioBordoCommandValidator : AbstractValidator<ExcluirNotificacaoDiarioBordoCommand>
    {
        public ExcluirNotificacaoDiarioBordoCommandValidator()
        {
            RuleFor(c => c.DiarioBordoId)
                .NotEmpty()
                .WithMessage("O Diario de bordo deve ser informado.");
        }
    }
}
