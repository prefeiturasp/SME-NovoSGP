using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnexosRegistroColetivoQuery : IRequest<IEnumerable<AnexoDto>>
    {
        public ObterAnexosRegistroColetivoQuery(long registroColetivoId)
        {
            RegistroColetivoId = registroColetivoId;
        }

        public long RegistroColetivoId { get; set; }
    }

    public class ObterAnexosRegistroColetivoQueryValidator : AbstractValidator<ObterAnexosRegistroColetivoQuery>
    {
        public ObterAnexosRegistroColetivoQueryValidator()
        {
            RuleFor(a => a.RegistroColetivoId)
                .NotEmpty()
                .WithMessage("O Id do registro coletivo deve ser informado!");
        }
    }
}
