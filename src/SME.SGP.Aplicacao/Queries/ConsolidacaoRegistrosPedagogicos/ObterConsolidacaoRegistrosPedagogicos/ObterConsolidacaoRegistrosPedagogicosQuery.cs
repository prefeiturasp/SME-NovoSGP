using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using SME.SGP.Infra;
using System;
using System.Collections.Generic;
using System.Text;

namespace SME.SGP.Aplicacao
{
    public class ObterConsolidacaoRegistrosPedagogicosQuery : IRequest<IEnumerable<ConsolidacaoRegistrosPedagogicosDto>>
    {
        public ObterConsolidacaoRegistrosPedagogicosQuery(long ueId, int anoLetivo)
        {
            UeId = ueId;
            AnoLetivo = anoLetivo;
        }

        public long UeId { get; }
        public int AnoLetivo { get; set; }
    }

    public class ObterConsolidacaoRegistrosPedagogicosQueryValidator : AbstractValidator<ObterConsolidacaoRegistrosPedagogicosQuery>
    {
        public ObterConsolidacaoRegistrosPedagogicosQueryValidator()
        {
            RuleFor(a => a.UeId)
                .NotEmpty()
                .WithMessage("O identificador da UE deve ser informado para consolidar os registros pedagógicos");

            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consolidar os registros pedagógicos");

        }
    }
}
