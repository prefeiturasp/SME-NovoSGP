using FluentValidation;
using MediatR;
using System;

namespace SME.SGP.Aplicacao
{
    public class ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery : IRequest<bool>
    {
        public ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery(Guid perfil)
        {
            Perfil = perfil;
        }
        public Guid Perfil { get; }
    }


    public class ObterUsuarioPossuiAbrangenciaEmTodasAsDresQueryValidator : AbstractValidator<ObterUsuarioPossuiAbrangenciaEmTodasAsDresQuery>
    {
        public ObterUsuarioPossuiAbrangenciaEmTodasAsDresQueryValidator()
        {
            RuleFor(c => c.Perfil)
            .NotEmpty()
            .WithMessage("O perfil do usuário deve ser informado.");
        }
    }

}
