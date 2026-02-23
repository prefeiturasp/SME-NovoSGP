using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional
{
    public class ObterTurmasEducacaoIntegralPainelEducacionalQuery : IRequest<IEnumerable<TurmaPainelEducacionalDto>>
    {
        public ObterTurmasEducacaoIntegralPainelEducacionalQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }

    public class ObterTurmasEducacaoIntegralPainelEducacionalQueryValidator : AbstractValidator<ObterTurmasEducacaoIntegralPainelEducacionalQuery>
    {
        public ObterTurmasEducacaoIntegralPainelEducacionalQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo.");
        }
    }
}
