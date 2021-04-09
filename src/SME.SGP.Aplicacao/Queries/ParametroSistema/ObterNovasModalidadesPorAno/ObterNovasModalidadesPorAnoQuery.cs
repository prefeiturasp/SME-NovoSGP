using FluentValidation;
using MediatR;
using SME.SGP.Dominio;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao
{
    public class ObterNovasModalidadesPorAnoQuery : IRequest<IEnumerable<Modalidade>>
    {
        public ObterNovasModalidadesPorAnoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; }
    }

    public class ObterNovasModalidadesPorAnoQueryValidator : AbstractValidator<ObterNovasModalidadesPorAnoQuery>
    {
        public ObterNovasModalidadesPorAnoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
                .NotEmpty()
                .WithMessage("O ano letivo deve ser informado para consulta das novas modalidades.");
        }
    }
}