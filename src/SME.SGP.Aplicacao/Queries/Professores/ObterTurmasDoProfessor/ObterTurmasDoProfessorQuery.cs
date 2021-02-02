using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterTurmasDoProfessorQuery : IRequest<IEnumerable<ProfessorTurmaReposta>>
    {
        public ObterTurmasDoProfessorQuery(string professorRf)
        {
            ProfessorRf = professorRf;
        }

        public string ProfessorRf { get; }
    }

    public class ObterTurmasDoProfessorQueryValidator : AbstractValidator<ObterTurmasDoProfessorQuery>
    {
        public ObterTurmasDoProfessorQueryValidator()
        {
            RuleFor(a => a.ProfessorRf)
                .NotEmpty()
                .WithMessage("O RF do professor deve ser informado para consulta de suas turmas");
        }
    }
}
