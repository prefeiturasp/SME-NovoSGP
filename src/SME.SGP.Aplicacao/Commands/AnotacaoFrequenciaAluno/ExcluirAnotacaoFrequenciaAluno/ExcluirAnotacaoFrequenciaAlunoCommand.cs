using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ExcluirAnotacaoFrequenciaAlunoCommand : IRequest<bool>
    {
        public ExcluirAnotacaoFrequenciaAlunoCommand(AnotacaoFrequenciaAluno anotacaoFrequenciaAluno)
        {
            AnotacaoFrequenciaAluno = anotacaoFrequenciaAluno;
        }

        public AnotacaoFrequenciaAluno AnotacaoFrequenciaAluno { get; set; }
    }

    public class ExcluirAnotacaoFrequenciaAlunoCommandValidator : AbstractValidator<ExcluirAnotacaoFrequenciaAlunoCommand>
    {
        public ExcluirAnotacaoFrequenciaAlunoCommandValidator()
        {
            RuleFor(c => c.AnotacaoFrequenciaAluno)
                .NotEmpty()
                .WithMessage("A anotação deve ser informada para exclusão");

        }
    }
}
