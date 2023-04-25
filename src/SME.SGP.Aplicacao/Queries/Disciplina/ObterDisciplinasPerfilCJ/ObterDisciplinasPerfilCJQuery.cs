using FluentValidation;
using MediatR;
using SME.SGP.Aplicacao.Integracoes.Respostas;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDisciplinasPerfilCJQuery : IRequest<IEnumerable<DisciplinaResposta>>
    {
        public ObterDisciplinasPerfilCJQuery(string codigoTurma, string login)
        {
            CodigoTurma = codigoTurma;
            Login = login;
        }

        public string CodigoTurma { get; }
        public string Login { get; }
    }

    public class ObterDisciplinasPerfilCJQueryValidator : AbstractValidator<ObterDisciplinasPerfilCJQuery>
    {
        public ObterDisciplinasPerfilCJQueryValidator()
        {
            RuleFor(c => c.CodigoTurma)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado para obter as disciplinas por perfil CJ.");

            RuleFor(c => c.Login)
                .NotEmpty()
                .WithMessage("O login deve ser informado para obter as disciplinas por perfil CJ.");
        }
    }
}
