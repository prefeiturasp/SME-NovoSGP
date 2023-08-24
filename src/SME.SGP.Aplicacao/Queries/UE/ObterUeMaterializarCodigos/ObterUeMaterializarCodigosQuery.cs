using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao 
{
    public class ObterUeMaterializarCodigosQuery : IRequest<(List<Ue> Ues, string[] CodigosUesNaoEncontradas)>
    {
        public string[] IdUes { get; set; }

        public ObterUeMaterializarCodigosQuery(string[] idUes)
        {
            IdUes = idUes;
        }
    }

    public class ObterUeMaterializarCodigosQueryValidator : AbstractValidator<ObterUeMaterializarCodigosQuery>
    {
        public ObterUeMaterializarCodigosQueryValidator()
        {
            RuleFor(x => x.IdUes)
                .NotEmpty()
                .WithMessage("Íds das Ues devem ser informadas.");
        }
    }
}
