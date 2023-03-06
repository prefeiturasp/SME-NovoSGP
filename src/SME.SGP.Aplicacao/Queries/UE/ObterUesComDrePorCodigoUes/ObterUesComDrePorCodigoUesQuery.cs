using System.Collections.Generic;
using FluentValidation;
using MediatR;
using SME.SGP.Dominio;

namespace SME.SGP.Aplicacao
{
    public class ObterUesComDrePorCodigoUesQuery : IRequest<IEnumerable<Ue>>
    {
        public ObterUesComDrePorCodigoUesQuery(string[] uesCodigos)
        {
            UesCodigos = uesCodigos;
        }

        public string[] UesCodigos { get; set; }
    }


    public class ObterUesComDrePorCodigoUesQueryValidator : AbstractValidator<ObterUesComDrePorCodigoUesQuery>
    {
        public ObterUesComDrePorCodigoUesQueryValidator()
        {
            RuleFor(x => x.UesCodigos).NotEmpty().NotNull()
                .WithMessage("Informe pelo menos uma ue para consulta de ues");
        }
    }
}