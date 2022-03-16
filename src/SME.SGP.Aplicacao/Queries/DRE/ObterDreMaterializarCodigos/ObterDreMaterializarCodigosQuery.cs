using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterDreMaterializarCodigosQuery : IRequest<Tuple<IEnumerable<Dre>, string[]>>
    {
        public string[] IdDres { get; set; }

        public ObterDreMaterializarCodigosQuery(string[] idDres)
        {
            IdDres = idDres;
        }
    }

    public class ObterDreMaterializarCodigosQueryValidator : AbstractValidator<ObterDreMaterializarCodigosQuery>
    {
        public ObterDreMaterializarCodigosQueryValidator()
        {
            RuleFor(x => x.IdDres)
                .NotEmpty()
                .WithMessage("Íds das Dres devem ser informadas.");
        }
    }
}