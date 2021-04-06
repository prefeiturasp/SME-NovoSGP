using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class TurmaPossuiComponenteCurricularPAPQuery : IRequest<bool>
    {
        public TurmaPossuiComponenteCurricularPAPQuery(string turmaCodigo, string login, Guid perfil)
        {
            TurmaCodigo = turmaCodigo;
            Login = login;
            Perfil = perfil;
        }

        public string TurmaCodigo { get; set; }

        public string Login { get; set; }

        public Guid Perfil { get; set; }
    }

    public class TurmaPossuiComponenteCurricularPAPValidator : AbstractValidator<TurmaPossuiComponenteCurricularPAPQuery>
    {
        public TurmaPossuiComponenteCurricularPAPValidator()
        {
            RuleFor(c => c.TurmaCodigo)
                .NotEmpty()
                .WithMessage("O código da turma deve ser informado.");

            RuleFor(c => c.Login)
              .NotEmpty()
              .WithMessage("O login do usuário logado deve ser informado.");

            RuleFor(c => c.Perfil)
              .NotEmpty()
              .WithMessage("O perfil do usuário logado deve ser informado.");
        }
    }
}
