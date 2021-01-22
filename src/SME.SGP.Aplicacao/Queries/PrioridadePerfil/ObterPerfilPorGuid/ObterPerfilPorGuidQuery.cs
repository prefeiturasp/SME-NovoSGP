using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterPerfilPorGuidQuery : IRequest<PrioridadePerfil>
    {
        public ObterPerfilPorGuidQuery(Guid perfil)
        {
            Perfil = perfil;
        }

        public Guid Perfil { get; set; }
    }

    public class ObterPerfilPorGuidQueryValidator : AbstractValidator<ObterPerfilPorGuidQuery>
    {
        public ObterPerfilPorGuidQueryValidator()
        {
            RuleFor(c => c.Perfil)
               .Must(a => a != Guid.Empty)
               .WithMessage("O perfil deve ser informado para consulta de seus dados.");

        }
    }
}
