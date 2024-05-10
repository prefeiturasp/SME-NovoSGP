using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterFotoAlunoPorAlunoCodigoQueryValidator : AbstractValidator<ObterFotoAlunoPorAlunoCodigoQuery>
    {
        public ObterFotoAlunoPorAlunoCodigoQueryValidator()
        {
            RuleFor(a => a.CodigoAluno)
                .NotEmpty()
                .WithMessage("O código do aluno é nescessário para obter a foto do aluno");
        }
    }
}
