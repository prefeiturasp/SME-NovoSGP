using FluentValidation;
using MediatR;
using SME.SGP.Dominio.Entidades;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterNotasParaConsolidacao
{
    public class ObterNotasParaConsolidacaoQuery : IRequest<IEnumerable<PainelEducacionalConsolidacaoNotaDadosBrutos>>
    {
        public ObterNotasParaConsolidacaoQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }
        public int AnoLetivo { get; set; }
    }

    public class ObterNotasParaConsolidacaoQueryValidator : AbstractValidator<ObterNotasParaConsolidacaoQuery>
    {
        public ObterNotasParaConsolidacaoQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo.");
        }
    }
}
