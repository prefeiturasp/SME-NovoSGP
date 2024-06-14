using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterAnexosPorInformativoIdQuery : IRequest<IEnumerable<InformativoAnexoDto>>
    {
        public ObterAnexosPorInformativoIdQuery(long informativoId)
        {
            InformativoId = informativoId;
        }
        public long InformativoId { get; }
    }

    public class ObterAnexosPorInformativoIdQueryValidator : AbstractValidator<ObterAnexosPorInformativoIdQuery>
    {
        public ObterAnexosPorInformativoIdQueryValidator()
        {
            RuleFor(c => c.InformativoId)
                .GreaterThan(0)
                .WithMessage("O Id do informativo deve ser informado para busca de anexos.");
        }
    }
}