using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterComponentesCurricularesDoProfessorCJNaTurmaQuery : IRequest<IEnumerable<AtribuicaoCJ>>
    {
        public ObterComponentesCurricularesDoProfessorCJNaTurmaQuery(string login)
        {
            Login = login;
        }

        public string Login { get; }
    }

    public class ObterComponentesCurricularesDoProfessorCJNaTurmaQueryValidator : AbstractValidator<ObterComponentesCurricularesDoProfessorCJNaTurmaQuery>
    {
        public ObterComponentesCurricularesDoProfessorCJNaTurmaQueryValidator()
        {
            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login do usuário deve ser informado.");
        }
    }
}
