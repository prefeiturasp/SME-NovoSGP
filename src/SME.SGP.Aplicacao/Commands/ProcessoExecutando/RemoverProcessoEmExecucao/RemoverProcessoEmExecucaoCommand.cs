using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class RemoverProcessoEmExecucaoCommand: IRequest<bool>
    {
        public RemoverProcessoEmExecucaoCommand(ProcessoExecutando processoExecutando)
        {
            ProcessoExecutando = processoExecutando;
        }

        public ProcessoExecutando ProcessoExecutando { get; set; }
    }

    public class RemoverProcessoEmExecucaoCommandValidator: AbstractValidator<RemoverProcessoEmExecucaoCommand>
    {
        public RemoverProcessoEmExecucaoCommandValidator()
        {
            RuleFor(a => a.ProcessoExecutando)
                .NotNull()
                .WithMessage("Necessário informar o processo a ser removido de execução.");
        }
    }
}
