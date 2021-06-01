using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand : IRequest<bool>
    {
        public ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand(long registroFrequenciaId)
        {
            RegistroFrequenciaId = registroFrequenciaId;
        }

        public long RegistroFrequenciaId { get; set; }
    }

    public class ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommandValidator : AbstractValidator<ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommand>
    {
        public ExcluirFrequenciasAlunoPorRegistroFrequenciaIdCommandValidator()
        {
            RuleFor(a => a.RegistroFrequenciaId)
                .NotEmpty()
                .WithMessage("O Id do registro de frequência deve ser infomado para limpar as frequencias dos alunos");
        }
    }
}
