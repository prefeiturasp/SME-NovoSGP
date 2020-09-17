using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirNotificacaoDiarioBordoCommand : IRequest<bool>
    {
        public ExcluirNotificacaoDiarioBordoCommand(long observacaoId)
        {
            ObservacaoId = observacaoId;
        }

        public long ObservacaoId { get; set; }
    }

    public class ExcluirNotificacaoDiarioBordoCommandValidator : AbstractValidator<ExcluirNotificacaoDiarioBordoCommand>
    {
        public ExcluirNotificacaoDiarioBordoCommandValidator()
        {
            RuleFor(c => c.ObservacaoId)
                .NotEmpty()
                .WithMessage("O id da observação deve ser informado.");
        }
    }
}
