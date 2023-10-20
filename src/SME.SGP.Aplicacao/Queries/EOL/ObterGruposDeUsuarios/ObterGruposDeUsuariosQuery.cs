using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterGruposDeUsuariosQuery : IRequest<IEnumerable<GruposDeUsuariosDto>>
    {
        public ObterGruposDeUsuariosQuery(int tipoPerfil)
        {
            TipoPerfil = tipoPerfil;
        }

        public int TipoPerfil { get; set; }
    }

    public class ObterGruposDeUsuariosQueryValidator : AbstractValidator<ObterGruposDeUsuariosQuery>
    {
        public ObterGruposDeUsuariosQueryValidator()
        {
            RuleFor(c => c.TipoPerfil)
                .NotEmpty()
                .WithMessage("O tipo perfil deve ser informado para busca dos grupos de usuários.");
        }
    }
}
