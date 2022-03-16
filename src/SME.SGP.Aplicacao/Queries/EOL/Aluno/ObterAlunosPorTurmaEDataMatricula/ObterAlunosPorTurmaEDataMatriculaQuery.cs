using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAlunosPorTurmaEDataMatriculaQuery : IRequest<IEnumerable<AlunoPorTurmaResposta>>
    {
        public string CodigoTurma { get; set; }
        public DateTime DataMatricula { get; set; }

        public ObterAlunosPorTurmaEDataMatriculaQuery(string codigoTurma, DateTime dataMatricula)
        {
            CodigoTurma = codigoTurma;
            DataMatricula = dataMatricula;
        }
    }

    public class ObterAlunosPorTurmaEDataMatriculaQueryValidator: AbstractValidator<ObterAlunosPorTurmaEDataMatriculaQuery>
    {
        public ObterAlunosPorTurmaEDataMatriculaQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.DataMatricula)
                .NotEmpty()
                .WithMessage("A data para referenciar a matrícula deve ser informada.");
        }
    }
}
