using FluentValidation;
using MediatR;
using SME.SGP.Infra.Dtos.PainelEducacional;
using System.Collections.Generic;

namespace SME.SGP.Aplicacao.Queries.PainelEducacional.ObterTurmasPainelEducacional
{
    public class ObterTurmasPainelEducacionalQuery : IRequest<IEnumerable<TurmaPainelEducacionalDto>>
    {
        public ObterTurmasPainelEducacionalQuery(int anoLetivo)
        {
            AnoLetivo = anoLetivo;
        }

        public int AnoLetivo { get; set; }
    }

    public class ObterTurmasPainelEducacionalQueryValidator : AbstractValidator<ObterTurmasPainelEducacionalQuery>
    {
        public ObterTurmasPainelEducacionalQueryValidator()
        {
            RuleFor(a => a.AnoLetivo)
               .NotEmpty()
               .WithMessage("É necessário informar o ano letivo.");
        }
    }
}
