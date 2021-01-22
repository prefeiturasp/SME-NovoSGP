using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class RemoverProcessoEmExecucaoPorIdCommand : IRequest<bool>
    {
        public RemoverProcessoEmExecucaoPorIdCommand(long id)
        {
            Id = id;
        }

        public long Id { get; set; }
    }

    public class RemoverProcessoEmExecucaoPorIdCommandValidator : AbstractValidator<RemoverProcessoEmExecucaoPorIdCommand>
    {
        public RemoverProcessoEmExecucaoPorIdCommandValidator()
        {
            RuleFor(a => a.Id)
               .NotEmpty()
               .WithMessage("O id do processo deve ser informado para exclusão.");
        }
    }
}
