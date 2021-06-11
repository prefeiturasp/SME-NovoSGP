using FluentValidation;
using MediatR;
using SME.SGP.Infra;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQueryValidator : AbstractValidator<ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQuery>
    {
        public ObterMotivoPorTurmaAlunoComponenteCurricularBimestreQueryValidator()
        {
            RuleFor(a => a.AlunoCodigo)
                .NotEmpty()
                .WithMessage("O Código do aluno deve ser informado.");
            RuleFor(a => a.TurmaId)
                .NotEmpty()
                .WithMessage("O id da Turma deve ser informado.");
        }
    }
}
