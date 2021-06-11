using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirPlanoAEEObservacaoCommand : IRequest<bool>
    {
        public ExcluirPlanoAEEObservacaoCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }

    public class ExcluirPlanoAEEObservacaoCommandValidator : AbstractValidator<ExcluirPlanoAEEObservacaoCommand>
    {
        public ExcluirPlanoAEEObservacaoCommandValidator()
        {
            RuleFor(a => a.Id)
                .NotEmpty()
                .WithMessage("O id da observação deve ser informada para exclusão");
        }
    }
}
