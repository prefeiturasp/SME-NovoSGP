using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class AlterarAnotacaoFrequenciaAlunoCommand : IRequest<bool>
    {
        public AlterarAnotacaoFrequenciaAlunoCommand(AnotacaoFrequenciaAluno anotacao)
        {
            Anotacao = anotacao;
        }

        public AnotacaoFrequenciaAluno Anotacao { get; set; }
    }

    public class AlterarAnotacaoFrequenciaAlunoCommandValidator : AbstractValidator<AlterarAnotacaoFrequenciaAlunoCommand>
    {
        public AlterarAnotacaoFrequenciaAlunoCommandValidator()
        {
            RuleFor(c => c.Anotacao)
            .NotEmpty() 
            .WithMessage("A anotação deve ser informada para atualização");

        }
    }
}
