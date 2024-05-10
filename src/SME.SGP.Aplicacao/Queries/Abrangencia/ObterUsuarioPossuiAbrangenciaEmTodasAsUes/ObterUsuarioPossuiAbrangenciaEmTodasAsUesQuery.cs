using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery : IRequest<bool>
    {
        public ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery(Guid perfil)
        {
            Perfil = perfil;
        }
        public Guid Perfil { get; }

    }


    public class ObterUsuarioPossuiAbrangenciaEmTodasAsUesQueryValidator : AbstractValidator<ObterUsuarioPossuiAbrangenciaEmTodasAsUesQuery>
    {
        public ObterUsuarioPossuiAbrangenciaEmTodasAsUesQueryValidator()
        {
            RuleFor(c => c.Perfil)
            .NotEmpty()
            .WithMessage("O perfil do usuário deve ser informado.");
        }
    }

}
